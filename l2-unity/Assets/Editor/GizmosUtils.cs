using UnityEditor;
using UnityEngine;

public class GizmosUtils {
    
    private const string AVATAR_STUB_PREFAB = "Assets/Resources/Editor/Prefab/AvatarStub.prefab";

    private static GameObject avatarStub;
    
    static GizmosUtils() {
        avatarStub = AssetDatabase.LoadAssetAtPath<GameObject>(AVATAR_STUB_PREFAB);
    }
    
    public static void DrawAvatarStub(Transform targetTransform) {;
        if (avatarStub == null) {
            return;
        }

        Matrix4x4 localToWorld = targetTransform.localToWorldMatrix;
        Gizmos.matrix = localToWorld;
        Gizmos.color = Color.grey;
        
        SkinnedMeshRenderer[] meshRenderers = avatarStub.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; ++i) {
            SkinnedMeshRenderer meshRenderer = meshRenderers[i];
            Mesh mesh = meshRenderer.sharedMesh;
            
            Transform meshTransform = meshRenderer.transform;
            Vector3 position = meshTransform.position;
            Quaternion rotation = meshTransform.rotation;
            Vector3 scale = meshTransform.lossyScale;
            
            Gizmos.DrawMesh(mesh, position, rotation, scale);
        }
    }
    
}