using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class EditorGUILayoutExt {

    public static string TextField(string text, Regex pattern, params GUILayoutOption[] options) {
        string value = EditorGUILayout.TextField(text, options);
        return pattern.IsMatch(value) ? value : text;
    }
    
    public static string TextField(string label, string text, Regex pattern, params GUILayoutOption[] options) {
        string value = EditorGUILayout.TextField(label, text, options);
        return pattern.IsMatch(value) ? value : text;
    }
    
    
}