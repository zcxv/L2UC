using System.Collections.Generic;
using UnityEngine;

public class RequestPledgeInfo : ClientPacket
{
    
    public RequestPledgeInfo(int clanId) : base((byte)GameClientPacketType.RequestPledgeInfo)
    {
        WriteI(clanId);
        BuildPacket();
    }
}
