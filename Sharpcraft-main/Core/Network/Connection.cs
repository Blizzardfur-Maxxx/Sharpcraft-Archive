#define DISABLE_PRINTSTACKTRACE
using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SharpCraft.Core.Network
{
    public class Connection
    {
        private const int RW_SLEEP_INTERVAL = 100; //default is 100ms
        private const int WRITE_STREAM_BUFFER_SIZE = 5120;
        public static readonly object numThreadsLock = new object();
        public static int numReadThreads;
        public static int numWriteThreads;
        private object sendQueueLock = new object();
        private Socket networkSocket;
        private readonly EndPoint remoteSocketAddress;
        private BinaryReader socketInputStream;
        private BinaryWriter socketOutputStream;
        private bool running = true;
        private IList<Packet> readPackets = new SynchronizedCollection<Packet>();//Collections.synchronizedList(new ArrayList<>());
        private IList<Packet> dataPackets = new SynchronizedCollection<Packet>();//Collections.synchronizedList(new ArrayList<>());
        private IList<Packet> chunkDataPackets = new SynchronizedCollection<Packet>();//Collections.synchronizedList(new ArrayList<>());
        private PacketListener packetListener;
        private bool closing = false;
        private Thread writeThread;
        private Thread readThread;
        private bool isTerminating = false;
        private string format_string = "";
        private object[] format_args;
        private int timeSinceLastRead = 0;
        private int sendQueueByteLength = 0;
        public int chunkDataSendCounter = 0;
        private int counter = 50;
        private volatile bool canInteruptRead;
        private volatile bool canInteruptWrite;

        private static bool IsEmpty<T>(IList<T> list) //small helper method to ease us in porting, i don't think you can use extensions on interfaces?
        {
            return list.Count == 0;
        }

        public Connection(Socket socket, string connName, PacketListener listener)
        {
            this.networkSocket = socket;
            remoteSocketAddress = socket.RemoteEndPoint;
            this.packetListener = listener;
            socket.ReceiveTimeout = 30000;
            SocketOptionLevel sol = SocketOptionLevel.IP;
            if (remoteSocketAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                sol = SocketOptionLevel.IPv6;
            }
            try
            {
                //this shit throws on older Windows
                socket.SetSocketOption(sol, SocketOptionName.TypeOfService, 24);
            }
            catch (Exception se)
            {
                Console.Error.WriteLine("warn: failed to set socket options: "+se);
            }

            Stream sockStrm = new NetworkStream(socket, true);
            socketInputStream = new BinaryReader(sockStrm);
            socketOutputStream = new BinaryWriter(new BufferedStream(sockStrm, WRITE_STREAM_BUFFER_SIZE));
            readThread = new Thread(new ThreadStart(() =>
            {
                lock (numThreadsLock)
                {
                    ++numReadThreads;
                }

                while (true)
                {
                    if (!running)
                    {
                        break;
                    }

                    if (closing)
                    {
                        break;
                    }

                    while (ReadPacket()) ;

                    try
                    {
                        canInteruptRead = true;
                        Thread.Sleep(RW_SLEEP_INTERVAL);
                        canInteruptRead = false;
                    }
                    catch (ThreadInterruptedException) { canInteruptRead = false; }

                }

                lock (numThreadsLock)
                {
                    --numReadThreads;
                }
            }));
            readThread.Name = $"{connName} read thread";
            writeThread = new Thread(new ThreadStart(() =>
            {
                lock (numThreadsLock)
                {
                    ++numWriteThreads;
                }

                while (true)
                {
                    if (!running)
                    {
                        break;
                    }

                    while (SendPacket()) ;

                    try
                    {
                        canInteruptWrite = true;
                        Thread.Sleep(RW_SLEEP_INTERVAL);
                        canInteruptWrite = false;
                    }
                    catch (ThreadInterruptedException) { canInteruptWrite = false; }


                    try
                    {
                        if (this.socketOutputStream != null)
                        {
                            this.socketOutputStream.Flush();
                        }
                    }
                    catch (Exception ioe)
                    {
                        if (!isTerminating)
                        {
                            HandleException(ioe);
                        }

                        ExPrintstacktrace(ioe);
                    }
                }

                lock (numThreadsLock)
                {
                    --numWriteThreads;
                }

            }));
            writeThread.Name = $"{connName} write thread";

            readThread.Start();
            writeThread.Start();
        }

        public void SetPacketListener(PacketListener listener) { packetListener = listener; }

        public void AddToSendQueue(Packet pack)
        {
            if (!closing)
            {
                lock (sendQueueLock)
                {
                    sendQueueByteLength += pack.Size();
                    if (pack.isChunkDataPacket)
                    {
                        chunkDataPackets.Add(pack);
                    }
                    else
                    {
                        dataPackets.Add(pack);
                    }
                }
            }
        }

        private bool SendPacket()
        {
            bool ret = false;

            try
            {
                Packet pack;
                if (!IsEmpty(this.dataPackets) && (this.chunkDataSendCounter == 0 || TimeUtil.MilliTime - this.dataPackets[0].creationTimeMillis >= this.chunkDataSendCounter))
                {
                    lock (this.sendQueueLock)
                    {
                        pack = this.dataPackets[0];
                        dataPackets.RemoveAt(0);
                        this.sendQueueByteLength -= pack.Size() + 1;
                    }

                    Packet.WritePacket(pack, this.socketOutputStream);
                    //Console.Error.WriteLine("send: " + pack);
                    ret = true;
                }

                if (this.counter-- <= 0 && !IsEmpty(this.chunkDataPackets) && (this.chunkDataSendCounter == 0 || TimeUtil.MilliTime - this.chunkDataPackets[0].creationTimeMillis >= this.chunkDataSendCounter))
                {
                    lock (this.sendQueueLock)
                    {
                        pack = this.chunkDataPackets[0];
                        chunkDataPackets.RemoveAt(0);
                        this.sendQueueByteLength -= pack.Size() + 1;
                    }
                    
                    Packet.WritePacket(pack, this.socketOutputStream);
                    //Console.Error.WriteLine("send: " + pack);
                    this.counter = 0;
                    ret = true;
                }

                return ret;
            }
            catch (Exception exception8)
            {
                if (!this.isTerminating)
                {
                    this.HandleException(exception8);
                }

                return false;
            }
        }

        public void Interrupt()
        {
            if (canInteruptRead)
            {
                readThread.Interrupt();
            }
            //if (canInteruptWrite) 
            //{
            //    writeThread.Interrupt();
            //}
        }

        private bool ReadPacket()
        {
            bool ret = false;
            try
            {
                Packet pack = Packet.ReadPacket(this.socketInputStream, this.packetListener.IsServerPacketListener());
                if (pack != null)
                {
                    //Console.Error.WriteLine("read: " + pack);
                    this.readPackets.Add(pack);
                    ret = true;
                }
                else
                {
                    this.CloseConnection("disconnect.endOfStream");
                }

                return ret;
            }
            catch (Exception exception3)
            {
                if (!this.isTerminating)
                {
                    this.HandleException(exception3);
                }

                return false;
            }
        }

        private void HandleException(Exception ex)
        {
            //ExPrintstacktrace(ex);
            ex.PrintStackTrace();
            CloseConnection("disconnect.genericReason", $"Internal exception: {ex}");
        }

        public void CloseConnection(string formatStr, params object[] args)
        {
            if (this.running)
            {
                isTerminating = true;
                format_string = formatStr;
                format_args = args;

                //this shit will work on older language versions but not modern .NET so leaving this as is
                #pragma warning disable SYSLIB0006
                new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        Thread.Sleep(5000);
                        if (readThread.IsAlive)
                        {
                            try
                            {
                                readThread.Abort();
                            }
                            catch (Exception)
                            { }
                            Console.Error.WriteLine("warn: read thread is still running!");
                        }

                        if (writeThread.IsAlive)
                        {
                            try
                            {
                                writeThread.Abort();
                            }
                            catch (Exception)
                            { }
                            Console.Error.WriteLine("warn: write thread is still running!");
                        }
                    }
                    catch (ThreadInterruptedException) { }


                })).Start();
                #pragma warning restore SYSLIB0006

                running = false;

                try
                {
                    socketInputStream.Dispose();
                    socketInputStream = null;

                    socketOutputStream.Dispose();
                    socketOutputStream = null;

                    networkSocket.Dispose();
                    networkSocket = null;
                }
                catch 
                {
                    //Console.Error.WriteLine("failed to dispose shit?");
                }
               
            }
        }

        public void Tick()
        {
            if (this.sendQueueByteLength > 1048576)
            {
                //fuck you
                //this.CloseConnection("disconnect.overflow");
            }

            if (IsEmpty(this.readPackets))
            {
                if (this.timeSinceLastRead++ == 1200)
                {
                    this.CloseConnection("disconnect.timeout");
                }
            }
            else
            {
                this.timeSinceLastRead = 0;
            }

            int cnt = 100;

            while (!IsEmpty(this.readPackets) && cnt-- >= 0)
            {
                Packet packet2 = this.readPackets[0];
                readPackets.RemoveAt(0);
                packet2.Handle(this.packetListener);
            }

            this.Interrupt();
            if (this.isTerminating && IsEmpty(this.readPackets))
            {
                this.packetListener.OnDisconnect(this.format_string, this.format_args);
            }
        }

        public EndPoint GetRemoteAddress() { return remoteSocketAddress; }

        public void Shutdown()
        {
            Interrupt();
            closing = true;
            readThread.Interrupt();
            new Thread(new ThreadStart(() => {
                try
                {
                    Thread.Sleep(2000);
                    if (running)
                    {
                        writeThread.Interrupt();
                        CloseConnection("disconnect.closed");
                    }
                }
                catch (Exception) { }
            })).Start();
        }

        public int GetNumChunkDataPackets() { return chunkDataPackets.Count; }

        private static void ExPrintstacktrace(Exception ex) 
        {
#if DISABLE_PRINTSTACKTRACE 
            return;
#else
            ex.PrintStackTrace();
#endif
        }
    }
}