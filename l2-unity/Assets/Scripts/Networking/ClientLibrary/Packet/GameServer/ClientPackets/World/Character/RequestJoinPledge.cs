using UnityEngine;

public class RequestJoinPledge : ClientPacket
{
    public RequestJoinPledge(int _objectId) : base((byte)GameClientPacketType.RequestJoinPledge)
    {
        WriteI(_objectId);
        WriteI(0);
        BuildPacket();
    }
}
