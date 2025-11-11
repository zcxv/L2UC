using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;


public class CraftingItemWindow : L2PopupWindow
{
    private ICreatorSimpleTrade _creatorSimpleTrade;
    private static CraftingItemWindow _instance;
    private RecipeItemMakeInfo _packet;
    private DataProviderCraftItem _dataProvider;
    public static CraftingItemWindow Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _creatorSimpleTrade = new CreatorSimpleTrade();
            _dataProvider = new DataProviderCraftItem();

        }
        else
        {
            Destroy(this);
        }
    }

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/Ñraft/CraftingItemWindow");
        _creatorSimpleTrade.LoadAsset(LoadAsset);

    }



    protected override IEnumerator BuildWindow(VisualElement root)
    {
        InitWindow(root);

        var dragArea = GetElementByClass("drag-area");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);


        VisualElement containerTradeSlots = GetElementById("BodyBox");
        _creatorSimpleTrade.CreateSlots(containerTradeSlots, L2Slot.SlotType.RecipeCraftItem , 18);

        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(_root);

        yield return new WaitForEndOfFrame();
    }

    public void AddData(RecipeItemMakeInfo packet)
    {
        if(packet != null)
        {
            _packet = packet;
            _creatorSimpleTrade.AddData(packet.RequiredItems , true);
            int itemId = _packet.RecipeData.ItemId;
            int count = _packet.RecipeData.Count;
            int rate = _packet.RecipeData.SuccessRate;
            int mpCost = _packet.RecipeData.MpCost;
            SetDataHeader(_windowEle , itemId , count , mpCost, rate);
        }
    }

    private void SetDataHeader(VisualElement container , int itemId , int count , int mpData , int successRate)
    {
        ItemInstance createItem = new ItemInstance(itemId, count, 0);
        _dataProvider.SetDataInfo(container , createItem, mpData , successRate);
    }
}
