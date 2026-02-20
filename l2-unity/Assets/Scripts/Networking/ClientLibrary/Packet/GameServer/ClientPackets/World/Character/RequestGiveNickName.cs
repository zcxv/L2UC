using UnityEngine;

public class RequestGiveNickName : ClientPacket
{
    public RequestGiveNickName(string memberName , string title) : base((byte)GameClientPacketType.RequestGiveNickName)
    {
        WriteOtherS(memberName);
        WriteOtherS(title);
        BuildPacket();
    }
}
