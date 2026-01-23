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


            _stateMachine.ChangeState(PlayerState.PHYSICAL_SKILLS);
            _stateMachine.NotifyEvent(Event.READY_TO_ACT , useSkill);
        }
    }

    public override void Exit() { }
    public override void Update()
    {

    }
}