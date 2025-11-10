using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


public class CraftingItemWindow : L2PopupWindow
{
    private ICreatorSimpleTrade _creatorSimpleTrade;
    private static CraftingItemWindow _instance;
    public static CraftingItemWindow Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _creatorSimpleTrade = new CreatorSimpleTrade();

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
        _creatorSimpleTrade.CreateSlots(containerTradeSlots, L2Slot.SlotType.RecipeCraftItem);

        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(_root);

        yield return new WaitForEndOfFrame();
    }
}
