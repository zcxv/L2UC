using UnityEngine;

public class RequestOustPledgeMember : ClientPacket
{
    public RequestOustPledgeMember(string memberName) : base((byte)GameClientPacketType.RequestOustPledgeMember)
    {
        WriteOtherS(memberName);
        BuildPacket();
    }
}
