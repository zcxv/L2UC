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

    public bool IsMagic()
    {
        var skill = SkillgrpTable.Instance.GetSkill(SkillID, Level);
        if (skill == null)
            return false;
        return skill.IsMagic == 1;
    }
}
