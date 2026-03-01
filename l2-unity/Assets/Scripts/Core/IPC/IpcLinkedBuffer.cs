using System;
using System.Buffers;
using System.Buffers.Binary;

namespace IPC {
    public class IpcLinkedBuffer : IDisposable {

        private const int PAGE_SIZE = 4096;

        internal class Buffer {
            public readonly byte[] Data;

            public Buffer Next { get; set; }

            public Buffer(byte[] data) {
                Data = data;
            }
        }

        public int Capacity { get; }

        public int WritePosition { get; set; }

        public int ReadPosition { get; set; }

        public int Limit {
            get => limit;
            set {
                if (value < 0) {
                    limit = 0;
                } else if (value > Capacity) {
                    limit = Capacity;
                } else {
                    limit = value;
                }

                if (WritePosition > limit) {
                    WritePosition = limit;
                }

                if (ReadPosition > limit) {
                    ReadPosition = limit;
                }
            }
        }

        public bool BigEndian { get; set; }

        private Buffer head;

        private Buffer lastChunk;
        private int lastChunkIndex;

        private int limit;

        public IpcLinkedBuffer(int capacity) {
            Capacity = PAGE_SIZE * (GetChunkNumber(capacity) + 1);
            limit = capacity;
            head = lastChunk = AllocBuffer();
            lastChunkIndex = 0;
        }

        private Buffer AllocBuffer() {
            return new Buffer(ArrayPool<byte>.Shared.Rent(PAGE_SIZE));
        }

        public void Dispose() {
            for (var next = head; next != null; next = next.Next) {
                ArrayPool<byte>.Shared.Return(next.Data);
            }

            head = null;
        }

        private int GetChunkNumber(int position) => position / PAGE_SIZE;

        private int GetChunkPosition(int position) => position % PAGE_SIZE;

        private Buffer GetChunk(int position) {
            int chunkIndex = GetChunkNumber(position);
            if (chunkIndex == lastChunkIndex) {
                return lastChunk;
            }

            if (chunkIndex == lastChunkIndex + 1 && lastChunk.Next != null) {
                lastChunkIndex++;
                lastChunk = lastChunk.Next;
                return lastChunk;
            }

            var buffer = head;
            for (int i = 0; i < chunkIndex; i++) {
                buffer = buffer.Next;
                if (buffer == null) {
                    return null;
                }
            }

            lastChunk = buffer;
            lastChunkIndex = chunkIndex;

            return buffer;
        }

        private Buffer GetChunkOrAllocate(int position, int sizeOf) {
            Buffer chunk = GetChunk(position);
            if (chunk == null) {
                AllocToSize(position + sizeOf);
                chunk = GetChunk(position);
            }

            return chunk;
        }

        private void AllocToSize(int size) {
            if (size > Capacity) {
                throw new IndexOutOfRangeException($"Overflow buffer capacity: {size} > {Capacity}");
            }

            var buffer = head;
            for (int i = 1; i < (size + PAGE_SIZE - 1) / PAGE_SIZE; i++) {
                buffer = (buffer.Next ??= AllocBuffer());
            }
        }

        private void CheckLimit(int position, int sizeOf) {
            if (position + sizeOf > limit) {
                throw new IndexOutOfRangeException($"Overflow buffer limit: {position} > {limit}");
            }
        }

        public void Flip() {
            limit = WritePosition;
            Reset();
        }

        public void Reset() {
            WritePosition = 0;
            ReadPosition = 0;
        }

        public void WriteByte(byte value) {
            WriteByte(WritePosition, value);
            WritePosition += sizeof(byte);
        }

        public void WriteByte(int position, byte value) {
            CheckLimit(position, sizeof(byte));

            Buffer chunk = GetChunkOrAllocate(position, sizeof(byte)) ?? throw new IndexOutOfRangeException();
            int relativePosition = GetChunkPosition(position);
            chunk.Data[relativePosition] = value;
        }

        public byte ReadByte() {
            try {
                return ReadByte(ReadPosition);
            } finally {
                ReadPosition += sizeof(byte);
            }
        }

        public byte ReadByte(int position) {
            CheckLimit(position, sizeof(byte));

            Buffer chunk = GetChunk(position) ?? throw new IndexOutOfRangeException();
            int relativePosition = GetChunkPosition(position);
            return chunk.Data[relativePosition];
        }
        
        public void WriteBytes(byte[] bytes) {
            WriteBytes(WritePosition, bytes, 0, bytes.Length);
            WritePosition += bytes.Length;
        }
        
        public void WriteBytes(int position, byte[] bytes) => WriteBytes(position, bytes, 0, bytes.Length);

        public void WriteBytes(byte[] bytes, int offset, int length) {
            WriteBytes(WritePosition, bytes, offset, length);
            WritePosition += length;
        }

        public void WriteBytes(int position, byte[] bytes, int offset, int length) {
            CheckLimit(position, length);
            int relativePosition = GetChunkPosition(position);
            
            var temp = bytes.AsSpan(offset, length);
            if (relativePosition + temp.Length <= PAGE_SIZE) {
                Buffer chunk = GetChunkOrAllocate(position, temp.Length) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition);
                temp.CopyTo(span);
            } else {
                for (int i = 0; i < temp.Length; i++) {
                    WriteByte(position + i, temp[i]);
                }
            }
        }

