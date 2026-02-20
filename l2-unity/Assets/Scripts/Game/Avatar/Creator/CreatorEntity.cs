using System;
using UnityEngine;

public class CreatorEntity : MonoBehaviour {

    public PlayerModel Model;
    public CreatorClassType ClassType;

    public CreatorRace Race => Model switch {
        PlayerModel.MElf or PlayerModel.FElf => CreatorRace.Elf,
        PlayerModel.MDarkElf or PlayerModel.FDarkElf => CreatorRace.DarkElf,
        PlayerModel.MOrc or PlayerModel.FOrc => CreatorRace.Orc,
        PlayerModel.MDwarf or PlayerModel.FDwarf => CreatorRace.Dwarf,
        _ => CreatorRace.Human
    };
    
    public int Sex => Enum.GetName(typeof(PlayerModel), Model).StartsWith("M") ? 0 : 1;

    [NonSerialized]
    public GameObject Avatar;
    
    public void Spawn() {
        Avatar = AvatarFactory.Create(Model, new PlayerAppearance());
        Avatar.name = $"Create_{Enum.GetName(typeof(PlayerModel), Model)}";
        Avatar.transform.parent = SceneLocator.Instance.Avatars.transform;
        Avatar.transform.position = transform.position;
        Avatar.transform.rotation = transform.rotation;
        Avatar.SetActive(true);
            
        // TODO m0nster: перенести это в сам аним контроллер, в OnActive/OnInactive
        BaseAnimationController animController = Avatar.GetComponent<BaseAnimationController>();
        animController.Initialize();
        UserGear gear = Avatar.GetComponent<UserGear>();
        animController.SetBool("wait_" + gear.WeaponAnim, true);
    }

    public void ResetModel() {
        if (Avatar != null) {
            Avatar.transform.rotation = transform.rotation;
            SetFace(0);
            SetHair(0, 0);
        }
    }

    public void SetFace(int faceId) {
        UserGear gear = Avatar.GetComponent<UserGear>();
        var hairPrototype = ModelTable.Instance.GetFace(Model, faceId);
        if (hairPrototype == null) {
            return;
        }
        
        gear.EquipFace(Instantiate(hairPrototype));
    }

    public void SetHair(int hairColor, int hairStyle) {
        var hair1M = ModelTable.Instance.GetHair(Model, hairStyle, hairColor, false);
        var hair2M = ModelTable.Instance.GetHair(Model, hairStyle, hairColor, true);
        if (hair1M == null || hair2M == null) {
            return;
        }

        UserGear gear = Avatar.GetComponent<UserGear>();
        gear.EquipHair(Instantiate(hair1M), Instantiate(hair2M));
    }

    public void Rotate(Vector3 rotation) {
        transform.eulerAngles += rotation;
    }

}