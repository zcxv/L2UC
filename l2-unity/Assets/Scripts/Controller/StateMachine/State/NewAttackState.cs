using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class NewAttackState : StateBase
{
    private const int WOODEN_ARROW = 17;
    public NewAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        AnimationManager.Instance.OnAnimationFinished += CallBackAnimationFinish;
        AnimationManager.Instance.OnAnimationStartShoot += CallBackStartShoot;
        AnimationManager.Instance.OnAnimationStartShoot += CallBackLoadArrow;
    }


    public override void Update()
    {

    }

    public override void HandleEvent(Event evt)
    {
        switch (evt)
        {
            case Event.READY_TO_ACT:
                Debug.Log("Attack Sate to Intention> начало новой atk пришел запрос от сервера");

                PlayerEntity.Instance.RefreshRandomPAttack();
                Animation random = PlayerEntity.Instance.RandomName;
                AnimationManager.Instance.PlayAnimation(random.ToString(), true);

                break;
            case Event.CANCEL:
                Debug.Log("Attack Sate to Intention> Отмена скорее всего запрос пришел из ActionFaild");
                PlayerStateMachine.Instance.ChangeIntention(Intention.INTENTION_IDLE);
                PlayerStateMachine.Instance.NotifyEvent(Event.WAIT_RETURN);
                PlayerEntity.Instance.LastAtkAnimation = null;
                break;

        }
    }



    private void CallBackAnimationFinish(string animName)
    {
        Animation[] specials = SpecialAnimationNames.GetSpecialsAttackAnimations();

        foreach (Animation special in specials)
        {
            if (animName == special.ToString())
            {
                PlayerStateMachine.Instance.ChangeIntention(Intention.INTENTION_IDLE);
                PlayerStateMachine.Instance.NotifyEvent(Event.WAIT_RETURN);
                break;
            }
        }
    }

    private void CallBackStartShoot(string animName)
    {
        Animation[] specials = SpecialAnimationNames.GetSpecialsAttackAnimations();

        foreach (Animation special in specials)
        {
            if (animName == special.ToString())
            {
                

                // Общая проверка всех необходимых компонентов
                if (PlayerEntity.Instance == null ||
                    PlayerEntity.Instance.GetGoEtcItem() == null ||
                    PlayerEntity.Instance.Target == null)
                {
                    Debug.LogError("NewAttackState->CallBackStartShoot: Критическая ошибка не все компоненты загрузились что-бы отправить стрелу в полет");
                    return;
                }

                Vector3 startPos = PlayerEntity.Instance.GetPositionRightHand();
                GameObject go = PlayerEntity.Instance.GetGoEtcItem();
                Vector3 target = PlayerEntity.Instance.Target.position;
   
                ProjectileManager.Instance.LaunchProjectile(go,  startPos, target);
                break;
            }
        }
    }

    private void CallBackLoadArrow(string animName)
    {
        PlayerEntity.Instance.EquipArrow(WOODEN_ARROW);
    }


}