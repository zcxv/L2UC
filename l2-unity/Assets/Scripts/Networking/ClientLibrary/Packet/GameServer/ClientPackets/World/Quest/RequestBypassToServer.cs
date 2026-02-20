using UnityEngine;

public class RequestBypassToServer : ClientPacket
{
    public RequestBypassToServer(string bypasscommand) : base((byte)GameClientPacketType.RequestBypassToServer)
    {
        WriteSOther(bypasscommand);
        BuildPacketManualType((byte)GameClientPacketType.RequestBypassToServer);
    }
}
