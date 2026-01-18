using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    public static SkillExecutor Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async void ExecuteSkill(Entity entity , Skillgrp skill , AnimationCombo animationCombo)
    {
        foreach (string animName in animationCombo.GetAnimCycle())
        {
            //"SpAtk01" need "SpAtk01_"
            await AnimationManager.Instance.AsyncPlayAnimationCrossFade(entity.IdentityInterlude.Id , animName + "_");
            
        }
    }
}