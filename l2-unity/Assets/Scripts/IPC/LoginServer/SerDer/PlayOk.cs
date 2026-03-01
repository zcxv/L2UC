using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x07)]
    public class PlayOk : IpcDeserializer<PlayOkDto> {
        public PlayOkDto Deserialize(IpcLinkedBuffer buffer) => new (
            SID: buffer.ReadD(),
            UID: buffer.ReadD()
        );
    }

}