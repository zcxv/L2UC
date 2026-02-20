using UnityEngine;

public class RequestWithdrawPledge : ClientPacket
{
    public RequestWithdrawPledge() : base((byte)GameClientPacketType.RequestWithdrawalPledge)
    {

       // WriteI(0);
        BuildPacket();
    }
}
