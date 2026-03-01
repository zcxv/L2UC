using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace IPC {

    public static class IpcLinkedBufferReader {

        public static int ReadC(this IpcLinkedBuffer buffer) => buffer.ReadByte() & 0xff;
        public static double ReadF(this IpcLinkedBuffer buffer) => buffer.ReadDouble();
        public static int ReadH(this IpcLinkedBuffer buffer) => buffer.ReadShort() & 0xffff;
        public static int ReadD(this IpcLinkedBuffer buffer) => buffer.ReadInt();
        public static long ReadQ(this IpcLinkedBuffer buffer) => buffer.ReadLong();
        public static byte[] ReadB(this IpcLinkedBuffer buffer, byte[] value) => buffer.ReadBytes(value, 0, value.Length);
        public static byte[] ReadB(this IpcLinkedBuffer buffer, byte[] value, int offset, int length) => buffer.ReadBytes(value, offset, length);

        public static string ReadS(this IpcLinkedBuffer buffer) {
            var chars = new List<char>();
            ushort c;
            while ((c = buffer.ReadUShort()) != 0x00) {
                chars.Add((char)c);
            }
            
            return new string(chars.ToArray());
        }

    }

}