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
            AnimationCombo animCombo = SkillgrpTable.Instance.GetAnimComboBySkillId(useSkill.SkillId, useSkill.SkillLvl);
            SkillExecutor.Instance.ExecuteSkill(_stateMachine.Player , useSkill.SkillGrp , animCombo);
        }
    }

    public override void Exit() { }
    public override void Update()
    {

    }
}