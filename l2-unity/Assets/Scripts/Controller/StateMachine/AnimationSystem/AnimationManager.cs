using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class AnimationManager : BaseAnimationManager , IAnimationManager
{
   
    private static AnimationManager _instance;
    private List<string> listTriggerAfterStart = new List<string>(10);
    private float _remainingAtkTime = 0;

    private TaskCompletionSource<bool> _animationFinishedTcs;



    public static IAnimationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AnimationManager();
            }
            return _instance;
        }
    }

    public void PlayAnimation(int objectId , string animationName, bool disableTriggerAfterStart)
    {
       
        string finalAnimName = GetFinalNameAnim(objectId , animationName);
        IAnimationController controller = GetPlayerController(objectId);
        Entity entity = GetEntity(objectId);
        DesableLastPlayerAnimationElseTrue(objectId, controller);


        SetRecentName(objectId , finalAnimName);
   
        controller.SetBool(finalAnimName, true, entity.name);
        Debug.Log($"AnimationManager> start bool name player  {entity.name} animation {finalAnimName}");
    }

    public void PlayAnimationTrigger(int objectId, string animationName)
    {
        string triggerName = GetFinalNameAnim(objectId , animationName);

        IAnimationController controller = GetPlayerController(objectId);
        Entity entity = GetEntity(objectId);
        DesableLastPlayerAnimationElseTrue(objectId, controller);
        controller.ToggleAnimationTrigger(triggerName);

        Debug.Log($"AnimationManager> start trigger name player  {entity.name} animation {triggerName}");
    }


    //Async Wait End Event
    public async Task AsyncPlayAnimationCrossFade(int objectId, string animationName, float duration = 0.3f)
    {
        _animationFinishedTcs = new TaskCompletionSource<bool>();
        PlayerAnimationCrossFade(objectId,  animationName , duration);
        await _animationFinishedTcs.Task;
    }

    public void PlayerAnimationCrossFade(int objectId , string animationName, float duration)
    {
        string crossFadeName = GetFinalNameAnim(objectId , animationName);
        IAnimationController controller = GetPlayerController(objectId);
        Entity entity = GetEntity(objectId);
        DesableLastPlayerAnimationElseTrue(objectId , controller);
        controller.ToggleAnimationCrossFade(crossFadeName , duration);
        //PlayerAnimationController.Instance.StartCrossFade(crossFadeName, duration);

        Debug.Log($"AnimationManager> start crossFade  {entity} animation  {crossFadeName}");
    }


    public void PlayMonsterAnimation(int objectId, string animationName)
    {
        IAnimationController controllerAnimator = GetMonsterController(objectId);
        DisableLastMonsterAnimationElseTrue(objectId, controllerAnimator, animationName);
        SetMonsterRecentName(objectId, animationName);
        controllerAnimator.SetBool(animationName, true);
    }
   
    public Dictionary<string, float> PlayerGetAllFloat(int objectId)
    {
        PlayerAnimationController controller = GetPlayerController(objectId);
        return controller.GetParametrs();
    }






    public void StopMonsterCurrentAnimation(int objectId, string animationName)
    {
        if (GetMonsterController(objectId) is { } controller)
        {
            controller.SetBool(animationName, false);
        }
        else
        {
            Debug.LogWarning($"AnimationManager->StopMonsterCurrentAnimation: Не критическая ошибка - animator not found for monster {objectId}. Animation: {animationName}");
        }
    }


}
