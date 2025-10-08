using UnityEngine;

public class RequestWithdrawPledge : ClientPacket
{
    public RequestWithdrawPledge() : base((byte)GameInterludeClientPacketType.RequestWithdrawPledge)
    {

       // WriteI(0);
        BuildPacket();
    }
}
