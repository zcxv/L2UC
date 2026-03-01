using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

[CustomEditor(typeof(Texture2DArray))]
public class Texture2DArrayEditor : Editor {
    
    private readonly List<Texture2D> textures = new();

    private bool isMipmapEnabled {
        get => flags == TextureCreationFlags.MipChain;
        set => flags = value ? TextureCreationFlags.MipChain : TextureCreationFlags.None;
    }

    private TextureCreationFlags flags;
    
    private ReorderableList reorderableList;
    
    private void OnEnable() {
        Texture2DArray texture2DArray = (Texture2DArray)target;
        LoadTextures(texture2DArray);
        CreateList();
    }

    private void LoadTextures(Texture2DArray texture2DArray) {
        int width = texture2DArray.width;
        int height = texture2DArray.height;
        GraphicsFormat graphicsFormat = texture2DArray.graphicsFormat;
        int mipMapCount = texture2DArray.mipmapCount;
        flags = texture2DArray.mipmapCount > 1 ? TextureCreationFlags.MipChain : TextureCreationFlags.None;

        for (int i = 0; i < texture2DArray.depth; i++) {
            Texture2D texture = new Texture2D(width, height, graphicsFormat, mipMapCount, flags);
            Graphics.CopyTexture(texture2DArray, i, texture, 0);
            textures.Add(texture);
        }
    }

    private void CreateList() {
        reorderableList = new ReorderableList(textures, typeof(Texture2D)) {
            elementHeight = 52,
            drawHeaderCallback = DrawHeader,
            drawElementCallback = DrawElement,
            onAddCallback = OnAdd,
            onRemoveCallback = OnRemove,
            onReorderCallbackWithDetails = OnReorder
        };
    }
    
    public override void OnInspectorGUI() {
        //DrawDefaultInspector();
        //EditorGUILayout.Space();
        isMipmapEnabled = GUILayout.Toggle(isMipmapEnabled, "Mip Maps");
        EditorGUILayout.Space();
        reorderableList.DoLayoutList();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(hasUnsavedChanges);
        if (GUILayout.Button("Revert")) {
            DiscardChanges();
        }

        if (GUILayout.Button("Apply")) {
            SaveChanges();
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
    }
    
    private void DrawHeader(Rect rect) {
        EditorGUI.LabelField(rect, "Textures");
    }
    
    private void DrawElement(Rect rect, int index, bool active, bool focus) {
        Texture2D texture = textures[index];
        Rect r = new Rect(rect.x, rect.y, 50, 50);
        textures[index] = (Texture2D)EditorGUI.ObjectField(r, texture, typeof(Texture2D), false);

        if (texture != null) {
            r = new Rect(rect.x + 52, rect.y, rect.width - 52, 15);
            EditorGUI.LabelField(r, "Width : " + texture.width + " Height : " + texture.height);
            r = new Rect(rect.x + 52, rect.y + 15, rect.width - 52, 15);
            EditorGUI.LabelField(r, "Mipmap Count : " + texture.mipmapCount);
            r = new Rect(rect.x + 52, rect.y + 30, rect.width - 52, 15);
            EditorGUI.LabelField(r, "Format : " + texture.format);
        }
    }
    
    private void OnAdd(ReorderableList list) {
        textures.Add(Texture2D.whiteTexture);
        hasUnsavedChanges = true;
    }

    private void OnRemove(ReorderableList list) {
        textures.RemoveAt(list.index);
        hasUnsavedChanges = true;
    }

    private void OnReorder(ReorderableList list, int oldIndex, int newIndex) {
        (textures[oldIndex], textures[newIndex]) = (textures[newIndex], textures[oldIndex]);
        hasUnsavedChanges = true;
    }

    private void OnDisable() {
        textures.Clear();
    }

    public override void SaveChanges() {
        base.SaveChanges();
        
        Texture2D tex0 = textures[0];
        
        int mipmapCount = isMipmapEnabled ? tex0.mipmapCount : 1;
        Texture2DArray tex2dArray = new Texture2DArray(tex0.width, tex0.height, textures.Count, tex0.graphicsFormat, flags, mipmapCount);
        for (int i = 0; i < textures.Count; i++) {
            Texture2D tex = textures[i];

            if (!isMipmapEnabled) {
                // Copy only Mip0
                Graphics.CopyTexture(tex, 0, 0, tex2dArray, i, 0);
            } else {
                // Copy all Mips
                Graphics.CopyTexture(tex, 0, tex2dArray, i);
            }
        }

        
        EditorUtility.CopySerialized(tex2dArray, target);
        AssetDatabase.SaveAssets();
    }

    public override void DiscardChanges() {
        base.DiscardChanges();
        
        textures.Clear();
        LoadTextures((Texture2DArray) target);
    }
}