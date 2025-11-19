using UnityEngine;
using System.Collections.Generic;

public class ListenerCharacterGear : MonoBehaviour
{

    public static ListenerCharacterGear Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
    
        EventBus.Instance.OnEquipped += HandleItemEquipped;
        EventBus.Instance.OnUnEquipped += HandleItemUnequipped;
    }

    private void OnDisable()
    {
     
        EventBus.Instance.OnEquipped -= HandleItemEquipped;
        EventBus.Instance.OnUnEquipped -= HandleItemUnequipped;
    }

    private void HandleItemEquipped(ItemInstance item , int objectId)
    {
        Entity entity = World.Instance.GetEntityNoLockSync(objectId);
        if(item.Category == ItemCategory.Weapon)
        {
            if (entity != null) entity.EquipWeapon(item.ItemId, false);
        }
        else if(item.Category == ItemCategory.ShieldArmor)
        {
            //if (entity != null) entity.EquipShield(item.ItemId, false);
        }

    }

    private void HandleItemUnequipped(ItemInstance item, int objectId)
    {
        Entity entity = World.Instance.GetEntityNoLockSync(objectId);
        if (entity != null) entity.UnEquipWeapon(false);
    }

    
}
