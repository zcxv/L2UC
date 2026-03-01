using System;
using System.Buffers.Binary;
using System.Text;

namespace IPC {

    public static class IpcLinkedBufferWriter {

        public static void WriteC(this IpcLinkedBuffer buffer, int value) => buffer.WriteByte((byte) value);
        public static void WriteF(this IpcLinkedBuffer buffer, double value) => buffer.WriteDouble(value);
        public static void WriteH(this IpcLinkedBuffer buffer, int value) => buffer.WriteShort((short) value);
        public static void WriteD(this IpcLinkedBuffer buffer, int value) => buffer.WriteInt(value);
        public static void WriteQ(this IpcLinkedBuffer buffer, long value) => buffer.WriteLong(value);
        public static void WriteB(this IpcLinkedBuffer buffer, byte[] value) => buffer.WriteBytes(value);
        
        public static void WriteS(this IpcLinkedBuffer buffer, string value) {
            foreach (char c in value) {
                buffer.WriteUShort(c);
            }
            buffer.WriteUShort(0x00);
        }

    }

}