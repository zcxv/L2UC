using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.UIElements;
using static UnityEngine.Analytics.IAnalytic;


public class SkillListWindow : L2PopupWindow
{


    //new fields
    private BuilderTabs _builderTabs;
    private VisualTreeAsset _tabTemplate;
    private VisualTreeAsset _tabHeaderTemplate;
    private VisualTreeAsset _templateActiveSkill;
    private VisualTreeAsset _templatePassiveSkill;
    private VisualTreeAsset _templateLearnSkill;

    //templte items
    private VisualTreeAsset _templateBoxPanel;
    private VisualTreeAsset _templateSlotSkill;
    private VisualTreeAsset _templateSkillsRow8x1;

    private const string _activeName = "Active";
    private const string _passiveName = "Passive";
    private const string _learnName = "Learn Skill";
    private List<SkillServer> _skillList;


    //end fields




    public VisualElement minimal_panel;
    private VisualElement _boxContent;

    private VisualElement _boxHeader;
    private VisualElement _rootWindow;
    private ButtonSkillLearn _button;
    private bool isHide;
    private VisualElement[] _menuItems;
    private VisualElement[] _rootTabs;
    private int[] _arrDfActiveSelect;
    private int[] _arrDfPassiveSelect;

    private VisualElement _activeTab_physicalContent;
    private VisualElement _activeTab_magicContent;
    private VisualElement _activeTab_enhancingContent;
    private VisualElement _activeTab_debilitatingContent;
    private VisualElement _activeTab_clanContent;
    private string _border_gold = "itemwindow_df_frame_Clear";

    private VisualElement _passiveTab_abilityContent;
    private VisualElement _passiveTab_subjectContent;

    //Debilitating
    private ActiveSkillsHide _supportActiveSkills;
    private PassiveSkillsHide _supportPassiveSkills;


    //All Active Skills
    private Dictionary<int, Skillgrp> _activeSkills;
    //All Passive Skills
    private Dictionary<int, Skillgrp> _passiveSkills;

    private Dictionary<int, VisualElement> _physicalSkillsRow;
    private Dictionary<int, VisualElement> _magicSkillsRow;
    private Dictionary<int, VisualElement> _enchancingSkillsRow;
    private Dictionary<int, VisualElement> _debilitatingSkillsRow;
    private Dictionary<int, VisualElement> _clanSkillsRow;

    private Dictionary<int, VisualElement> _abillitySkillsRow;
    private Dictionary<int, VisualElement> _subjectSkillsRow;
    private int _sizeActiveCells = 42;
    private int _sizePassiveCells = 14;

    private static SkillListWindow _instance;
    public static SkillListWindow Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _builderTabs = new BuilderTabs();
            _supportActiveSkills = new ActiveSkillsHide(this);


            _button = new ButtonSkillLearn(this);
            _menuItems = new VisualElement[3];
            _rootTabs = new VisualElement[3];
            _arrDfActiveSelect = new int[5] { 0,0,0,0,0};
            _arrDfPassiveSelect = new int[2] { 0, 0 };

            _supportPassiveSkills = new PassiveSkillsHide(this);
            //42 cells active panels
            _physicalSkillsRow = new Dictionary<int, VisualElement>(7);
            _magicSkillsRow = new Dictionary<int, VisualElement>(7);
            _enchancingSkillsRow = new Dictionary<int, VisualElement>(7);
            _debilitatingSkillsRow = new Dictionary<int, VisualElement>(7);
            _clanSkillsRow = new Dictionary<int, VisualElement>(14);

