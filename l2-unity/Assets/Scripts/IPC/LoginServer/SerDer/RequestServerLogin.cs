using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x02)]
    public class RequestServerLogin : IpcSerializer<RequestServerLoginDto> {
        public void Serialize(IpcLinkedBuffer buffer, RequestServerLoginDto dto) {
            buffer.WriteD(dto.UID);
            buffer.WriteD(dto.SID);
            buffer.WriteC(dto.ServerId);
        }
    }

}