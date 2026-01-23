using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SkillFXEmitter
{
   private AnimationEventsBase _currentActions;


    public void OnHit(string animationName)
    {
        Debug.Log("Specials Hit events");
    }
    public void OnFinishAnim(string animationName)
    {
        Debug.Log("Specials Hit OnFinishAnim " + animationName);
    }

    public void SetupActions(AnimationEventsBase actions)
    {
        if (_currentActions == null) return;

        _currentActions = actions;
        actions.OnAnimationStartHit += OnHit;
        actions.OnAnimationFinished += OnFinishAnim;
    }

    public void CleanupActions(AnimationEventsBase actions)
    {
        if (_currentActions == null) return;

        _currentActions.OnAnimationStartHit -= OnHit;
        _currentActions.OnAnimationStartShoot -= OnFinishAnim;

        _currentActions = null;
    }

}
