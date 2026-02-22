using System;
using UnityEngine;

public class SelectorSlotEntity : MonoBehaviour {
    
    /// <summary>
    /// Selection slot.
    /// </summary>
    public int Slot;
    
    public GameObject ActiveSlotMark;

    [field: NonSerialized]
    public bool IsSelected { get; private set; }

    [field: NonSerialized]
    public GameObject Avatar { get; private set; }
    
    private SelectableAvatarEntity entity;
    
    public void SetPackage(CharSelectInfoPackage package) {
        Avatar = AvatarFactory.Create(package.PlayerModel, package.Appreance);
        Avatar.name = package.Name;
        Avatar.transform.position = transform.position;
        Avatar.transform.rotation = transform.rotation;
        Avatar.transform.parent = SceneLocator.Instance.Avatars.transform;
        
        UserGear gear = Avatar.GetComponent<UserGear>();
        entity = Avatar.GetComponent<SelectableAvatarEntity>();
        entity.CharacterInfo = package;
        entity.WeaponAnim = gear.WeaponAnim; // TODO m0nster: move to SelectableCharacterEntity
        entity.ResetMovement();
        
        Avatar.SetActive(true);
        
        BaseAnimationController animController = Avatar.GetComponent<BaseAnimationController>();
        animController.Initialize();
        animController.SetBool("wait_" + gear.WeaponAnim, true); // TODO m0nster: move to anim controller
    }

    public void RemovePackage() {
        if (Avatar != null) {
            Destroy(Avatar);
            IsSelected = false;
            Avatar = null;
            entity = null;
        }
    }

    public void Select() {
        entity.MoveToAndRotate(ActiveSlotMark.transform.position, ActiveSlotMark.transform.rotation);
        IsSelected = true;
    }

    public void Unselect() {
        entity.MoveToAndRotate(transform.position, transform.rotation);
        IsSelected = false;
    }

}