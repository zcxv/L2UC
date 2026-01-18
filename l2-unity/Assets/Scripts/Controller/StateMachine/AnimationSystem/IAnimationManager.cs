using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using UnityEngine;

public interface IAnimationManager
{
    public void RegisterController(int objectId, IAnimationController controller, Entity entity);
    //void SetAnimationManager(PlayerAnimationController controller , PlayerEntity Player);
    void PlayAnimation(int objectId , string animationName , bool disableTriggerAfterStart);
    public void PlayAnimationTrigger(int objectId , string triggerName);

    public Task AsyncPlayAnimationCrossFade(int objectId , string animationName, float duration = 0.3f);
    void PlayOriginalAnimation(int objectId , string animationName);
    string GetCurrentAnimationName(int objectId);
    string GetLastAnimationName(int objectId);
    void StopCurrentAnimation(int objectId , string paramName , string runName = "");
    void PlayMonsterAnimation(int objectId, string animationName);
    void StopMonsterCurrentAnimation(int objectId, string animationName);
    Dictionary<string, float> PlayerGetAllFloat(int objectId);
    void PlayerSetAllFloat(int objectId , Dictionary<string, float> floatValues);
    public AnimationEventsBase  GetAnimationEvents(int objectId);

}
