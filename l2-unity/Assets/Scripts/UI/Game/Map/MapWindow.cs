using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class MapWindow : L2PopupWindow
{
    private static MapWindow _instance;
    private ICreatorSimpleTab _creatorSimpleTab;
    private const string _tabName = "Aden";
    private VisualTreeAsset _tabTemplate;
    private VisualTreeAsset _tabHeaderTemplate;
    private const string _mapTemplateName = "Data/UI/_Elements/Game/Map/MapAdenTemplate";
    private MapPanner _mapPanner;
    public static MapWindow Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _creatorSimpleTab = new CreatorSimpleTab();
            _mapPanner = new MapPanner();
            _instance = this;

        }
        else
        {
            Destroy(this);
        }
    }

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/Map/MapWindow");
        _tabTemplate = LoadAsset("Data/UI/_Elements/Game/Inventory/InventoryTab");
        _tabHeaderTemplate = LoadAsset("Data/UI/_Elements/Game/Inventory/InventoryTabHeader");
        //id 0 - Map Aden
        _creatorSimpleTab.LoadAsset(LoadAsset, new string[1] { _mapTemplateName });
    }

    protected override IEnumerator BuildWindow(VisualElement root)
    {
        InitWindow(root);

        var dragArea = GetElementByClass("drag-area");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);


        var content = (VisualElement)GetElementById("content");

        _creatorSimpleTab.InitContentTabs(new string[1] { _tabName });
        _creatorSimpleTab.CreateTabs(content, _tabTemplate, _tabHeaderTemplate);
        _creatorSimpleTab.SetContent(0);

        var elements = _creatorSimpleTab.GetVisualElements(new string[2] { "viewport" , "map" });
        _mapPanner.SetElements(elements[0], elements[1]);
        _mapPanner.RegisterCallback();

        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(_root);

        yield return new WaitForEndOfFrame();

  
    }
}

