using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SkillAnimationRunner
{
   public async Task StartRun(string[] cycle , int objectId , IAnimationManager animationManager , Action onComplete = null)
    {
        foreach (string animName in cycle)
        {
            if(animName != "none")
            {
                //"SpAtk01" need "SpAtk01_"
                Debug.Log("SkillAnimationRunner>StartRun: animName " + animName);
                await animationManager.AsyncPlayAnimationCrossFade(objectId, animName + "_");
            }

        }

        onComplete?.Invoke();
    }
}
