using UnityEngine;

public class RequestEnchantItem : ClientPacket
{
    public RequestEnchantItem(int _objectId) : base((byte)GameClientPacketType.RequestEnchantItem)
    {
        WriteI(_objectId);
        BuildPacket();
    }
}
