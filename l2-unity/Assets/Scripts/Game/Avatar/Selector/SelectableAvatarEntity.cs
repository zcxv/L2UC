using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableAvatarEntity : MonoBehaviour {
    
    [NonSerialized] public string WeaponAnim;
    
    [NonSerialized] public CharSelectInfoPackage CharacterInfo;
    
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private CharacterController characterController;
    private BaseAnimationController animationController;

    private bool IsWalkAnimationActive => animationController.GetBool("walk_" + WeaponAnim);
    
    private float WalkAnimationSpeed => CharacterInfo.PlayerModel switch {
        PlayerModel.FElf => 0.6f,
        PlayerModel.MDwarf => 0.35f,
        _ => 0.5f
    };

    private float WalkSpeed => CharacterInfo.PlayerModel switch {
        PlayerModel.FFighter or PlayerModel.MFighter or PlayerModel.FMagic or PlayerModel.MMagic => 1.2f,
        _ => 1.5f
    };
    
    private void Awake() {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<BaseAnimationController>();
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    private void Update() {
        float distance = Vector3.Distance(targetPosition, transform.position);
        if(distance > 0.05f) {
            if(!IsWalkAnimationActive) {
                StartWalkingAnimation();
            }
            
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
            characterController.Move(transform.forward.normalized * (Time.deltaTime * WalkSpeed));
        } else if(IsWalkAnimationActive) {
            StopWalkingAnimation();
            transform.rotation = targetRotation;
        }
    }

    private void StartWalkingAnimation() {
        animationController.SetWalkSpeedLobby(WalkAnimationSpeed);
        animationController.SetBool("wait_" + WeaponAnim, false);
        animationController.SetBool("walk_" + WeaponAnim, true);
    }

    private void StopWalkingAnimation() {
        animationController.SetBool("walk_" + WeaponAnim, false);
        animationController.SetBool("wait_" + WeaponAnim, true);
    }

    public void MoveToAndRotate(Vector3 position, Quaternion rotation) {
        targetPosition = position;
        targetRotation = rotation;
    }

    public void ResetMovement() {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

}
