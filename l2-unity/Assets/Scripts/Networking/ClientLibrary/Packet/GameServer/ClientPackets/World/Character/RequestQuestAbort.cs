using UnityEngine;

public class RequestQuestAbort : ClientPacket
{
    public RequestQuestAbort(int questId) : base((byte)GameClientPacketType.RequestQuestAbort)
    {
        WriteI(questId);
        BuildPacket();
    }
}
