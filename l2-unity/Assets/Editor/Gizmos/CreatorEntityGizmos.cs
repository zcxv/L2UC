using UnityEditor;
using UnityEngine;

public static class CreatorEntityGizmos {
    
    [DrawGizmo(GizmoType.Selected)]
    private static void DrawGizmosSelected(CreatorEntity creatorEntity, GizmoType gizmoType) {
        GizmosUtils.DrawAvatarStub(creatorEntity.transform);
    }

    [DrawGizmo(GizmoType.NonSelected)]
    private static void DrawGizmosNonSelected(CreatorEntity creatorEntity, GizmoType gizmoType) {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawIcon(creatorEntity.transform.position + (Vector3.up * 0.5f), "Gizmo_CharacterStay_MIP4.png");
    }
    
}