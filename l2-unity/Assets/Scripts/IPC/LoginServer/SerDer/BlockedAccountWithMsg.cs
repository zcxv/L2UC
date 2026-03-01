using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x09)]
    public class BlockedAccountWithMsg : IpcDeserializer<BlockedAccountWithMsgDto> {
        public BlockedAccountWithMsgDto Deserialize(IpcLinkedBuffer buffer) {
            int count = buffer.ReadC();
            var messages = new BlockedAccountMessage[count];
            for (int i = 0; i < count; i++) {
                int messageId = buffer.ReadH();
                string token = buffer.ReadS();
                
                messages[i] = new BlockedAccountMessage(messageId, token);
            }
            
            return new BlockedAccountWithMsgDto(messages);
        }
    }

}