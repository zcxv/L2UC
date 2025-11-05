using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class RecipeBookWindow : L2PopupWindow
{
    private VisualTreeAsset _tabTemplate;
    private VisualTreeAsset _tabHeaderTemplate;
    private VisualElement _inventoryTabView;

    private static RecipeBookWindow _instance;
    private RecipeBookItemList _packet;
    public static RecipeBookWindow Instance { get { return _instance; } }
    private ICreatorTradeTab _creatorWindow;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _creatorWindow = new CreatorTradingWindows();

        }
        else
        {
            Destroy(this);
        }
    }

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/Ñraft/RecipeBookWindow");
        _creatorWindow.LoadAsset(LoadAsset);
    }

    protected override IEnumerator BuildWindow(VisualElement root)
    {
        InitWindow(root);

        var dragArea = GetElementByClass("drag-area");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);

        _creatorWindow.InitTradeTabs(new string[] { "ALL" });
        _creatorWindow.CreateTradeTabs(GetElementById("InventoryTabView") , L2Slot.SlotType.Recipe , true);


        Button trashBtn = (Button)GetElementById("TrashBtn");
        trashBtn.AddManipulator(new ButtonClickSoundManipulator(trashBtn));
        trashBtn.AddManipulator(new TooltipManipulator(trashBtn, "Trash"));
        L2Slot trashSlot = new L2Slot(trashBtn, 100, L2Slot.SlotType.Trash);

        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(_root);

        yield return new WaitForEndOfFrame();
    }

    public void AddData(RecipeBookItemList packet)
    {
        if(packet.ListRecipes != null)
        {
            _packet = packet;
            _creatorWindow.AddData(packet.ListItemInstance);
        }
    }

    public int GetRecipeIdByPosition(int position)
    {
        return _packet.ListRecipes.FirstOrDefault(x => x.GetPosition() == position + 1)?.GetIDMK() ?? 0;
    }
}
