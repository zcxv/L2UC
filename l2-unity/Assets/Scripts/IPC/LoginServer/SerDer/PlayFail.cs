using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x06)]
    public class PlayFail : IpcDeserializer<PlayFailDto> {
        public PlayFailDto Deserialize(IpcLinkedBuffer buffer) => new (
            (LoginFailureReason) buffer.ReadC()
        );
    }

}