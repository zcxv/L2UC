using Org.BouncyCastle.Bcpg;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class NewAttackState : StateBase
{
    private const int WOODEN_ARROW = 17;
    public NewAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        AnimationManager.Instance.OnAnimationFinished += CallBackAnimationFinish;
        AnimationManager.Instance.OnAnimationStartShoot += CallBackStartShoot;
        AnimationManager.Instance.OnAnimationLoadArrow += CallBackLoadArrow;
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

    private void CallBackStartShoot(string animName , float remainingAtkTime)
    {
        Animation[] specials = SpecialAnimationNames.GetSpecialsAttackAnimations();

        foreach (Animation special in specials)
        {
            if (animName == special.ToString())
            {


                GameObject go = PlayerEntity.Instance.GetGoEtcItem();
                Transform target = PlayerEntity.Instance.Target;

                if (PlayerEntity.Instance == null ||
                    go == null ||
                    target == null)
                {
                    Debug.LogError("NewAttackState->CallBackStartShoot: Критическая ошибка не все компоненты загрузились что-бы отправить стрелу в полет");
                    return;
                }

                //I'm currently considering it as an attack for bows. Basically, when we attack with a bow,
                //we take the total attack time. For example, the entire attack time is 1552ms. Then, using /2,
                //we get 770 for the bow attack and 770 for the arrow flight. Therefore, in the bow attack code,
                //we take the time of 770 and use the 2nd part here.
                //float timeAtk = CalcBaseParam.CalculateTimeL2j(PlayerEntity.Instance.Stats.BasePAtkSpeed) / 2;
                //timeAtk = TimeUtils.ConvertMsToSec(timeAtk); // Convert to seconds if needed
                Vector3 startPos = PlayerEntity.Instance.GetPositionRightHand();

                float baseAttackTime = CalcBaseParam.CalculateTimeL2j(PlayerEntity.Instance.Stats.BasePAtkSpeed);
                float targetDistance = PlayerEntity.Instance.TargetDistance();
                float[] timeAndFlye = CalcBaseParam.CalculateAttackAndFlightTimes(targetDistance, baseAttackTime);
                var timeAtk = TimeUtils.ConvertMsToSec(timeAndFlye[1]);

                ProjectileData settings = new ProjectileData(go, target, startPos, target);
                settings.lifetime = timeAtk;

                ProjectileManager.Instance.LaunchProjectile(go,  startPos, target , settings);
                break;
            }
        }
    }

    private void CallBackLoadArrow(string animName)
    {
        PlayerEntity.Instance.EquipArrow(WOODEN_ARROW);
    }


}