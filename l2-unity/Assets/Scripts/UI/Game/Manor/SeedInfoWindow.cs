using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SeedInfoWindow : L2PopupWindow
{
    private ICreator _creatorWindow;
    private ICreatorTables _creatorTable;
    private static SeedInfoWindow _instance;
    public static SeedInfoWindow Instance { get { return _instance; } }
    private VisualTreeAsset _tabTemplate;
    private VisualTreeAsset _tabHeaderTemplate;
    private List<TableColumn> _defaultColumns;

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

        _creatorWindow.InitContentTabs(new string[] { "Seeds", "Fruit" , "Bas.Info" });
        _creatorWindow.CreateTabs(content, _tabTemplate, _tabHeaderTemplate);
        _creatorWindow.InsertTablesIntoContent(_creatorTable, GetColumnWindow() , true);

        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(_root);

    }

    private List<TableColumn> GetColumnWindow()
    {

        var harvest = new TableColumn(false, "Name of the seed", 0, new List<string>() { "Seed: Dark Code", "Seed: New Dark Code", "Seed: Blue Coda" }, 13 , 200);
        var auth = new TableColumn(true, "In stock", 0, new List<string>() { "1990", "0", "1990" }, 0);
        var buy = new TableColumn(true, "For sale", 0, new List<string>() { "2000", "0", "190" }, 0);
        var price = new TableColumn(true, "Price", 0, new List<string>() { "50", "0", "50" }, 0);

        return new List<TableColumn> { harvest, auth, buy, price};
    }


}
