using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestWindow : L2PopupWindow
{

    private static QuestWindow _instance;
    private BuilderTabs _builderTabs;

    private VisualTreeAsset _tabTemplate;
    private VisualTreeAsset _tabHeaderTemplate;
    private VisualTreeAsset _tabContenTemplate;
    private VisualTreeAsset _singleInsideContentTemplate;
    private VisualTreeAsset _toggleButtonTemplate;

    private IContent _singlContent;
    private const string _singleTabName = "Single";
    private const string _repeatTabName = "Repeat";
    private const string _epicTabName = "Epic";
    private const string _transferTabName = "Transfer";
    private const string _specialTabName = "Special";
    public static QuestWindow Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _builderTabs = new BuilderTabs();
            _singlContent = new SingleContentTab();
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/Quest/QuestWindow");


        _tabTemplate = LoadAsset("Data/UI/_Elements/Game/Inventory/InventoryTab");
        _tabHeaderTemplate = LoadAsset("Data/UI/_Elements/Game/SkillLearn/SkillTabHeader");
        _singleInsideContentTemplate = LoadAsset("Data/UI/_Elements/Game/Quest/AllContent/SingleInsideContent");
        _tabContenTemplate = LoadAsset("Data/UI/_Elements/Game/Quest/QuestTabContent");
        _toggleButtonTemplate = LoadAsset("Data/UI/_Elements/Template/Elements/ToggleButtons/ToggleButton");
    }

    protected override IEnumerator BuildWindow(VisualElement root)
    {
        InitWindow(root);
        yield return new WaitForEndOfFrame();

        var dragArea = GetElementByClass("drag-area");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);
        var content = (VisualElement)GetElementById("content");

        var bg = (VisualElement)GetElementById("Darkener");

        if (bg != null)
        {
             bg.style.opacity = new StyleFloat(0.9f);
        }


        _builderTabs.InitContentTabs(new string[5] { _singleTabName, _repeatTabName, _epicTabName , _transferTabName , _specialTabName });
        _builderTabs.CreateTabs(content, _tabTemplate, _tabHeaderTemplate);

        
        _singlContent.SetTemplateContent(_tabContenTemplate , new List<VisualTreeAsset>() { _singleInsideContentTemplate , _toggleButtonTemplate });
 

        _builderTabs.EventSwitchOut += OnSwitchEventOut;

        VisualElement element = _builderTabs.GetActiveContent();
        _singlContent.GetOrCreateTab(element);

        RegisterCloseWindowEvent("btn-close-frame");
        //RegisterCloseWindowEventByName("CloseButton");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(_root);

        HideWindow();
    }

    public void AddData(List<QuestInstance> quests)
    {
        _singlContent.AddElementsToContent(quests);
    }

    private void OnSwitchEventOut(ITab tab, bool isTrade)
    {
        var tabName = tab.GetTabName();
        var element = _builderTabs.GetActiveContent();

        switch (tabName)
        {
            case _singleTabName:
                _singlContent.GetOrCreateTab(element);
                break;
        }


    }
}
