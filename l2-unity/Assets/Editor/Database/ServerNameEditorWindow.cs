using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ServerNameEditorWindow : EditorWindow {

    private const string TITLE = "Server Name Editor";

    [MenuItem("Database/Server Name Editor")]
    private static void ShowWindow() {
        var window = GetWindow<ServerNameEditorWindow>();
        window.titleContent = new GUIContent(TITLE);
        window.Show();
    }
    
    private EventCallback<ChangeEvent<int>> IdChangedCallback;
    private EventCallback<ChangeEvent<string>> NameChangedCallback;

    private List<ServerName> serverNames;

    private Toolbar toolbar;
    private MultiColumnListView listView;

    private void CreateGUI() {
        Load();
    }

    private void Load() {
        using StreamReader file = new StreamReader("Assets/StreamingAssets/Data/Meta/ServerNames.json");
        serverNames = JsonConvert.DeserializeObject<List<ServerName>>(file.ReadToEnd());
        serverNames.Sort((o1, o2) => o1.Id.CompareTo(o2.Id));
    }

    private void OnEnable() {
        Load();

        toolbar = BuildToolbar();
        listView = BuildMultiColumnListView();
        
        rootVisualElement.Add(toolbar);
        rootVisualElement.Add(listView);
    }

    private Toolbar BuildToolbar() {
        Toolbar toolbar = new();
        
        ToolbarButton addNewButton = new ToolbarButton(OnAddNewButtonClick);
        addNewButton.tooltip = "Add new server name";
        addNewButton.Add(new Image { image = EditorGUIUtility.IconContent("Toolbar Plus").image });
        toolbar.Add(addNewButton);

        ToolbarButton refreshButton = new ToolbarButton(OnRefreshButtonClick);
        refreshButton.tooltip = "Reload data from disk";
        refreshButton.Add(new Image { image = EditorGUIUtility.IconContent("Refresh").image });
        toolbar.Add(refreshButton);
        
        return toolbar;
    }

    private MultiColumnListView BuildMultiColumnListView() {
        Columns columns = new Columns {
            IdColumn,
            NameColumn,
            DeleteColumn
        };
        
        MultiColumnListView listView = new(columns) {
            itemsSource = serverNames,
            style = {
                flexGrow = 1
            }
        };
        listView.RegisterCallback<KeyDownEvent>(evt => {
            if (evt.keyCode == KeyCode.Delete) {
                var indices = new List<int>(listView.selectedIndices);
                if (indices.Count > 0) {
                    OnDeleteMultiple(indices);
                    evt.StopPropagation();
                }
            }
        });

        return listView;
    }

    private Column IdColumn => new() {
        title = "ID",
        width = 45,
        makeCell = () => new IntegerField(),
        bindCell = (e, i) => {
            IntegerField field = (IntegerField)e;
            field.SetValueWithoutNotify(serverNames[i].Id);

            if (IdChangedCallback != null) {
                field.UnregisterValueChangedCallback(IdChangedCallback);
            }

            IdChangedCallback = evt => OnIdChanged(i, evt);
            field.RegisterValueChangedCallback(IdChangedCallback);
        },
    };

    private Column NameColumn => new() {
        title = "Name",
        width = 120,
        makeCell = () => new TextField(),
        bindCell = (e, i) => {
            TextField field = (TextField)e;
            field.SetValueWithoutNotify(serverNames[i].Name);

            if (NameChangedCallback != null) {
                field.UnregisterValueChangedCallback(NameChangedCallback);
            }

            NameChangedCallback = evt => OnNameChanged(i, evt);
            field.RegisterValueChangedCallback(NameChangedCallback);
        },
    };

    private Column DeleteColumn => new() {
        title = "",
        width = 30,
        makeCell = () => {
            Button button = new Button {
                style = {
                    backgroundImage = (StyleBackground)EditorGUIUtility.IconContent("Toolbar Minus").image,
                    width = 18,
                    height = 18,
                    marginTop = 1,
                    marginLeft = 4
                }
            };

            button.RegisterCallback<MouseEnterEvent>(evt => button.style.unityBackgroundImageTintColor = Color.red);
            button.RegisterCallback<MouseLeaveEvent>(evt => button.style.unityBackgroundImageTintColor = Color.white);

            return button;
        },
        bindCell = (e, i) => {
            Button button = (Button)e;
            button.style.unityBackgroundImageTintColor = Color.white;
            button.clickable = new Clickable(() => OnDeleteItem(i));
        }
    };
    
    private void OnIdChanged(int index, ChangeEvent<int> evt) {
        string name = serverNames[index].Name;
        serverNames[index] = new ServerName(evt.newValue, name);
        hasUnsavedChanges = true;
    }

    private void OnNameChanged(int index, ChangeEvent<string> evt) {
        int id = serverNames[index].Id;
        serverNames[index] = new ServerName(id, evt.newValue);
        hasUnsavedChanges = true;
    }

    private void OnDeleteItem(int index) {
        serverNames.RemoveAt(index);
        listView.Rebuild();
        hasUnsavedChanges = true;
    }

    private void OnDeleteMultiple(List<int> indexes) {
        indexes.Sort((o1, o2) => o2.CompareTo(o1));
        foreach (var index in indexes) {
            serverNames.RemoveAt(index);
        }
        
        listView.Rebuild();
        hasUnsavedChanges = true;
    }
    
    private void OnAddNewButtonClick() {
        int lastId = serverNames.Max(e => e.Id);
        serverNames.Add(new ServerName(++lastId, ""));
        hasUnsavedChanges = true;
        listView.Rebuild();
    }

    private void OnRefreshButtonClick() {
        if (hasUnsavedChanges) {
            switch (EditorUtility.DisplayDialogComplex("Unsaved changes", "Save changes?", "Save", "Cancel", "Don't save")) {
                case 0:
                    SaveChanges();
                    Load();
                    break;
                case 2:
                    DiscardChanges();
                    break;
            }
        }
    }

    public override void SaveChanges() {
        base.SaveChanges();
        
        string content = JsonConvert.SerializeObject(serverNames, Formatting.Indented);
        using StreamWriter file = new StreamWriter("Assets/StreamingAssets/Data/Meta/ServerNames.json");
        file.Write(content);
        file.Flush();
    }

    public override void DiscardChanges() {
        base.DiscardChanges();
        
        Load();
        listView.Rebuild();
    }
}