using UnityEngine;

public class RequestAcquireSkill : ClientPacket
{
    public RequestAcquireSkill(int skillId, int skillLevel, int skillType) : base((byte)GameClientPacketType.RequestAcquireSkill)
    {

        WriteI(skillId);
        WriteI(skillLevel);
        WriteI(skillType);

        BuildPacket();
    }
}
