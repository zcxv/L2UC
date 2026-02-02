using UnityEngine;

public class NewMagicSkillsState  : AbstractAttackEvents
{
    public NewMagicSkillsState(PlayerStateMachine stateMachine) :
         base(stateMachine.GetObjectId(),
         SpecialAnimationNames.GetMagicSkillsAnimations(),
         stateMachine)
    { }

    public override void Enter()
    {

    }
    public override void HandleEvent(Event evt, object payload = null)
    {
        MagicSkillUse useSkill = GetPayload(payload);

        switch (evt)
        {
            case Event.READY_TO_ACT:

                AnimationCombo readyCombo = SkillgrpTable.Instance.GetAnimComboBySkillId(useSkill.SkillId, useSkill.SkillLvl);
                SkillExecutor.Instance.ExecuteSkillOverride(_stateMachine.Player, readyCombo, _events);
                break;
            case Event.CANCEL:
                Debug.Log("NewMagicSkillsState Use Sate> Отмена скорее всего запрос пришел из ActionFaild");
                break;
            case Event.APPLY_SELF_SKILL:
                AnimationCombo selfCombo = SkillgrpTable.Instance.GetAnimComboBySkillId(useSkill.SkillId, useSkill.SkillLvl);
                SkillExecutor.Instance.ExecuteSkillOverride(_stateMachine.Player, selfCombo, _events);
                break;

        }
    }



   
}
