using UnityEngine;

public class NewPhysicalSkillsState : StateBase
{
    public NewPhysicalSkillsState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
   
    }
    public override void HandleEvent(Event evt)
    {
        switch (evt)
        {
            case Event.DEAD:
                AnimationManager.Instance.PlayOriginalAnimation(AnimationNames.DEAD.ToString());
                break;

        }
    }
}