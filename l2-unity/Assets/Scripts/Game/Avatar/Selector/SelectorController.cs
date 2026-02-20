using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SelectorService))]
public class SelectorController : MonoBehaviourSingleton<SelectorController> {
    
    public const int UNSELECTED_SLOT = -1;

    [NonSerialized]
    public Camera Camera;
    public LayerMask LayerMask;

    public int SelectedSlot => SelectorService.Instance.SelectedSlot;

    public void SetPackages(List<CharSelectInfoPackage> packages) {
        SelectorService.Instance.SetPackages(packages);
    }

    public void Select(int slotIndex) {
        SelectorService.Instance.Select(slotIndex);
    }

    private void Update() {
        if (!Input.GetMouseButtonDown(0)) {
            return;
        }
        
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, LayerMask)) {
            SelectorService.Instance.Select(hit.transform.parent.gameObject);
        }
    }
}