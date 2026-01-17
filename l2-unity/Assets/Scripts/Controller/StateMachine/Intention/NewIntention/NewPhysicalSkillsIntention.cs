using UnityEngine;

public class NewPhysicalSkillsIntention : IntentionBase
{
    public NewPhysicalSkillsIntention(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter(object arg0)
    {
        if (arg0.GetType() == typeof(MagicSkillUse))
        {
            MagicSkillUse useSkill = (MagicSkillUse)arg0;
            AnimationCombo anim = SkillgrpTable.Instance.GetAnimComboBySkillId(useSkill.SkillId, useSkill.SkillLvl);
            SkillExecutor.Instance.ExecuteSkill(useSkill.SkillGrp , anim);
            
        }
    }

    public override void Exit() { }
    public override void Update()
    {

    }
}