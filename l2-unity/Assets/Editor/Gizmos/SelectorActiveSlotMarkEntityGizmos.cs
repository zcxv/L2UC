using UnityEditor;
using UnityEngine;

public static class SelectorActiveSlotMarkEntityGizmos {
    
    [DrawGizmo(GizmoType.Selected)]
    private static void DrawGizmosSelected(SelectorActiveSlotMarkEntity markEntity, GizmoType gizmoType) {
        GizmosUtils.DrawAvatarStub(markEntity.transform);
    }

    [DrawGizmo(GizmoType.NonSelected)]
    private static void DrawGizmosNonSelected(SelectorActiveSlotMarkEntity markEntity, GizmoType gizmoType) {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawIcon(markEntity.transform.position + (Vector3.up * 0.5f), "Gizmo_CharacterStay_MIP4.png");
    }
    
}