using UnityEditor;
using UnityEngine;

public static class URPLitBaseMapAutoMapper {
    
    [MenuItem("Tools/Assign Missing BaseMaps")]
    public static void MapSelected() {
        var materials = Selection.GetFiltered<Material>(SelectionMode.Assets);

        foreach (var material in materials) {
            if (material.shader.name != "Universal Render Pipeline/Lit") {
                continue;
            }

            if (material.GetTexture("_BaseMap") != null) {
                continue;
            }

            var guids = AssetDatabase.FindAssets($"{material.name} t:Texture");
            if (guids.Length == 0) {
                Debug.LogError($"[AutoMap] Texture not found for material: {material.name}", material);
                continue;
            } 
            
            if (guids.Length > 1) {
                Debug.LogError($"[AutoMap] Found multiple textures material: {material.name}", material);
                continue;
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var tex = AssetDatabase.LoadAssetAtPath<Texture>(path);

            Undo.RecordObject(material, "Assign BaseMap");
            material.SetTexture("_BaseMap", tex);
        }
    }
    
}