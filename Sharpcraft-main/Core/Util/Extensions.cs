using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

public static class Extensions
{
    public static LinkedListNode<T> GetNodeAt<T>(this LinkedList<T> list, int index)
    {
        if (index < (list.Count >> 1))
        {
            LinkedListNode<T> x = list.First;
            for (int i = 0; i < index; i++)
                x = x.Next;
            return x;
        }
        else
        {
            LinkedListNode<T> x = list.Last;
            for (int i = list.Count - 1; i > index; i--)
                x = x.Previous;
            return x;
        }
    }

    public static void WriteBytes(this BufferedStream stream, byte[] bytes)
    {
        stream.Write(bytes, 0, bytes.Length);
    }

    public static void WriteBytes(this BinaryWriter writer, byte[] bytes)
    {
        writer.Write(bytes, 0, bytes.Length);
    }

    public static void WriteJavaChar(this BinaryWriter writer, char value)
    {
        writer.Write((byte)((value >> 8) & 0xFF));
        writer.Write((byte)(value & 0xFF));
    }

    public static void WriteBEShort(this BinaryWriter writer, short value)
    {
        writer.Write(IPAddress.HostToNetworkOrder(value));
    }

    public static void WriteBEInt(this BinaryWriter writer, int value)
    {
        writer.Write(IPAddress.HostToNetworkOrder(value));
    }

    public static void WriteBELong(this BinaryWriter writer, long value)
    {
        writer.Write(IPAddress.HostToNetworkOrder(value));
    }

    public static void WriteBEFloat(this BinaryWriter writer, float value)
    {
        writer.WriteBEInt(BitConverter.SingleToInt32Bits(value));
    }

    public static void WriteBEDouble(this BinaryWriter writer, double value)
    {
        writer.WriteBELong(BitConverter.DoubleToInt64Bits(value));
    }

    //writes a 'modified utf-8 string' to the writer
    //this is not going to be compatible with non-ascii characters probably
    public static void WriteMUTF8(this BinaryWriter writer, string str)
    {
        byte[] data = Encoding.UTF8.GetBytes(str);
        short length = (short)data.Length;
        writer.WriteBEShort(length);
        writer.WriteBytes(data);
    }

    //reads a 'modified utf-8 string' from the reader
    //this is not going to be compatible with non-ascii characters probably
    public static string ReadMUTF8(this BinaryReader reader)
    {
        short length = reader.ReadBEShort();
        byte[] data = reader.ReadBytes(length);
        if (data.Length < length) throw new EndOfStreamException();
        return Encoding.UTF8.GetString(data);
    }

    //mc-specific version of utf-16be where the maximum length is 32k
    public static void WriteUTF16BE(this BinaryWriter writer, string str)
    {
        if (str.Length > 32767)
            throw new IOException("String too big");
        else
        {
            writer.WriteBEShort((short)str.Length);
            byte[] b = Encoding.BigEndianUnicode.GetBytes(str);
            writer.Write(b, 0, b.Length);
        }
    }

    //mc-specific version of utf-16be with max length specified
    public static string ReadUTF16BE(this BinaryReader reader, int maxSize)
    {
        short size = reader.ReadBEShort();

        if (size > maxSize)
            throw new IOException($"Received string length longer than maximum allowed ({size} > {maxSize})");
        else if (size < 0)
            throw new IOException("Received string length is less than zero! Weird string!");
        else
        {
            //the 'size' variable is the character count, not the actual byte count
            //2 byte character encodings my behated
            int actualSize = size * 2;
            string str = Encoding.BigEndianUnicode.GetString(reader.ReadBytes(actualSize));
            return str;
        }
    }

    public static char ReadJavaChar(this BinaryReader reader)
    {
        byte ch1 = reader.ReadByte();
        byte ch2 = reader.ReadByte();
        return (char)((ch1 << 8) + ch2);
    }

    public static short ReadBEShort(this BinaryReader reader)
    {
        return IPAddress.NetworkToHostOrder(reader.ReadInt16());
    }

    public static int ReadBEInt(this BinaryReader reader)
    {
        return IPAddress.NetworkToHostOrder(reader.ReadInt32());
    }

    public static long ReadBELong(this BinaryReader reader)
    {
        return IPAddress.NetworkToHostOrder(reader.ReadInt64());
    }

    public static float ReadBEFloat(this BinaryReader reader)
    {
        return BitConverter.Int32BitsToSingle(reader.ReadBEInt());
    }

    public static double ReadBEDouble(this BinaryReader reader)
    {
        return BitConverter.Int64BitsToDouble(reader.ReadBELong());
    }

    public static void ReadFully(this BinaryReader reader, byte[] buffer, int off, int len)
    {
        if (len < 0)
            throw new IndexOutOfRangeException();
        int n = 0;
        while (n < len)
        {
            int count = reader.Read(buffer, off + n, len - n);
            if (count < 0)
                throw new EndOfStreamException();
            n += count;
        }
    }

    public static void ReadFully(this BinaryReader reader, byte[] buffer)
    {
        reader.ReadFully(buffer, 0, buffer.Length);
    }

    public static void PrintStackTrace(this Exception ex)
    {
        Console.Error.WriteLine(ex);
    }
}
