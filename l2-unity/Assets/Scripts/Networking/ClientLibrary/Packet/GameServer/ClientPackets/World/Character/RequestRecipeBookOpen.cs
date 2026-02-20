using UnityEngine;

public class RequestRecipeBookOpen : ClientPacket
{
    public RequestRecipeBookOpen(int isDwarven) : base((byte)GameClientPacketType.RequestRecipeBookOpen)
    {
        WriteI(isDwarven);
        BuildPacket();
    }
}
