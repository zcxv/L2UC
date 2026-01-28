using System.Threading;
using UnityEngine;


public class NewPhysicalSkillsState : AbstractAttackEvents
{
    public NewPhysicalSkillsState(PlayerStateMachine stateMachine) : 
        base(stateMachine.GetObjectId() ,
        SpecialAnimationNames.GetPhisicalSkillsAnimations(),
        stateMachine) { }

    public override void Enter()
    {
   
    }
    public override void HandleEvent(Event evt , object payload = null)
    {
        switch (evt)
        {
            case Event.READY_TO_ACT:
                Debug.Log("NewPhysicalSkillsState Sate> начало новой atk пришел запрос от сервера");

                if(payload is MagicSkillUse useSkill)
                {
                   
                    Debug.Log("NewPhysicalSkillsState Use Sate> обнаружили что идет запуск физического скила");
                    AnimationCombo animCombo = SkillgrpTable.Instance.GetAnimComboBySkillId(useSkill.SkillId, useSkill.SkillLvl);
                    Debug.Log($"[SyncCheck] useSkill время от сервера " + useSkill.HitTime);
                   //not use bow atk
                   //RotateFaceToMonster(_stateMachine.Player);
                   SkillExecutor.Instance.ExecuteSkill(_stateMachine.Player, useSkill.SkillGrp, animCombo , _events);
                    Debug.Log("NewPhysicalSkillsState Use Sate> обнаружили завершили физического скила");
                }
                break;
            case Event.CANCEL:
                Debug.Log("NewPhysicalSkillsState Use Sate> Отмена скорее всего запрос пришел из ActionFaild");
    
                break;

        }
    }

    private void RotateFaceToMonster(Entity entity)
    {
        Transform monster = PlayerEntity.Instance.Target;
        if (monster == null) return;


        RotationService.Instance.RotateTowards(entity.transform, monster.position, () =>
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