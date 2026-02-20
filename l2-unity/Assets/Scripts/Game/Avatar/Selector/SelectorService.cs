using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class SelectorService : MonoBehaviourSingleton<SelectorService> {

    public int SelectedSlot => selectedSlot?.Slot ?? -1;
    
    private SelectorSlotEntity[] slots;
    private SelectorSlotEntity selectedSlot;

    private void Start() {
        slots = GameObject.FindGameObjectsWithTag(Tags.TRIGGER)
            .Select(go => go.GetComponent<SelectorSlotEntity>())
            .Where(go => go != null)
            .OrderBy(go => go.Slot)
            .ToArray();
    }
    
    public bool SetPackages(List<CharSelectInfoPackage> packages) {
        if (packages.Count > slots.Length) {
            Debug.LogError($"Too many packages: {packages.Count} with maximum {slots.Length}!");
            return false;
        }
        
        ResetPackages();
        for (int i = 0; i < packages.Count; i++) {
            SelectorSlotEntity slot = slots[i];
            slot.SetPackage(packages[i]);
        }
        
        return true;
    }

    public void ResetPackages() {
        for (int i = 0; i < slots.Length; i++) {
            slots[i].RemovePackage();
        }

        selectedSlot = null;
    }
    
    public bool Select(GameObject selectedObject) {
        SelectorSlotEntity slot = FindSlot(selectedObject);
        if (slot == null) {
            return false;
        }

        if (selectedSlot != null) {
            selectedSlot.Unselect();
        }
        
        slot.Select();
        selectedSlot = slot;

        CharSelectWindow.Instance.SelectSlot(slot.Slot);
        return true;
    }
    
    public void Select(int slotIndex) {
        SelectorSlotEntity slot = slots[slotIndex];
        
        if (selectedSlot != null) {
            selectedSlot.Unselect();
        }
        
        slot.Select();
        selectedSlot = slot;

        CharSelectWindow.Instance.SelectSlot(slot.Slot);
    }

    private SelectorSlotEntity FindSlot(GameObject selectedObject) {
        for (int i = 0; i < slots.Length; i++) {
            var slot = slots[i];
            if (slot.Avatar == selectedObject) {
                return slot;
            }
        }
        
        return null;
    }

}