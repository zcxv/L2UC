using UnityEngine;

public class NewAttackState : StateBase
{

    public NewAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }


    public override void Update()
    {

    }

    public override void HandleEvent(Event evt)
    {
        switch (evt)
        {
            case Event.READY_TO_ACT:
                Debug.Log("Attack Sate to Intention> начало новой атакие пришел запрос от сервера");

                PlayerEntity.Instance.RefreshRandomPAttack();
                Animation random = PlayerEntity.Instance.RandomName;

                //if (PlayerEntity.Instance.LastAtkAnimation == null)
                //{
                //    PlayerEntity.Instance.LastAtkAnimation = PlayerEntity.Instance.RandomName;
                    AnimationManager.Instance.PlayAnimation(random.ToString(), true);
               // }
               // else
               // {
                 //   if (!PlayerEntity.Instance.LastAtkAnimation.AreAnimationsEqual(PlayerEntity.Instance.LastAtkAnimation, random))
                  //  {
                  //      AnimationManager.Instance.PlayAnimation(random.ToString(), true);
                   //     Debug.Log($"AnimationManager> start name player  test2 animation pre start random {random} and {PlayerEntity.Instance.LastAtkAnimation}");
                   //     PlayerEntity.Instance.LastAtkAnimation = random;
                   // }
                //}

                    break;
            case Event.CANCEL:
                Debug.Log("Attack Sate to Intention> Отмена скорее всего запрос пришел из ActionFaild");
                PlayerStateMachine.Instance.ChangeIntention(Intention.INTENTION_IDLE);
                PlayerStateMachine.Instance.NotifyEvent(Event.WAIT_RETURN);
                PlayerEntity.Instance.LastAtkAnimation = null;
                break;

        }
    }
}