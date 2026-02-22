using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FindMissingScripts {

    [MenuItem("Tools/Find Missing Scripts")]
    public static void Find() {
        for (int i = 0; i < EditorSceneManager.sceneCount; i++) {
            Scene scene = EditorSceneManager.GetSceneAt(i);
            FindInScene(scene);
        }
        
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids) {
            FindInPrefab(guid);
        }
    }

    private static void FindInScene(Scene scene) {
        GameObject[] roots = scene.GetRootGameObjects();
        foreach (GameObject root in roots) {
            FindInGameObject(root);
        }
    }
 
    private static void FindInGameObject(GameObject gameObject) {
        Component[] components = gameObject.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++) {
            if (components[i] == null) {
                Debug.LogWarning($"{gameObject.name} has an empty script", gameObject);
            }
        }
        
        foreach (Transform child in gameObject.transform) {
            FindInGameObject(child.gameObject);
        }
    }

    private static void FindInPrefab(string guid) {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null) {
            return;
        }
        
        FindInGameObject(prefab);
    }
    
}