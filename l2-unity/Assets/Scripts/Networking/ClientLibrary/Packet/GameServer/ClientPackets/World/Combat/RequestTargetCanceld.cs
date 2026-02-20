using UnityEngine;

public class RequestTargetCanceld : ClientPacket
{
    public RequestTargetCanceld() : base((byte)GameClientPacketType.RequestTargetCanceld)
    {
        WriteShort(0);
        BuildPacket();
    }
}
