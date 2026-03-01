using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    /// <remarks>LoginFail2</remarks>
    [Opcode(0x01)]
    public class LoginFail : IpcDeserializer<LoginFailDto> {
        public LoginFailDto Deserialize(IpcLinkedBuffer buffer) => new(
            (LoginFailureReason) buffer.ReadC()
        );
    }

}