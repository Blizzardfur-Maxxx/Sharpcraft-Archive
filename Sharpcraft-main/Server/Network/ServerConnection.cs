using SharpCraft.Core;
using SharpCraft.Core.Util;
using SharpCraft.Core.Util.Logging;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCraft.Server.Network
{
    public class ServerConnection
    {
        private static Logger logger = Logger.GetLogger(SharedConstants.LOGGER_NS);
        private Socket listeningSocket;
        private Thread listenThread;
        public volatile bool running = false;
        private int connectionCount = 0;
        private List<PendingConnection> pendingConnections = new List<PendingConnection>();
        private List<PlayerConnection> playerConnections = new List<PlayerConnection>();
        private Server mcServer;

        public ServerConnection(Server server, IPAddress address, int port)
        {
            this.mcServer = server;
            this.listeningSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            this.listeningSocket.Bind(new IPEndPoint(address, port));
            this.listeningSocket.Listen();
            this.running = true;
            this.listenThread = new Thread(new ThreadStart(() =>
            {
                Dictionary<EndPoint, long> tstamps = new Dictionary<EndPoint, long>();

                while(running)
                {
                    try
                    {
                        Socket sock = listeningSocket.Accept();
                        if (sock != null)
                        {
                            EndPoint sockaddr = sock.RemoteEndPoint;
                            if (tstamps.TryGetValue(sockaddr, out long time) && !((IPEndPoint)sockaddr).Address.Equals(IPAddress.Loopback) && TimeUtil.MilliTime - time < 5000L)
                            {
                                tstamps[sockaddr] = TimeUtil.MilliTime;
                                sock.Close();
                            }
                            else
                            {
                                tstamps[sockaddr] = TimeUtil.MilliTime;
                                PendingConnection conn = new PendingConnection(server, sock, "Connection #" + connectionCount++);
                                AddPendingConnection(conn);
                            }
                        }

                    }
                    catch (IOException ioe)
                    {
                        ioe.PrintStackTrace();
                    }
                }
            }));
            this.listenThread.Name = "Listen thread";
            this.listenThread.Start();

        }

        public void AddPlayerConnection(PlayerConnection conn) 
        {
            playerConnections.Add(conn);
        }

        private void AddPendingConnection(PendingConnection pendingconnection)
        {
            if (pendingconnection == null)
            {
                throw new ArgumentException("Got null pendingconnection");
            }
            else 
            {
                pendingConnections.Add(pendingconnection);
            }
        }

        public void Tick()
        {
            int i1;
            for (i1 = 0; i1 < this.pendingConnections.Count; ++i1)
            {
                PendingConnection conn = this.pendingConnections[i1];

                try
                {
                    conn.TryLogin();
                }
                catch (Exception e)
                {
                    conn.KickUser("Internal server error");
                    logger.Log(LogLevel.WARNING, "Failed to handle packet: " + e, e);
                }

                if (conn.finishedProcessing)
                {
                    this.pendingConnections.RemoveAt(i1--);
                }

                conn.netConnection.Interrupt();
            }

            for (i1 = 0; i1 < this.playerConnections.Count; ++i1)
            {
                PlayerConnection pconn = this.playerConnections[i1];

                try
                {
                    pconn.Tick();
                }
                catch (Exception e)
                {
                    logger.Log(LogLevel.WARNING, "Failed to handle packet: " + e, e);
                    pconn.KickPlayer("Internal server error");
                }

                if (pconn.connectionClosed)
                {
                    this.playerConnections.RemoveAt(i1--);
                }

                pconn.netManager.Interrupt();
            }
        }
    }
}
