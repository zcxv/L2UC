using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x07)]
    public class AuthGameGuard : IpcSerializer<AuthGameGuardDto> {
        public void Serialize(IpcLinkedBuffer buffer, AuthGameGuardDto dto) {
            buffer.WriteD(dto.SessionId);
            buffer.WriteD(dto.GameGuardKey.Data1);
            buffer.WriteD(dto.GameGuardKey.Data2);
            buffer.WriteD(dto.GameGuardKey.Data3);
            buffer.WriteD(dto.GameGuardKey.Data4);
        }
    }

}