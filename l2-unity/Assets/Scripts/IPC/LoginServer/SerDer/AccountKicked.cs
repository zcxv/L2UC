using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x08)]
    public class AccountKicked : IpcDeserializer<AccountKickedDto> {
        public AccountKickedDto Deserialize(IpcLinkedBuffer buffer) => new (
            (LoginFailureReason) buffer.ReadC()
        );
    }

}