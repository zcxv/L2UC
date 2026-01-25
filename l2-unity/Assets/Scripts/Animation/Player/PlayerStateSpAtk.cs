using UnityEngine;

public class PlayerStateSpAtk : StateMachineBehaviour
{
    private float _startTime = -1;
    private float _endTime = -1;

    private const string SP_TIME_ATK = "sptimeatk";
    public string parameterName;
    private AnimationCurve _animationCurve;
    private bool _isSwitchIdle = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        AnimatorClipInfo[] clipInfos = animator.GetNextAnimatorClipInfo(layerIndex);

        if (clipInfos == null || clipInfos.Length == 0)
        {
            clipInfos = animator.GetCurrentAnimatorClipInfo(layerIndex);
        }

        _animationCurve = new AnimationCurve();
        _isSwitchIdle = false;


        int timeAtk = GetTimeAtk(animator , parameterName);



        _startTime = Time.time;
        _endTime = TimeUtils.ConvertMsToSec(timeAtk);

        float timeAnimation = clipInfos[0].clip.length;
        //default
        //RecreateAnimationCurve(_animationCurve, _endTime, timeAnimation , 1.0f, 1.3f);
        RecreateAnimationCurve(_animationCurve, _endTime, timeAnimation, 1.0f, 1.1f);

        PlayerAnimationController.Instance.UpdateAnimatorAtkSpeedL2j(timeAtk, timeAnimation);

        StopAnimationTrigger(animator, parameterName);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentTime = Time.time;
        float timeOut = currentTime - _startTime;
        float normalizedTime = timeOut / _endTime;
        float speed = _animationCurve.Evaluate(normalizedTime);

        if (timeOut >= _endTime) SwitchToIdle(stateInfo);

