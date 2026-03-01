using IPC.LoginServer.DTO;

namespace IPC.LoginServer.SerDer {

    [Opcode(0x04)]
    public class ServerList : IpcDeserializer<ServerListDto> {
        public ServerListDto Deserialize(IpcLinkedBuffer buffer) {
            int count = buffer.ReadC();
            int lastServerId = buffer.ReadC();
            
            var serverList = new ServerListEntry[count];
            for (int i = 0; i < count; i++) {
                serverList[i] = new ServerListEntry(
                    Id: buffer.ReadC(),
                    Host: ToHost(buffer.ReadBytes(new byte[4])),
                    Port: buffer.ReadD(),
                    AgeLimit: (ServerAgeLimit) buffer.ReadC(),
                    IsPvpAllowed: buffer.ReadC() != 0x00,
                    OnlinePlayers: buffer.ReadH(),
                    MaxPlayers: buffer.ReadH(),
                    IsOnline: buffer.ReadC() != 0x00,
                    ServerConfiguration: (ServerConfiguration) buffer.ReadD(),
                    Region: buffer.ReadC()
                );
            }

            return new ServerListDto(lastServerId, serverList);
        }

        private string ToHost(byte[] bytes) => $"{bytes[0]}.{bytes[1]}.{bytes[2]}.{bytes[3]}";
    }

}