            _activeSkills = new Dictionary<int, Skillgrp>(_sizeActiveCells);
            _passiveSkills = new Dictionary<int, Skillgrp>(_sizeActiveCells);
            //14 cells passive panels > temporarily
            _abillitySkillsRow = new Dictionary<int, VisualElement>(_sizePassiveCells);
            _subjectSkillsRow = new Dictionary<int, VisualElement>(_sizePassiveCells);
        }
        else
        {
            Destroy(this);
        }
    }

    public bool IsWindowContain(Vector2 vector2)
    {
        return  _windowEle.worldBound.Contains(vector2);
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/SkillLearn/SkillListWindow");
        _tabTemplate = LoadAsset("Data/UI/_Elements/Game/Inventory/InventoryTab");
        _tabHeaderTemplate = LoadAsset("Data/UI/_Elements/Game/SkillLearn/SkillTabHeader");

        _templateActiveSkill = LoadAsset("Data/UI/_Elements/Game/SkillLearn/TabActive");
        _templatePassiveSkill = LoadAsset("Data/UI/_Elements/Game/SkillLearn/TabPassive");
        _templateLearnSkill = LoadAsset("Data/UI/_Elements/Game/SkillLearn/TabLearn");

        //template panels 8x1 skills
        _templateBoxPanel = LoadAsset("Data/UI/_Elements/Template/Skills/SkillBoxRow");
        _templateSkillsRow8x1 = LoadAsset("Data/UI/_Elements/Template/Skills/SkillPanels/SkillsRow8x1");
        _templateSlotSkill = LoadAsset("Data/UI/_Elements/Template/Skills/SkillPanels/SlotSkill");
    }

    protected override IEnumerator BuildWindow(VisualElement root)
    {
        InitWindow(root);


        yield return new WaitForEndOfFrame();

        var dragArea = GetElementByClass("drag-area");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);

        var content = (VisualElement)GetElementById("content");

        _builderTabs.InitContentTabs(new string[3] { _activeName, _passiveName , _learnName });
        _builderTabs.CreateTabs(content, _tabTemplate, _tabHeaderTemplate);

        _supportActiveSkills.SetActiveSkillTemplate(_templateActiveSkill , _templateBoxPanel , _templateSkillsRow8x1 , _templateSlotSkill);
        _supportPassiveSkills.SetPassiveSkillTemplate(_templatePassiveSkill, _templateBoxPanel, _templateSkillsRow8x1, _templateSlotSkill);

        _builderTabs.EventSwitchOut += OnSwitchEventOut;

        VisualElement element = _builderTabs.GetActiveContent();
        _supportActiveSkills.GetOrCreateTab(element);

        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(_root);


 


        //ToolTipManager.GetInstance().RegisterCallbackActiveSkills(_physicalSkillsRow , this);
        //ToolTipManager.GetInstance().RegisterCallbackActiveSkills(_magicSkillsRow, this);
        //ToolTipManager.GetInstance().RegisterCallbackActiveSkills(_enchancingSkillsRow, this);
        //ToolTipManager.GetInstance().RegisterCallbackActiveSkills(_debilitatingSkillsRow,  this);
        //ToolTipManager.GetInstance().RegisterCallbackActiveSkills(_clanSkillsRow,  this);

        //ToolTipManager.GetInstance().RegisterCallbackPassiveSkills(_abillitySkillsRow, this);
        //ToolTipManager.GetInstance().RegisterCallbackPassiveSkills(_subjectSkillsRow, this);

        //ToolTipManager.Instance.RegisterCallbackSkills(_abillitySkillsRow, 1 , this);
        //ToolTipManager.Instance.RegisterCallbackSkills(_subjectSkillsRow , 1 ,  this);

       
    }

    public void SetSkillList(List<SkillInstance> list)
    {
        if (list == null) return;
        PrintList(list);
        var activeSkills = list.Where(s => !s.IsPassive).ToList();
        var passiveSkills = list.Where(s => s.IsPassive).ToList();
        _supportActiveSkills.CreateSlots(activeSkills);
        _supportPassiveSkills.CreateSlots(passiveSkills);
    }

    public void UpdateSkillList(List<SkillInstance> list)
    {
        if (list == null) return;
        PrintList(list);
        var activeSkills = list.Where(s => !s.IsPassive).ToList();
        var passiveSkills = list.Where(s => s.IsPassive).ToList();
        _supportActiveSkills.UpdateSlots(activeSkills);
        _supportPassiveSkills.CreateSlots(passiveSkills);
    }

    public void PrintList(List<SkillInstance> list)
    {
        foreach (var skill in list)
        {
            Debug.Log(skill.SkillID);
        }

        Debug.Log("Size list " + list.Count);
    }

    // }

    // _activeName = "Active";
    //_passiveName = "Passive";
    // _learnName = "Learn Skill";
    private void OnSwitchEventOut(ITab tab, bool isTrade)
    {
        Debug.Log("Event Switch Tab !!!!");
        var tabName = tab.GetTabName();
        var element = _builderTabs.GetActiveContent();
        switch (tabName)
        {
            case _activeName:
                _supportActiveSkills.GetOrCreateTab(element);
                break;

            case _passiveName:
                _supportPassiveSkills.GetOrCreateTab(element);
                break;

            case _learnName:

                break;
        }


    }
    public Skillgrp GetSkillIdByCellId(int active , int cellId)
    {
        if(active == 1)
        {
            if(_activeSkills.ContainsKey(cellId)) return _activeSkills[cellId];
        }
        else if(active == 2)
        {
            if (_passiveSkills.ContainsKey(cellId)) return _passiveSkills[cellId];
        }
        return null;
    }



  
    public void HideElement(bool is_hide, VisualElement line)
    {
        if (is_hide)
        {
            line.style.display = DisplayStyle.None;
        }
        else
        {
            line.style.display = DisplayStyle.Flex;
        }

    }

}
