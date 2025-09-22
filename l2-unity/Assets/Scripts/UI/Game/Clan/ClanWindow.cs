using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClanWindow : L2PopupWindow
{
   
    private static ClanWindow _instance;
    private MasterClan _masterClan;
    private ICreatorTables _creatorTableWindows;

    private DropdownField _dropdown;
    private string _selectDropDown = "";
    private List<string> _listDropDown;
    public static ClanWindow Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _creatorTableWindows = new CreatorTableWindows();
            _masterClan = new MasterClan();
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/Clan/ClanWindow");
    }

    protected override IEnumerator BuildWindow(VisualElement root)
    {


        InitWindow(root);


        yield return new WaitForEndOfFrame();

        var dragArea = GetElementByClass("drag-area");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);


        _dropdown = _windowEle.Q<DropdownField>("comboBox");
        DisableEventOnOver(_dropdown);
        _dropdown.RegisterValueChangedCallback(OnDropdownValueChanged);
        _dropdown.RegisterCallback<PointerDownEvent>(OnDropdownPointer, TrickleDown.TrickleDown);
        //_dropdown.value = "";
        //_dropdown.choices = new List<string> { "123","321","e43" };
        //_listDropDown = new List<string> { "123", "321", "e43" };

        var master_table_content = GetElementByClass("master-table-list");
        _creatorTableWindows.InitTable(master_table_content);
        _creatorTableWindows.LoadAsset(LoadAsset);
        _masterClan.ForEachClan(_creatorTableWindows);

        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(_root);

        OnCenterScreen(root);
    }

    public void AddClanData()
    {

    }

    private void OnDropdownPointer(PointerDownEvent evt)
    {
        if (_listDropDown == null || _listDropDown.Count == 0)
        {
            evt.PreventDefault();
            evt.StopImmediatePropagation();
        }
    }

    private void OnDropdownValueChanged(ChangeEvent<string> evt)
    {
        //string playeName = evt.newValue;
        //_selectDropDown = playeName;
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}
