using UnityEngine;
using static AttackingState;

public class AttackAction : L2Action
{
    public AttackAction() : base() { }

    // Local action
    public override void UseAction()
    {
        Debug.LogWarning("Use attack action.");

        if (TargetManager.Instance.HasTarget())
        {
            if (!PlayerEntity.Instance.IsAttack && PlayerStateMachine.Instance.State != PlayerState.DEAD)
             {
                 var target = PlayerEntity.Instance.GetTargetEntity();
                 if (target != null && !target.IsDead())
                {
                     Debug.LogWarning("Trying To Attack");
                     ClickManager.Instance.OnClickOnEntity();
                }
            }
        }
    }
}