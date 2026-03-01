using System.IO;
using UnityEditor;
using UnityEngine;

public class FindSpriteShaders {
    [MenuItem("Tools/Find Sprite Shaders")]
    public static void Find() {
        FindShaders();
        FindShaderGraphs();
    }

    private static void FindShaders() {
        foreach (var guid in AssetDatabase.FindAssets("t:Shader")) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.StartsWith("Packages/")) {
                continue;
            }
            
            Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
            if (shader.name.Contains("Sprite-Lit") || shader.name.Contains("Sprite-Unlit")) {
                Debug.Log($"<color=green>Found:</color> {shader.name} at {path}", shader);
            }
        }
    }

    private static void FindShaderGraphs() {
        foreach (var guid in AssetDatabase.FindAssets("")) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.StartsWith("Packages/")) {
                continue;
            }
            
            if (path.EndsWith(".shadergraph")) {
                string content = File.ReadAllText(path);
                if (!content.Contains("UniversalSpriteLitSubTarget") && !content.Contains("UniversalSpriteUnlitSubTarget")) {
                    continue;
                }
                
                Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
                Debug.Log($"<color=green>Found:</color> {shader.name} at {path}", shader);
            }
        }
    }
}