        PlayerAnimationController.Instance.SetPAtkSpeed(speed);
    }

    private void SwitchToIdle(AnimatorStateInfo stateInfo)
    {

        float currentNormalizedTime = stateInfo.normalizedTime;

        if (!_isSwitchIdle & currentNormalizedTime > 0.9f & IsDieTarget() | !_isSwitchIdle & currentNormalizedTime > 0.9f & !PlayerEntity.Instance.IsAttack)
        {
            PlayerEntity.Instance.IsAttack = false;
            _isSwitchIdle = true;
            PlayerStateMachine.Instance.ChangeIntention(Intention.INTENTION_IDLE);
            PlayerStateMachine.Instance.NotifyEvent(Event.WAIT_RETURN);
        }
    }

    private bool IsDieTarget()
    {
        Entity entity = World.Instance.GetEntityNoLockSync(PlayerEntity.Instance.TargetId);
        if (entity != null) return entity.IsDead();
        return false;
    }
    private void StopAnimationTrigger(Animator animator, string parameterName)
    {
        if (animator.GetBool(parameterName) != false)
        {
            AnimationManager.Instance.StopCurrentAnimation(animator.GetInteger(AnimatorUtils.OBJECT_ID), parameterName, "player");
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Attack Sate to Intention> конец атаки OnStateExit");
        PlayerEntity.Instance.IsAttack = false;
    }


    private void RecreateAnimationCurveDefault(AnimationCurve animationCurve, float timeAtk, float timeAnimation)
    {
        Keyframe startKey = new Keyframe(0f, 0f);
        Keyframe endKey = new Keyframe(timeAtk, timeAnimation);
        //float speedAtk = (float)0.362 / timeAtk;
        //float speedAtk = (float)0.3585 / timeAtk;
        float speedAtk = (float)0.1585 / timeAtk; //default
                                                  //default to sword slow down
                                                  //test 0,603 для стандартной атакие или 0.36293652 0.1 на еденицу времени
                                                  //Keyframe slowDownAttackKey = new Keyframe(0.07511136f, speedAtk); //default
        Keyframe slowDownAttackKey = new Keyframe(0.09011136f, speedAtk);
        // Keyframe slowDownAttackKey = new Keyframe(0.07511136f, 0.4373413f);
        //Keyframe slowDownAttackKey = new Keyframe(0.07511136f, 0.9073413f);

        animationCurve.AddKey(startKey);
        animationCurve.AddKey(slowDownAttackKey);
        animationCurve.AddKey(endKey);
    }

    // Normal speed
    //RecreateAnimationCurve(myCurve, baseTimeAtk, baseTimeAnimation);

    // 2-5% faster attack (speedMultiplier = 0.95-0.98)
    //RecreateAnimationCurve(myCurve, baseTimeAtk, baseTimeAnimation, 0.96f, 1.0f);

    // Slower attack (if needed)
    //RecreateAnimationCurve(myCurve, baseTimeAtk, baseTimeAnimation, 1.0f, 1.2f);

    // Faster and slower combination
    // RecreateAnimationCurve(myCurve, baseTimeAtk, baseTimeAnimation, 0.97f, 1.1f);
    private void RecreateAnimationCurve(AnimationCurve animationCurve, float timeAtk, float timeAnimation,
 float speedMultiplier = 1.0f, float slowDownFactor = 1.0f)
    {
        float adjustedTimeAtk = timeAtk * speedMultiplier;
        float adjustedTimeAnimation = timeAnimation * slowDownFactor;

        animationCurve.keys = new Keyframe[0];

        // 1. Старт
        Keyframe startKey = new Keyframe(0f, 0f);

        // 2. БЫСТРЫЙ ПОДЪЕМ: за 20% времени проходим 45% анимации
        // Это убирает медлительность в начале (те самые 500мс станут бодрее)
        Keyframe fastWindup = new Keyframe(adjustedTimeAtk * 0.2f, adjustedTimeAnimation * 0.45f);

        // 3. ПЕРЕХОД: точка, где замах закончен и начинается падение меча
        // Смещаем на 45% времени, чтобы удар не был слишком коротким
        Keyframe midPoint = new Keyframe(adjustedTimeAtk * 0.45f, adjustedTimeAnimation * 0.65f);

        // 4. УДАР: распределяем оставшееся движение более плавно
        // Вместо резкого скачка делаем промежуточную точку на 75% времени
        Keyframe strikeKey = new Keyframe(adjustedTimeAtk * 0.75f, adjustedTimeAnimation * 0.85f);

        // 5. ЗАВЕРШЕНИЕ (End)
        Keyframe endKey = new Keyframe(adjustedTimeAtk, adjustedTimeAnimation);

        animationCurve.AddKey(startKey);
        animationCurve.AddKey(fastWindup);
        animationCurve.AddKey(midPoint);
        animationCurve.AddKey(strikeKey);
        animationCurve.AddKey(endKey);

        animationCurve.preWrapMode = WrapMode.ClampForever;
        animationCurve.postWrapMode = WrapMode.ClampForever;

        // Сглаживание
        for (int i = 0; i < animationCurve.keys.Length; i++)
        {
            // Используем SmoothTangents, чтобы переходы были не ломаными, а дугообразными
            animationCurve.SmoothTangents(i, 0);
        }
    }



    private bool IsBow(string animName) => animName.IndexOf("bow") != -1;

    private int GetTimeAtk(Animator animator , string animName)
    {
        if (IsBow(animName))
        {
            //return CalcBaseParam.CalculateTimeL2j(PlayerEntity.Instance.Stats.BasePAtkSpeed);
            float baseAttackTime = animator.GetInteger(SP_TIME_ATK);
            float targetDistance = PlayerEntity.Instance.TargetDistance();
            float[] timeAndFlye = CalcBaseParam.CalculateAttackAndFlightTimes(targetDistance, baseAttackTime);


            return (int)timeAndFlye[0];
        }
        return animator.GetInteger(SP_TIME_ATK);
    }


}
