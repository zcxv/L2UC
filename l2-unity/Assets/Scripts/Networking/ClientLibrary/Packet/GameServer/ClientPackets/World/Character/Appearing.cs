using UnityEngine;

public class Appearing : ClientPacket
{
    public Appearing() : base((byte)GameClientPacketType.Appearing)
    {
        BuildPacket();
    }
}
