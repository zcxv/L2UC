using UnityEditor;
using UnityEngine;

public static class SelectorSlotEntityGizmos {
    
    [DrawGizmo(GizmoType.Selected)]
    private static void DrawGizmosSelected(SelectorSlotEntity selectorSlot, GizmoType gizmoType) {
        GizmosUtils.DrawAvatarStub(selectorSlot.transform);
    }

    [DrawGizmo(GizmoType.NonSelected)]
    private static void DrawGizmosNonSelected(SelectorSlotEntity selectorSlot, GizmoType gizmoType) {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawIcon(selectorSlot.transform.position + (Vector3.up * 0.5f), "Gizmo_CharacterStay_MIP4.png");
    }
    
}