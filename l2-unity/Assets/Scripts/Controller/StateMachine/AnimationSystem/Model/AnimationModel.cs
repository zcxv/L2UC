using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AnimationModel
{
    private IAnimationController _controller;
    private Entity _entity;
    private Type _typeController;
    private AnimationEventsBase _eventSource;

    //this is outside of AnimatorManager
   // public event Action<string> OnAnimationFinished;
   // public event Action<string, float> OnAnimationStartShoot;
   // public event Action<string, float> OnAnimationFinishedHit;
   // public event Action<string, float> OnAnimationStartHit;
   // public event Action<string> OnAnimationLoadArrow;

    public AnimationModel(IAnimationController controller , Entity entity)
    {
        _controller = controller;
        _entity = entity;
        _typeController = controller.GetType();
        _eventSource = controller as AnimationEventsBase;
        //InitializeAnimationEventSystem();
    }

   // private void InitializeAnimationEventSystem()
   // {
      //  if (_eventSource != null)
      //  {
            //this is inside the animator
           // _eventSource.OnAnimationFinished += AnimationFinishedPlayerCallback;
           // _eventSource.OnAnimationStartShoot += AnimationShootPlayerCallback;
          //  _eventSource.OnAnimationStartHit += AnimationHitPlayerCallback;
           // _eventSource.OnAnimationFinishedHit += AnimationFinishedHitPlayerCallback;
           // _eventSource.OnAnimationStartLoadArrow += AnimationLoadArrowPlayerCallback;
       // }

    //}

    public IAnimationController GetController() => _controller;
    public Entity GetEntity() => _entity;
    public Type GetType() => _typeController;

    // public void AnimationFinishedPlayerCallback(string animationName)
    // {
    //  //_animationFinishedTcs?.TrySetResult(true);
    //   OnAnimationFinished?.Invoke(animationName);
    // }

    // public void AnimationShootPlayerCallback(string animationName)
    // {
    //  OnAnimationStartShoot?.Invoke(animationName, 0);
    //}

    // public void AnimationHitPlayerCallback(string animationName)
    // {
    //     OnAnimationStartHit?.Invoke(animationName, 0);
    // }

    //public void AnimationFinishedHitPlayerCallback(string animationName)
    //{
    //    OnAnimationFinishedHit?.Invoke(animationName, 0);
    //}

    //public void AnimationLoadArrowPlayerCallback(string animationName)
    //{
    //     OnAnimationLoadArrow?.Invoke(animationName);
    // }

    public AnimationEventsBase GetEvents() => _eventSource;
}
