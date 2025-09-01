using UnityEngine;

public class SkillInstance : AbstractSkill
{
    public SkillInstance(int pId, int pLevel, bool pPassive, bool pDisabled)
    {
        SkillID = pId;
        Level = pLevel;
        IsPassive = pPassive;
        IsDisabled = pDisabled;
    }
}
