using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;


public class SeedInfoWindow : L2PopupWindow
{
    private ICreator _creatorWindow;
    private ICreatorTables _creatorTable;
    private static SeedInfoWindow _instance;
    public static SeedInfoWindow Instance { get { return _instance; } }
    private VisualTreeAsset _tabTemplate;
    private VisualTreeAsset _tabHeaderTemplate;
    private List<TableColumn> _defaultColumns0;
    private List<TableColumn> _defaultColumns1;
    private List<TableColumn> _defaultColumns2;
    private readonly string[] _tabsName = new string[] { "Seeds", "Fruit", "Bas.Info" };

    private const string commandTab0 = "manor_menu_select?ask=3&state=1&time=0";
    private const string commandTab1 = "manor_menu_select?ask=4&state=1&time=0";
    private const string commandTab2 = "manor_menu_select?ask=5&state=1&time=0";

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/Manor/SeedInfoWindow");
        _tabTemplate = LoadAsset("Data/UI/_Elements/Game/Inventory/InventoryTab");
        _tabHeaderTemplate = LoadAsset("Data/UI/_Elements/Game/Inventory/InventoryTabHeader");
    }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _creatorWindow =  new CreatorTabsWindows();
            _creatorTable = new CreatorTableWindows();
        }
        else
        {
            Destroy(this);
        }
    }

    protected override IEnumerator BuildWindow(VisualElement root)
    {
        InitWindow(root);
        yield return new WaitForEndOfFrame();

        var dragArea = GetElementByClass("drag-area");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);
        var content = (VisualElement)GetElementById("content");


        _creatorTable.LoadAsset(LoadAsset);

        _defaultColumns0 = GetColumnWindowTab0();
        _defaultColumns1 = GetColumnWindowTab1();
        _defaultColumns2 = GetColumnWindowTab2();

        _creatorWindow.InitContentTabs(_tabsName);
        _creatorWindow.CreateTabs(content, _tabTemplate, _tabHeaderTemplate);
        _creatorWindow.InsertTablesIntoContent(_creatorTable, _defaultColumns0, true);
        _creatorWindow.EventSwitchTabByIndexOfTab += OnSwitchTab;

        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(_root);

    }

    private List<TableColumn> GetColumnWindowTab0()
    {

        var harvest = new TableColumn(false, "Name Of The Seed", 0, new List<string>() {  }, 13 , 200);
        var auth = new TableColumn(true, "In Stock", 0, new List<string>() {  }, 0);
        var buy = new TableColumn(true, "For Sale", 0, new List<string>() { }, 0);
        var price = new TableColumn(true, "Price", 0, new List<string>() {  }, 0);

        return new List<TableColumn> { harvest, auth, buy, price};
    }

    private List<TableColumn> GetColumnWindowTab1()
    {

        var harvest = new TableColumn(false, "Name Of Fruits", 0, new List<string>() {  }, 13, 200);
        var auth = new TableColumn(true, "In Stock", 0, new List<string>() {  }, 0);
        var buy = new TableColumn(true, "For Sale", 0, new List<string>() {  }, 0);
        var price = new TableColumn(true, "Price", 0, new List<string>() {  }, 0);
        var reward = new TableColumn(true, "Reward", 0, new List<string>() {  }, 0);

        return new List<TableColumn> { harvest, auth, buy, price , reward };
    }

    private List<TableColumn> GetColumnWindowTab2()
    {

        var harvest = new TableColumn(false, "Name Of Fruits", 0, new List<string>() { }, 13, 200);
        var auth = new TableColumn(true, "Lvl", 0, new List<string>() {  }, 0);
        var buy = new TableColumn(true, "Price Seed", 0, new List<string>() {  }, 0);
        var price = new TableColumn(true, "Price Fruits", 0, new List<string>() {  }, 0);
        var reward1 = new TableColumn(true, "Reward1", 0, new List<string>() {  }, 0);
        var reward2 = new TableColumn(true, "Reward2", 0, new List<string>() {  }, 0);

        return new List<TableColumn> { harvest, auth, buy, price , reward1, reward2 };
    }

    public void SetDataSeedInfo(List<SeedProduction> listSeedProduction)
    {
        List<string> harvest = new List<string>();
        List<string> auth = new List<string>();
        List<string> buy = new List<string>();
        List<string> price = new List<string>();


        foreach (var seedProduction in listSeedProduction)
        {
            harvest.Add(""+seedProduction.GetSeedId());
            auth.Add("" + seedProduction.GetAmount());
            buy.Add("" + seedProduction.GetStartAmount());
            price.Add("" + seedProduction.GetPrice());
        }

        _defaultColumns0[0].SetData(harvest);
        _defaultColumns0[1].SetData(auth);
        _defaultColumns0[2].SetData(buy);
        _defaultColumns0[3].SetData(price);

        _creatorTable.UpdateTableData(_defaultColumns0);
        Debug.Log("SetDataCropInfo 0");
    }

    public void SetDataCropInfo(List<CropProcure> listCropInfo)
    {
        List<string> harvest = new List<string>();
        List<string> auth = new List<string>();
        List<string> buy = new List<string>();
        List<string> price = new List<string>();
        List<string> reward = new List<string>();

        foreach (var seedProduction in listCropInfo)
        {
            harvest.Add("" + seedProduction.GetSeedId());
            auth.Add("" + seedProduction.GetAmount());
            buy.Add("" + seedProduction.GetStartAmount());
            price.Add("" + seedProduction.GetPrice());
            reward.Add("" + seedProduction.GetReward());
        }

        _defaultColumns1[0].SetData(harvest);
        _defaultColumns1[1].SetData(auth);
        _defaultColumns1[2].SetData(buy);
        _defaultColumns1[3].SetData(price);
        _defaultColumns1[4].SetData(reward);

        _creatorTable.UpdateTableData(_defaultColumns1);
        Debug.Log("SetDataCropInfo 1");
    }

    public void SetDataDefaultManorInfo(List<Seed> listSeed)
    {
        List<string> harvest = new List<string>();
        List<string> auth = new List<string>();
        List<string> buy = new List<string>();
        List<string> price = new List<string>();
        List<string> reward1 = new List<string>();
        List<string> reward2 = new List<string>();

        foreach (var seed in listSeed)
        {
            ItemName name = ItemNameTable.Instance.GetItemName(seed.CropId);

            if(name != null)
            {
                ItemName reward1name = ItemNameTable.Instance.GetItemName(seed.Reward1ItemId);
                ItemName reward2name = ItemNameTable.Instance.GetItemName(seed.Reward2ItemId);
                harvest.Add("" + name.Name);
                auth.Add("" + seed.SeedLevel);
                buy.Add("" + seed.SeedReferencePrice);
                price.Add("" + seed.CropReferencePrice);
                reward1.Add("" + reward1name.Name);
                reward2.Add("" + reward2name.Name);
            }
        }

        _defaultColumns2[0].SetData(harvest);
        _defaultColumns2[1].SetData(auth);
        _defaultColumns2[2].SetData(buy);
        _defaultColumns2[3].SetData(price);
        _defaultColumns2[4].SetData(reward1);
        _defaultColumns2[5].SetData(reward2);

        _creatorTable.UpdateTableData(_defaultColumns2);
    }

    public void OnSwitchTab(int indexTab)
    {
      
        if (!ArrayUtils.IsValidIndexArray(_tabsName, indexTab))
        {
            return;
        }

        switch (indexTab)
        {
            case 0:
                Debug.Log("Event index 0 ");
                HtmlWindow.Instance.UseActionCommand(commandTab0);
                _creatorTable.SetSelectRow(-1);
                _creatorWindow.RefreshDataColumns(_defaultColumns0);
                break;
            case 1:
                Debug.Log("Event index 1 ");
                HtmlWindow.Instance.UseActionCommand(commandTab1);
                _creatorTable.SetSelectRow(-1);
                _creatorWindow.RefreshDataColumns(_defaultColumns1);
                break;
            case 2:
                Debug.Log("Event index 2 ");
                HtmlWindow.Instance.UseActionCommand(commandTab2);
                _creatorTable.SetSelectRow(-1);
                _creatorWindow.RefreshDataColumns(_defaultColumns2);
                break;
        }
    }



}
