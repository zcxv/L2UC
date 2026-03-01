using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x05)]
    public class RequestServerList : IpcSerializer<RequestServerListDto> {
        public void Serialize(IpcLinkedBuffer buffer, RequestServerListDto dto) {
            buffer.WriteD(dto.UID);
            buffer.WriteD(dto.SID);
        }
    }

}