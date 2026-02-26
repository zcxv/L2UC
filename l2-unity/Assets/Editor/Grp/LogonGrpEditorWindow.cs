using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// A utility that transforms LogonGRP into CreatorEntity and SelectorSlotEntity triggers on the active scene.
/// </summary>
public class LogonGrpEditorWindow : EditorWindow {

    private struct Logongrp {
        public Vector3 position;
        public Vector3 rotation;

        public Logongrp(Vector3 position, Vector3 rotation) {
            this.position = position;
            this.rotation = rotation;
        }
    }
    
    [MenuItem("Database/LogonGRP Converter")]
    private static void ShowWindow() {
        var window = GetWindow<LogonGrpEditorWindow>();
        window.titleContent = new GUIContent("LogonGRP Converter");
        window.Show();
    }

    private const int CHARACTER_SELECT_SLOTS = 8;

    private static readonly List<PlayerModel> Models = new() {
        PlayerModel.MFighter,
        PlayerModel.FFighter,
        PlayerModel.MMagic,
        PlayerModel.FMagic,
        PlayerModel.MElf,
        PlayerModel.FElf,
        PlayerModel.MElf,
        PlayerModel.FElf,
        PlayerModel.MDarkElf,
        PlayerModel.FDarkElf,
        PlayerModel.MDarkElf,
        PlayerModel.FDarkElf,
        PlayerModel.MOrc,
        PlayerModel.FOrc,
        PlayerModel.MShaman,
        PlayerModel.FShaman,
        PlayerModel.MDwarf,
        PlayerModel.FDwarf
    };

    private string path = "";
    private bool setDefaultModels = true;
    
    private bool IsPathSet => path.Length > 0 && File.Exists(path);

    private void OnGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("File: ", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField(path);
        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("...", EditorStyles.miniButtonRight)) {
            OnFileSelectButtonPress();
        }
        GUILayout.EndHorizontal();

        setDefaultModels = EditorGUILayout.Toggle("Set default models", setDefaultModels);

        EditorGUI.BeginDisabledGroup(!IsPathSet);
        if (GUILayout.Button("Convert", EditorStyles.miniButton)) {
            OnConvertButtonPress();
        }
        EditorGUI.EndDisabledGroup();
    }

    private void OnFileSelectButtonPress() {
        path = EditorUtility.OpenFilePanel("Select LogonGRP File", "Assets", "txt");
    }

    private void OnConvertButtonPress() {
        GameObject triggers = GetTriggersRoot();
        Logongrp[] logons = LoadLogonGrps();
        
        var activeSlotMark = CreateSelectorActiveSlotMark(triggers, logons);
        CreateSelectorSlots(triggers, logons, activeSlotMark);
        CreateCreators(triggers, logons);
    }

    private GameObject GetTriggersRoot() {
        GameObject[] triggersRoots = (
            from root in EditorSceneManager.GetActiveScene().GetRootGameObjects()
            where root.name == "Triggers"
            select root
        ).ToArray();

        if (triggersRoots.Length > 1) {
            Debug.LogError("More than one 'Triggers' root game object found");
        }
        
        GameObject triggersRoot = triggersRoots.Length switch {
            0 => new GameObject("Triggers"),
            _ => triggersRoots[0]
        };
        
        return triggersRoot;
    }
    
    private Logongrp[] LoadLogonGrps() {
        List<Logongrp>  logongrps = new List<Logongrp>();
        using (StreamReader reader = new StreamReader(path)) {
            string line;
            while ((line = reader.ReadLine()) != null) {
                float x = 0f;
                float y = 0f;
                float z = 0f;
                float yaw = 0f;
                
                string[] keyvals = line.Split('\t');
                
                for (int i = 0; i < keyvals.Length; i++) {
                    if (!keyvals[i].Contains("=")) {
                        continue;
                    }

                    string[] keyval = keyvals[i].Split("=");
                    string key = keyval[0];
                    string value = keyval[1];

                    switch (key) {
                        case "x":
                            x = NumberUtils.FromIntToFloat(int.Parse(value));
                            break;
                        case "y":
                            y = NumberUtils.FromIntToFloat(int.Parse(value));
                            break;
                        case "z":
                            z = NumberUtils.FromIntToFloat(int.Parse(value));
                            break;
                        case "yaw":
                            yaw = NumberUtils.FromIntToFloat(int.Parse(value));
                            break;
                    }
                }

                Vector3 position = VectorUtils.ConvertPosToUnity(new Vector3(x, y, z));
                logongrps.Add(new Logongrp(position, new Vector3(0, 360.00f * yaw / 65536f, 0)));
            }
        }
        
        return logongrps.ToArray();
    }

    private GameObject[] CreateSelectorSlots(GameObject root, Logongrp[] logons, GameObject activeSlotMark) {
        GameObject[] slotGameObjects = new GameObject[CHARACTER_SELECT_SLOTS];
        for (int i = 0; i < CHARACTER_SELECT_SLOTS - 1; i++) {
            Logongrp logon = logons[i];
            
            var gameObject = slotGameObjects[i] = new GameObject($"SelectorSlotEntity-{i}");
            gameObject.transform.SetParent(root.transform);
            gameObject.transform.position = logon.position;
            gameObject.transform.eulerAngles = logon.rotation;
            gameObject.tag = Tags.TRIGGER;
            SelectorSlotEntity component = gameObject.AddComponent<SelectorSlotEntity>();
            component.Slot = i;
            component.ActiveSlotMark = activeSlotMark;
        }
        
        return slotGameObjects;
    }

    private GameObject CreateSelectorActiveSlotMark(GameObject root, Logongrp[] logons) {
        Logongrp logon = logons[CHARACTER_SELECT_SLOTS - 1];
        
        var gameObject = new GameObject("SelectorActiveSlotMarkEntity");
        gameObject.transform.SetParent(root.transform);
        gameObject.transform.position = logon.position;
        gameObject.transform.eulerAngles = logon.rotation;
        gameObject.tag = Tags.TRIGGER;
        gameObject.AddComponent<SelectorActiveSlotMarkEntity>();
        
        return gameObject;
    }

    private GameObject[] CreateCreators(GameObject root, Logongrp[] logons) {
        GameObject[] gameObjects = new GameObject[logons.Length - CHARACTER_SELECT_SLOTS];
        for (int i = CHARACTER_SELECT_SLOTS, j = 0; i < logons.Length; i++, j++) {
            Logongrp logon = logons[i];
            
            var gameObject = gameObjects[j] = new GameObject($"CreatorEntity-{j}");
            gameObject.transform.SetParent(root.transform);
            gameObject.transform.position = logon.position;
            gameObject.transform.eulerAngles = logon.rotation;
            gameObject.tag = Tags.TRIGGER;
            CreatorEntity component = gameObject.AddComponent<CreatorEntity>();
            if (setDefaultModels) {
                component.Model = Models[j];
            }
        }

        return gameObjects;
    }

}