        public byte[] ReadBytes(byte[] bytes) {
            try {
                return ReadBytes(ReadPosition, bytes, 0, bytes.Length);
            } finally {
                ReadPosition += bytes.Length;
            }
        }

        public byte[] ReadBytes(int position, byte[] bytes) => ReadBytes(position, bytes, 0, bytes.Length);

        public byte[] ReadBytes(byte[] bytes, int offset, int length) {
            try {
                return ReadBytes(ReadPosition, bytes, offset, length);
            } finally {
                ReadPosition += length;
            }
        }

        public byte[] ReadBytes(int position, byte[] bytes, int offset, int length) {
            CheckLimit(offset, length);
            int relativePosition = GetChunkPosition(position);
            
            var temp = bytes.AsSpan(offset, length);
            if (relativePosition + temp.Length <= PAGE_SIZE) {
                Buffer chunk = GetChunkOrAllocate(position, temp.Length) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition, length);
                span.CopyTo(temp);
            } else {
                for (int i = 0; i < temp.Length; i++) {
                    temp[i] = ReadByte(position + i);
                }
            }

            return bytes;
        }

        public void WriteShort(short value) {
            WriteShort(WritePosition, value);
            WritePosition += sizeof(short);
        }

        public void WriteShort(int position, short value) {
            CheckLimit(position, sizeof(short));
            int relativePosition = GetChunkPosition(position);

            if (relativePosition + sizeof(short) <= PAGE_SIZE) {
                Buffer chunk = GetChunkOrAllocate(position, sizeof(short)) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition);
                if (BigEndian) {
                    BinaryPrimitives.WriteInt16BigEndian(span, value);
                } else {
                    BinaryPrimitives.WriteInt16LittleEndian(span, value);
                }
            } else {
                Span<byte> temp = stackalloc byte[sizeof(short)];
                if (BigEndian) {
                    BinaryPrimitives.WriteInt16BigEndian(temp, value);
                } else {
                    BinaryPrimitives.WriteInt16LittleEndian(temp, value);
                }

                for (int i = 0; i < temp.Length; i++) {
                    WriteByte(position + i, temp[i]);
                }
            }
        }

        public short ReadShort() {
            try {
                return ReadShort(ReadPosition);
            } finally {
                ReadPosition += sizeof(short);
            }
        }

        public short ReadShort(int position) {
            CheckLimit(position, sizeof(short));
            int relativePosition = GetChunkPosition(position);

            if (relativePosition + sizeof(short) <= PAGE_SIZE) {
                Buffer chunk = GetChunk(position) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition);
                return BigEndian ? BinaryPrimitives.ReadInt16BigEndian(span) : BinaryPrimitives.ReadInt16LittleEndian(span);
            }

            Span<byte> temp = stackalloc byte[sizeof(short)];
            for (int i = 0; i < temp.Length; i++) {
                temp[i] = ReadByte(position + i);
            }

            return BigEndian ? BinaryPrimitives.ReadInt16BigEndian(temp) : BinaryPrimitives.ReadInt16LittleEndian(temp);
        }
        
        public void WriteUShort(ushort value) {
            WriteUShort(WritePosition, value);
            WritePosition += sizeof(ushort);
        }

        public void WriteUShort(int position, ushort value) {
            CheckLimit(position, sizeof(ushort));
            int relativePosition = GetChunkPosition(position);

            if (relativePosition + sizeof(ushort) <= PAGE_SIZE) {
                Buffer chunk = GetChunkOrAllocate(position, sizeof(ushort)) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition);
                if (BigEndian) {
                    BinaryPrimitives.WriteUInt16BigEndian(span, value);
                } else {
                    BinaryPrimitives.WriteUInt16LittleEndian(span, value);
                }
            } else {
                Span<byte> temp = stackalloc byte[sizeof(ushort)];
                if (BigEndian) {
                    BinaryPrimitives.WriteUInt16BigEndian(temp, value);
                } else {
                    BinaryPrimitives.WriteUInt16LittleEndian(temp, value);
                }

                for (int i = 0; i < temp.Length; i++) {
                    WriteByte(position + i, temp[i]);
                }
            }
        }
        
        public ushort ReadUShort() {
            try {
                return ReadUShort(ReadPosition);
            } finally {
                ReadPosition += sizeof(ushort);
            }
        }

        public ushort ReadUShort(int position) {
            CheckLimit(position, sizeof(ushort));
            int relativePosition = GetChunkPosition(position);

            if (relativePosition + sizeof(ushort) <= PAGE_SIZE) {
                Buffer chunk = GetChunk(position) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition);
                return BigEndian ? BinaryPrimitives.ReadUInt16BigEndian(span) : BinaryPrimitives.ReadUInt16LittleEndian(span);
            }

            Span<byte> temp = stackalloc byte[sizeof(ushort)];
            for (int i = 0; i < temp.Length; i++) {
                temp[i] = ReadByte(position + i);
            }

            return BigEndian ? BinaryPrimitives.ReadUInt16BigEndian(temp) : BinaryPrimitives.ReadUInt16LittleEndian(temp);
        }

        public void WriteInt(int value) {
            WriteInt(WritePosition, value);
            WritePosition += sizeof(int);
        }

        public void WriteInt(int position, int value) {
            CheckLimit(position, sizeof(int));
            int relativePosition = GetChunkPosition(position);

            if (relativePosition + sizeof(int) <= PAGE_SIZE) {
                Buffer chunk = GetChunkOrAllocate(position, sizeof(int)) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition);
                if (BigEndian) {
                    BinaryPrimitives.WriteInt32BigEndian(span, value);
                } else {
                    BinaryPrimitives.WriteInt32LittleEndian(span, value);
                }
            } else {
                Span<byte> temp = stackalloc byte[sizeof(int)];
                if (BigEndian) {
                    BinaryPrimitives.WriteInt32BigEndian(temp, value);
                } else {
                    BinaryPrimitives.WriteInt32LittleEndian(temp, value);
                }

                for (int i = 0; i < temp.Length; i++) {
                    WriteByte(position + i, temp[i]);
                }
            }
        }

        public int ReadInt() {
            try {
                return ReadInt(ReadPosition);
            } finally {
                ReadPosition += sizeof(int);
            }
        }

        public int ReadInt(int position) {
            CheckLimit(position, sizeof(int));
            int relativePosition = GetChunkPosition(position);

            if (relativePosition + sizeof(int) <= PAGE_SIZE) {
                Buffer chunk = GetChunk(position) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition);
                return BigEndian ? BinaryPrimitives.ReadInt32BigEndian(span) : BinaryPrimitives.ReadInt32LittleEndian(span);
            }

            Span<byte> temp = stackalloc byte[sizeof(int)];
            for (int i = 0; i < temp.Length; i++) {
                temp[i] = ReadByte(position + i);
            }

            return BigEndian ? BinaryPrimitives.ReadInt32BigEndian(temp) : BinaryPrimitives.ReadInt32LittleEndian(temp);
        }

        public void WriteLong(long value) {
            WriteLong(WritePosition, value);
            WritePosition += sizeof(long);
        }

        public void WriteLong(int position, long value) {
            CheckLimit(position, sizeof(long));
            int relativePosition = GetChunkPosition(position);

            if (relativePosition + sizeof(long) <= PAGE_SIZE) {
                Buffer chunk = GetChunkOrAllocate(position, sizeof(long)) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition);
                if (BigEndian) {
                    BinaryPrimitives.WriteInt64BigEndian(span, value);
                } else {
                    BinaryPrimitives.WriteInt64LittleEndian(span, value);
                }
            } else {
                Span<byte> temp = stackalloc byte[sizeof(long)];
                if (BigEndian) {
                    BinaryPrimitives.WriteInt64BigEndian(temp, value);
                } else {
                    BinaryPrimitives.WriteInt64LittleEndian(temp, value);
                }

                for (int i = 0; i < temp.Length; i++) {
                    WriteByte(position + i, temp[i]);
                }
            }
        }

        public long ReadLong() {
            try {
                return ReadLong(ReadPosition);
            } finally {
                ReadPosition += sizeof(long);
            }
        }

        public long ReadLong(int position) {
            CheckLimit(position, sizeof(long));
            int relativePosition = GetChunkPosition(position);

            if (relativePosition + sizeof(long) <= PAGE_SIZE) {
                Buffer chunk = GetChunk(position) ?? throw new IndexOutOfRangeException();
                var span = chunk.Data.AsSpan(relativePosition);
                return BigEndian ? BinaryPrimitives.ReadInt64BigEndian(span) : BinaryPrimitives.ReadInt64LittleEndian(span);
            }

            Span<byte> temp = stackalloc byte[sizeof(long)];
            for (int i = 0; i < temp.Length; i++) {
                temp[i] = ReadByte(position + i);
            }

            return BigEndian ? BinaryPrimitives.ReadInt64BigEndian(temp) : BinaryPrimitives.ReadInt64LittleEndian(temp);
        }

        public void WriteFloat(float value) {
            WriteDouble(WritePosition, value);
            WritePosition += sizeof(float);
        }

        public void WriteFloat(int position, float value) {
            WriteInt(position, BitConverter.SingleToInt32Bits(value));
        }

        public float ReadFloat() {
            try {
                return ReadFloat(ReadPosition);
            } finally {
                ReadPosition += sizeof(float);
            }
        }

        public float ReadFloat(int position) {
            return BitConverter.Int32BitsToSingle(ReadInt(position));
        }

        public void WriteDouble(double value) {
            WriteDouble(WritePosition, value);
            WritePosition += sizeof(double);
        }

        public void WriteDouble(int position, double value) {
            WriteLong(position, BitConverter.DoubleToInt64Bits(value));
        }

        public double ReadDouble() {
            try {
                return ReadDouble(ReadPosition);
            } finally {
                ReadPosition += sizeof(double);
            }
        }

        public double ReadDouble(int position) {
            return BitConverter.Int64BitsToDouble(ReadLong(position));
        }
    }

}