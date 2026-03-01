using System;
using System.Collections.Generic;

namespace IPC {

    public class IpcDeserializerRegistry {
        private readonly Dictionary<int, object> registry = new();
        private readonly OpcodeType opcodeType;

        public IpcDeserializerRegistry(OpcodeType opcodeType) {
            this.opcodeType = opcodeType;
        }

        public void RegisterSubRegistry(int[] opcodes, OpcodeType opcodeType) {
            IpcDeserializerRegistry deserializerRegistry = this;
            for (int i = 0; i < opcodes.Length; i++) {
                int opcode = opcodes[i];

                if (i == opcodes.Length - 1) {
                    deserializerRegistry.registry[opcode] = new IpcDeserializerRegistry(opcodeType);
                } else {
                    if (deserializerRegistry.registry[opcode] is not IpcDeserializerRegistry subRegistry) {
                        throw new Exception($"Fail to register sub-registry for opcodes: {opcodes.ToStringView()}. " +
                                            $"Opcode 0x{opcode:X} is not registered.");
                    }

                    deserializerRegistry = subRegistry;
                }
            }
        }

        public void RegisterPacket(int[] opcodes, IpcDeserializer<PacketDto> deserializer) {
            IpcDeserializerRegistry deserializerRegistry = this;
            for (int i = 0; i < opcodes.Length; i++) {
                int opcode = opcodes[i];
                if (i == opcodes.Length - 1) {
                    deserializerRegistry.registry[opcode] = deserializer;
                } else {
                    if (deserializerRegistry.registry[opcode] is not IpcDeserializerRegistry subRegistry) {
                        throw new Exception($"Fail to register packet '{deserializer.GetType().GetFriendlyName()}' for opcodes: " +
                                            $"{opcodes.ToStringView()}. Opcode 0x{opcode:X} is not registered.");
                    }

                    deserializerRegistry = subRegistry;
                }
            }
        }

        public IpcDeserializer<PacketDto> Get(IpcLinkedBuffer buffer) {
            IpcDeserializerRegistry deserializerRegistry = this;
            do {
                int opcode = deserializerRegistry.GetOpcode(buffer);
                object obj = deserializerRegistry.registry[opcode];
                if (obj is IpcDeserializerRegistry subRegistry) {
                    deserializerRegistry = subRegistry;
                } else {
                    deserializerRegistry = null;
                }

                if (obj is IpcDeserializer<PacketDto> packet) {
                    return packet;
                }
            } while (deserializerRegistry != null);

            return null;
        }
        
        public IpcDeserializer<PacketDto> Get(int[] opcodes) {
            IpcDeserializerRegistry deserializerRegistry = this;
            for (int i = 0; i < opcodes.Length; i++) {
                int opcode = opcodes[i];

                object obj = deserializerRegistry.registry[opcode];
                if (i == opcodes.Length - 1) {
                    if (obj is not IpcDeserializer<PacketDto> deserializer) {
                        break;
                    }

                    return deserializer;
                }

                if (obj is not IpcDeserializerRegistry subRegistry) {
                    break;
                }

                deserializerRegistry = subRegistry;
            }

            return null;
        }
        
        private int GetOpcode(IpcLinkedBuffer buffer) {
            return opcodeType switch {
                OpcodeType.Int => buffer.ReadInt(),
                OpcodeType.UShort => buffer.ReadUShort(),
                OpcodeType.Short => buffer.ReadShort(),
                OpcodeType.UByte => buffer.ReadByte() & 0xff,
                OpcodeType.Byte => buffer.ReadByte(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

    }

}