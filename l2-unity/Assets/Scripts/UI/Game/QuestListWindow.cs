using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestListWindow : L2PopupWindow
{
    private static QuestListWindow _instance;
    private VisualElement _content;
    private VisualTreeAsset _templaTable;
    private VisualTreeAsset _templateHeader;
    private ICreatorTables _creatorTableWindows;
    public static QuestListWindow Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _creatorTableWindows = new CreatorTableWindows();
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/QuestListWindow");

    }


    protected override IEnumerator BuildWindow(VisualElement root)
    {


        InitWindow(root);


        yield return new WaitForEndOfFrame();

        var dragArea = GetElementByClass("drag-area");
        _content = GetElementByClass("quest-content");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);

        _creatorTableWindows.InitTable(_content , _windowEle);
        _creatorTableWindows.LoadAsset(LoadAsset);
        _creatorTableWindows.CreateTable(new List<TableColumn> { new TableColumn(false, "Mission Name", 0) , new TableColumn(false, "Conditions", 0), new TableColumn(false, "Level", 0), new TableColumn(false, "Repeatable", 0), new TableColumn(false, "Source", 0)});


        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(root);

    }


}

