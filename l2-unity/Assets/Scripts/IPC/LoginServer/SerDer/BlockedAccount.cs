using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x02)]
    public class BlockedAccount : IpcDeserializer<BlockedAccountDto> {
        public BlockedAccountDto Deserialize(IpcLinkedBuffer buffer) => new (
            (BlockedAccountReason) buffer.ReadD()
            // d
        );
    }

}