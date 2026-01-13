
using System.Threading;
using UnityEngine;



public class NewAttackState : StateBase
{
    private const int WOODEN_ARROW = 17;
  
    public NewAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        AnimationManager.Instance.OnAnimationFinished += CallBackAnimationFinish;
        AnimationManager.Instance.OnAnimationStartShoot += CallBackStartShoot;
        AnimationManager.Instance.OnAnimationFinishedHit += CallBackFinishedHit;
        AnimationManager.Instance.OnAnimationLoadArrow += CallBackLoadArrow;
        AnimationManager.Instance.OnAnimationStartHit += CallBackStartHit;

        ProjectileManager.Instance.OnHitMonster += OnHitBodyMonster;
        ProjectileManager.Instance.OnHitCollider += OnHitColliderMonster;
        SwordCollisionService.Instance.OnHitCollider += OnHitColliderMonster;

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
                RotateFaceToMonster(_stateMachine.Player);
                PlayerEntity.Instance.RefreshRandomPAttack();
                Animation random = PlayerEntity.Instance.RandomName;
                AnimationManager.Instance.PlayAnimationTrigger(random.ToString());

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
        Animation[] specialsBows = SpecialAnimationNames.GetSpecialsAttackAnimations();

        foreach (Animation special in specialsBows)
        {

            if (animName == special.ToString())
            {
               PlayerStateMachine.Instance.ChangeIntention(Intention.INTENTION_IDLE);
               PlayerStateMachine.Instance.NotifyEvent(Event.WAIT_RETURN);
               break;
            }
        }
    }

    private void CallBackFinishedHit(string animName, float remainingAtkTime)
    {
        Animation[] specialsBows = SpecialAnimationNames.GetSpecialsAttackAnimations();

        foreach (Animation special in specialsBows)
        {
            if (animName == special.ToString())
            {

                if (special.Type == TypesAnimation.MeleeAttack)
                {
   

                    Transform[] swordBasePoints = _stateMachine.Player.GetSwordBasePoints();
                    if (swordBasePoints.Length > 1) SwordCollisionService.Instance.UnregisterSword(swordBasePoints[0]);
                    PlayerEntity.Instance.RemoveProceduralPose();
                    break;
                }
            }
        }
    }

    private void IfMonsterDead(Entity target)
    {
        if (target == null) return;


        if (target != null & target is MonsterEntity)
        {
            MonsterEntity monsterEntity = (MonsterEntity)target;
            Debug.Log("Попали и увидели что монстр уже должен быть мертвым hp  " + monsterEntity.Hp() + " RemainingHP " + monsterEntity.CalculateRemainingHp());

            if (monsterEntity.IsDead() || monsterEntity.CalculateRemainingHp() <= 0)
            {
                monsterEntity.SetDead(true);
                MonsterStateMachine stateMachine = monsterEntity.GetStateMachine();
                stateMachine.ChangeState(MonsterState.DEAD);
                stateMachine.NotifyEvent(Event.FORCE_DEATH);
                Debug.Log("Попали и увидели что монстр уже должен быть мертвым hp запускаем анимацию смерти " + monsterEntity.IsDead());
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

                if(special.Type == TypesAnimation.BowAttack)
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

                    Vector3 startPos = PlayerEntity.Instance.GetPositionRightHand();

                    float baseAttackTime = CalcBaseParam.CalculateTimeL2j(PlayerEntity.Instance.Stats.BasePAtkSpeed);
                    float targetDistance = PlayerEntity.Instance.TargetDistance();
                    float[] timeAndFlye = CalcBaseParam.CalculateAttackAndFlightTimes(targetDistance, baseAttackTime);
                    var timeAtk = TimeUtils.ConvertMsToSec(timeAndFlye[1]);

                    ProjectileData settings = new ProjectileData(go, target, startPos, target);
                    settings.lifetime = timeAtk;

                    ProjectileManager.Instance.LaunchProjectile(go, startPos, target, settings);
                    break;
                }

            }
        }
    }

    private void CallBackLoadArrow(string animName)
    {
        PlayerEntity.Instance.EquipArrow(WOODEN_ARROW);
    }

    private void CallBackStartHit(string animName, float remainingAtkTime)
    {
        Animation[] specials = SpecialAnimationNames.GetSpecialsAttackAnimations();
        foreach (Animation special in specials)
        {
            if(special.Type == TypesAnimation.MeleeAttack)
            {
                RegisterSwordCollision(_stateMachine.Player);
            }
        }

    }

    private void OnHitBodyMonster(GameObject prefab, Transform target, Vector3 hitPointCollider, Vector3 hitDirection)
    {
        HitManager.Instance.HandleHitBody(prefab, target, hitPointCollider, hitDirection);
    }

    private void OnHitColliderMonster(Transform attacker , Transform target, Vector3 hitPointCollider, Vector3 hitDirection)
    {
        Entity entity = PlayerEntity.Instance.GetTargetEntity();

        if(entity is MonsterEntity)
        {
            MonsterEntity monster = (MonsterEntity)entity;
            HitManager.Instance.HandleHitCollider(attacker, monster.GetStateMachine(), hitPointCollider, hitDirection);
            IfMonsterDead(PlayerEntity.Instance.GetTargetEntity());
        }

    }



    private void RegisterSwordCollision(PlayerEntity entity)
    {
        if (entity == null) return;

        Transform[] swordBasePoints = entity.GetSwordBasePoints();

        if (swordBasePoints != null && swordBasePoints.Length > 1)
        {
            Transform swordBase = swordBasePoints[0];
            Transform swordTip = swordBasePoints[1];
            Transform target =  PlayerEntity.Instance.Target;
            SwordCollisionService.Instance.RegisterSword(swordBase, swordTip, target ,  0);
        }
    }

    private void RotateFaceToMonster(Entity entity)
    {
        Transform monster = PlayerEntity.Instance.Target;
        if (monster == null) return;

  
        RotationService.Instance.RotateTowards(entity.transform, monster.position,  () =>
        {
            float monsterHeight = monster.GetComponent<Entity>().Appearance.CollisionHeight;
            Vector3 monsterFacePosition = monster.position + Vector3.up * (monsterHeight * 0.8f);

            Vector3 startPoint = entity.transform.position + Vector3.up * 1.5f;
            Vector3 lookDir = (monsterFacePosition - startPoint).normalized;
            float verticalAngle = Mathf.Asin(lookDir.y) * Mathf.Rad2Deg;

            // --- НАСТРОЙКА СПИНЫ ---
            // Берем 40% от общего угла для естественности
            float spineAngle = Mathf.Clamp(verticalAngle * 0.4f, -15f, 10f);
            Vector3 spineRotation = new Vector3(0, 0, spineAngle);

            // --- НАСТРОЙКА РУКИ ---
            // Добавляем еще 30% наклона именно для руки, чтобы она била ниже
            float armAngle = Mathf.Clamp(verticalAngle * 0.3f, -20f, 10f);
            Vector3 armRotation = new Vector3(0, 0, armAngle);
            // ВНИМАНИЕ: Если рука крутится не туда, проверьте ось (возможно, нужна X вместо Z)

            // 4. Применяем через ваш PlayerEntity и SpineProceduralController
            PlayerEntity playerEntity = (PlayerEntity)entity;

            // Применяем к позвоночнику (вы уже настроили это в SetProceduralPose)
            playerEntity.SetProceduralSpinePose(spineRotation);
            playerEntity.SetProceduralRightUpperArmPose(armRotation);
        });

        //DebugLineDraw.ShowDrawLineDebugNpc(-1, startPoint, lookDir * 3f, Color.black);
    }







}