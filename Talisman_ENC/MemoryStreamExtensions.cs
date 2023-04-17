using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talisman_ENC
{
    public static class MemoryStreamExtensions
    {
        #region Write Method
        //Write Method
        public static void Write(this MemoryStream stream, byte[] input)
        {
            stream.Write(input, 0, input.Length);
        }

        public static void Write(this MemoryStream stream, string input, Encoding encoding)
        {
            stream.Write(encoding.GetBytes(input), 0, encoding.GetBytes(input).Length);
        }

        public static void Write(this MemoryStream stream, double input)
        {
            stream.Write(BitConverter.GetBytes(input), 0, 8);
        }

        public static void Write(this MemoryStream stream, float input)
        {
            stream.Write(BitConverter.GetBytes(input), 0, 4);
        }

        public static void Write(this MemoryStream stream, Int64 input)
        {
            stream.Write(BitConverter.GetBytes(input), 0, 8);
        }

        public static void Write(this MemoryStream stream, int input)
        {
            stream.Write(BitConverter.GetBytes(input), 0, 4);
        }

        public static void Write(this MemoryStream stream, uint input)
        {
            stream.Write(BitConverter.GetBytes(input), 0, 4);
        }

        public static void Write(this MemoryStream stream, short input)
        {
            stream.Write(BitConverter.GetBytes(input), 0, 2);
        }

        public static void Write(this MemoryStream stream, byte input)
        {
            stream.WriteByte(input);
        }

        #endregion

        #region Read Method
        //Read Method
        public static byte[] ReadBytes(this MemoryStream stream, int size)
        {
            var data = new byte[size];
            stream.Read(data, 0, size);
            return data;
        }

        public static bool ReadBoolen(this MemoryStream stream)
        {
            return stream.ReadByte() == 0 ? false : true;
        }

        public static byte[] ReadToEnd(this MemoryStream stream)
        {
            var data = new byte[stream.Length - stream.Position];
            stream.Read(data, 0, (int)(stream.Length - stream.Position));
            return data;
        }

        public static byte[] ReadBytes(this MemoryStream stream, long size)
        {
            var data = new byte[size];
            stream.Read(data, 0, (int)size);
            return data;
        }

        public static Int16 ReadInt16(this MemoryStream stream)
        {
            var data = new byte[2];
            stream.Read(data, 0, 2);
            return BitConverter.ToInt16(data, 0);
        }

        public static UInt16 ReadUInt16(this MemoryStream stream)
        {
            var data = new byte[2];
            stream.Read(data, 0, 2);
            return BitConverter.ToUInt16(data, 0);
        }

        public static Int32 ReadInt32(this MemoryStream stream)
        {
            var data = new byte[4];
            stream.Read(data, 0, 4);
            return BitConverter.ToInt32(data, 0);
        }

        public static UInt32 ReadUInt32(this MemoryStream stream)
        {
            var data = new byte[4];
            stream.Read(data, 0, 4);
            return BitConverter.ToUInt32(data, 0);
        }

        public static Int64 ReadInt64(this MemoryStream stream)
        {
            var data = new byte[8];
            stream.Read(data, 0, 8);
            return BitConverter.ToInt64(data, 0);
        }

        public static UInt64 ReadUInt64(this MemoryStream stream)
        {
            var data = new byte[8];
            stream.Read(data, 0, 8);
            return BitConverter.ToUInt64(data, 0);
        }

        public static float ReadSingle(this MemoryStream stream)
        {
            var data = new byte[4];
            stream.Read(data, 0, 4);
            return BitConverter.ToSingle(data, 0);
        }

        public static string ReadString(this MemoryStream stream, int size, Encoding encoding)
        {
            var data = new byte[size];
            stream.Read(data, 0, size);
            return encoding.GetString(data);
        }

        public static string ReadString(this MemoryStream stream)
        {
            List<byte> output = new List<byte>();
            byte reader = (byte)stream.ReadByte();
            do
            {
                output.Add(reader);
                reader = (byte)stream.ReadByte();
            } while (reader != 0);
            stream.Seek(-1, SeekOrigin.Current);
            return Encoding.ASCII.GetString(output.ToArray());
        }
        #endregion

        #region Location Method
        //Location Method
        public static void Insert(this MemoryStream stream, byte[] input, int offset)
        {
            var buffer = new byte[4096];
            var length = stream.Length;
            stream.SetLength(stream.Length + input.LongLength);
            long pos = length;
            int next_pos;
            while (pos > offset)
            {
                next_pos = pos - 4096 >= offset ? 4096 : (int)(pos - offset);
                pos -= next_pos;
                stream.Position = pos;
                stream.Read(buffer, 0, next_pos);
                stream.Position = pos + input.LongLength;
                stream.Write(buffer, 0, next_pos);
            }
            stream.Position = offset;
            stream.Write(input, 0, input.Length);
            stream.Flush();
        }

        public static void Remove(this MemoryStream stream, int size, int offset)
        {
            var buffer = new byte[4096];
            var length = stream.Length;
            long pos = offset + size;
            int next_pos;
            while (pos < length)
            {
                next_pos = pos + 4096 <= length ? 4096 : (int)(length - pos);
                stream.Position = pos;
                stream.Read(buffer, 0, next_pos);
                stream.Position = pos - size;
                stream.Write(buffer, 0, next_pos);
                pos += next_pos;
            }
            stream.SetLength(length - size);
            stream.Flush();
        }

        public static void Skip(this MemoryStream stream, int to)
        {
            stream.Seek(to, SeekOrigin.Current);
        }

        public static void Skip(this MemoryStream stream, long to)
        {
            stream.Seek(to, SeekOrigin.Current);
        }


        public static long Tell(this MemoryStream stream)
        {
            return stream.Position;
        }

        public static int Tell32(this MemoryStream stream)
        {
            return (int)stream.Position;
        }

        public static void Seek(this MemoryStream stream, long to)
        {
            stream.Seek(to, SeekOrigin.Begin);
        }
        #endregion
    }
}
