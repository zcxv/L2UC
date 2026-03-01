using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x00)]
    public class Init : IpcDeserializer<InitDto> {
        private const int RSA_MODULUS_SIZE = 128;
        private const int BLOWFISH_KEY_SIZE = 16;

        public InitDto Deserialize(IpcLinkedBuffer buffer) => new(
            SessionId: buffer.ReadD(),
            ProtocolVersion: buffer.ReadD(),
            ScrambledPublicKey: buffer.ReadBytes(new byte[RSA_MODULUS_SIZE]),
            GameGuardKey: new GameGuardKey(
                Data1: buffer.ReadD(),
                Data2: buffer.ReadD(),
                Data3: buffer.ReadD(),
                Data4: buffer.ReadD()
            ),
            BlowfishKey: buffer.ReadBytes(new byte[BLOWFISH_KEY_SIZE])
        );
    }

}