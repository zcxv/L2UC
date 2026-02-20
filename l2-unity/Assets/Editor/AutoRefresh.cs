using UnityEditor;

[InitializeOnLoad]
public static class AutoRefresh {
    
    private const string MENU_NAME = "Jobs/Auto Refresh In Play Mode";
    
    private static bool enabled;
    
    static AutoRefresh() {
        enabled = EditorPrefs.GetBool(MENU_NAME, false);

        EditorApplication.delayCall += () => {
            Menu.SetChecked(MENU_NAME, enabled);
            ToggleAutoRefresh(enabled);
        };
    }
    
    [MenuItem(MENU_NAME)]
    private static void ToggleMenuItem() {
        enabled = !enabled;
        Menu.SetChecked(MENU_NAME, enabled);
        EditorPrefs.SetBool(MENU_NAME, enabled);
        
        ToggleAutoRefresh(enabled);
    }

    private static void ToggleAutoRefresh(bool enabled) {
        if (!EditorPrefs.HasKey("kAutoRefresh")) {
            return;
        }

        if (enabled) {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorPrefs.SetBool("kAutoRefresh", true);
        } else {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
    }
    
    private static void OnPlayModeStateChanged(PlayModeStateChange playModeState) {
        switch (playModeState) {
            case PlayModeStateChange.EnteredPlayMode:
                EditorPrefs.SetBool("kAutoRefresh", false);
                break;
            case PlayModeStateChange.EnteredEditMode:
                EditorPrefs.SetBool("kAutoRefresh", true);
                break;
        }
    }
    
}