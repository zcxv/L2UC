using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x0b)]
    public class GGAuth : IpcDeserializer<GGAuthDto> {
        public GGAuthDto Deserialize(IpcLinkedBuffer buffer) => new (
            SessionId: buffer.ReadD(),
            GameGuardKey: new GameGuardKey(
                Data1: buffer.ReadD(),
                Data2: buffer.ReadD(),
                Data3: buffer.ReadD(),
                Data4: buffer.ReadD()
            )
        );
    }

}