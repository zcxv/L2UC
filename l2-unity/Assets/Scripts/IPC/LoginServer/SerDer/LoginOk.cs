using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x03)]
    public class LoginOk : IpcDeserializer<LoginOkDto> {
        public LoginOkDto Deserialize(IpcLinkedBuffer buffer) => new(
            UID: buffer.ReadD(),
            SID:  buffer.ReadD(),
            // d - unk
            // d - unk
            BillingMode: (BillingMode) (buffer.ReadInt(buffer.ReadPosition += 8) & 0xff00),
            PaidTime: buffer.ReadD(),
            // d - 0 const
            Warnings: buffer.ReadInt(buffer.ReadPosition += 4)
            // b[16] - forbidden servers
        );
    }

}