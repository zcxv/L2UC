using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x00)]
    public class RequestLogin : IpcSerializer<RequestLoginDto> {
        public void Serialize(IpcLinkedBuffer buffer, RequestLoginDto dto) {
            buffer.WriteB(dto.EncryptedData);
            buffer.WriteD(dto.SessionId);
            buffer.WriteH(0x00);
            buffer.WriteC(0x00);
        }
    }

}