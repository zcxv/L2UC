

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.UI.VirtualMouseInput;



public class CreatorTableWindows : ICreatorTables
{
    private VisualElement _content;
    private const string _templateNameTable = "Data/UI/_Elements/Template/TableList";
    private const string _templateHeaderItem = "Data/UI/_Elements/Template/TableHeaderItem";
    private const string _templateColumnInside = "Data/UI/_Elements/Template/TableHeaderName";
    private const string _templateRowsName = "Data/UI/_Elements/Template/TableRows";
    private const string _templateRowBlackName = "Data/UI/_Elements/Template/TableRowBlack";
    private const string _templateRowWhiteName = "Data/UI/_Elements/Template/TableRowWhite";


    private const string _tableListViewName = "table_listView";
    private const string _tableBodyListName = "table_body";
    private const string _tableHeaderListName = "table_header";
    private const string _tableHeaderLabelName = "columnLabel";
    private const string _tableHeaderBorder = "columnBorder";


    private VisualTreeAsset _templateTable;
    private VisualTreeAsset _templateHeader;
    private VisualTreeAsset _templateInside;
    private VisualTreeAsset _templateRows;

    private VisualTreeAsset _templateRowBlack;
    private VisualTreeAsset _templateRowWhite;

    private int _lastElementIndex = -1;

    private VisualElement _table;
    private List<VisualElement> _tableHeHeaderItem;
    private VisualElement containerHeader;
    private List<VisualElement> childrenList;
    private ListView _listView;
    private ScrollView _innerScrollView;
    private List<TableColumn> _columnsList;
    private Texture2D _defaultCursor;
    public void InitTable(VisualElement content)
    {
        _content = content;
        _defaultCursor = IconManager.Instance.LoadCursorByName("Default");
    }


    public void LoadAsset(Func<string, VisualTreeAsset> loaderFunc)
    {
        _templateTable = loaderFunc(_templateNameTable);
        _templateHeader = loaderFunc(_templateHeaderItem);
        _templateInside = loaderFunc(_templateColumnInside);
        _templateRows = loaderFunc(_templateRowsName);
        _templateRowBlack = loaderFunc(_templateRowBlackName);
        _templateRowWhite = loaderFunc(_templateRowWhiteName);
    }



    public void CreateTable(List<TableColumn> columnsList)
    {
        if(_templateTable == null || _templateHeader == null)
        {
            Debug.LogError("CreatorTableWindows not found TemplateTable or TemplateHeader");
            return;
        }

         _table = ToolTipsUtils.CloneOne(_templateTable);

        _listView = _table.Q<ListView>(_tableListViewName);
        _listView.virtualizationMethod = CollectionVirtualizationMethod.FixedHeight;
        _listView.fixedItemHeight = 18;
        _columnsList = columnsList;

        _innerScrollView = _listView.Q<ScrollView>();


        _listView.itemsSource = _columnsList[0].ListData.Select((_, index) =>
        _columnsList.Select(col => col.ListData[index]).ToArray()).ToList();

        _listView.makeItem = MakeItem;
        _listView.bindItem = BindListViewItems;


        _tableHeHeaderItem = CreateHeaderItems(columnsList);
         VisualElement containerHeaderList = _table.Q<VisualElement>(_tableHeaderListName);
        InjectHeaderList(_tableHeHeaderItem, containerHeaderList);
        _content.Add(_table);
    }

    public void ReCreateTable(List<TableColumn> headersName)
    {
        //DestroyTable();
        //CreateTable(headersName);
    }
   
    private void InjectHeaderList(List<VisualElement> _tableHeHeaderItem , VisualElement containerHeaderList)
    {
        foreach (var header in _tableHeHeaderItem)
        {
            containerHeaderList.Add(header);
        }
    }

    public VisualElement MakeItem()
    {

            VisualElement tablesContainer = new VisualElement();

            tablesContainer.style.flexDirection = FlexDirection.Row;
            tablesContainer.style.flexGrow = 1;
            tablesContainer.style.flexShrink = 1;

            for (int i=0; i < _columnsList.Count; i++)
            {
                var listRows = ToolTipsUtils.CloneOne(_templateRows);
                VisualElement element = CreateRow("");
                listRows.Add(element);
                tablesContainer.Add(listRows);


                listRows.RegisterCallback<MouseDownEvent>(evt => UnityEngine.Cursor.SetCursor(_defaultCursor, Vector2.zero, UnityEngine.CursorMode.Auto));
                listRows.RegisterCallback<MouseUpEvent>(evt => UnityEngine.Cursor.SetCursor(_defaultCursor, Vector2.zero, UnityEngine.CursorMode.Auto));
                listRows.RegisterCallback<PointerMoveEvent>(evt => UnityEngine.Cursor.SetCursor(_defaultCursor, Vector2.zero, UnityEngine.CursorMode.Auto));
                listRows.RegisterCallback<PointerDownEvent>(evt => UnityEngine.Cursor.SetCursor(_defaultCursor, Vector2.zero, UnityEngine.CursorMode.Auto));
            }



        return tablesContainer;

   }

  private void BindListViewItems(VisualElement ve , int i)
    {

            var row = (VisualElement)ve;

            for (int n =0; n < _columnsList.Count; n++)
            {
                var column = _columnsList[n];
                VisualElement row_list1 = row[n];
                VisualElement in_1 = row_list1[0];
                //TableRowBlack - inside 0 > content_highlight & 1 > label_text
                Label label = (Label)in_1[1];
                label.text = column.ListData[i];
            }

    }

   

    public List<VisualElement> CreateHeaderItems(List<TableColumn> tablesColumns)
    {
        if (_templateHeader == null)
        {
            Debug.LogError("CreatorTableWindows not found TemplateHeader");
            return new List<VisualElement>();
        }
        //Create Header Box (Only BorderBox)
        containerHeader = ToolTipsUtils.CloneOne(_templateHeader);
        var headerBorder = containerHeader.Q<VisualElement>(_tableHeaderBorder);

        headerBorder.RegisterCallback<GeometryChangedEvent>(evt => OnHeaderGeometryChanged(evt , headerBorder , _innerScrollView));

        return tablesColumns.Select(headerName =>
        {
            //Create Items Inside Header
            var rootContainerHeader = ToolTipsUtils.CloneOne(_templateInside);
            Label tableHeaderLabel = rootContainerHeader.Q<Label>(_tableHeaderLabelName);
            VisualElement containerBgElement = rootContainerHeader.Q<VisualElement>("bgElement");

            SetManualWidth(containerBgElement, headerName.ManualWidth);
            CreateHeader(tableHeaderLabel, containerBgElement, headerName);
            headerBorder.Add(rootContainerHeader);

            return containerHeader;
        }).ToList();
    }

    public void SetManualWidth(VisualElement containerBgElement, float width)
    {
        if (width > 0)
        {
            containerBgElement.style.flexBasis = width;    
            containerBgElement.style.flexGrow = 1;       
            containerBgElement.style.flexShrink = 0;
        }
    }



    void OnHeaderGeometryChanged(GeometryChangedEvent evt, VisualElement containerHeader , ScrollView innerScrollView)
    {
        EditorApplication.delayCall += () =>
        {
            bool isFirst = true;
            float[] allWith = new float[containerHeader.childCount];

            for (int i = 0; i < containerHeader.childCount; i++)
            {
                var headerItem = containerHeader[i];


                float headerWidth = headerItem.resolvedStyle.width;

                float newWidth = isFirst ? headerWidth + 2 : headerWidth;
                isFirst = false;
                allWith[i] = newWidth;

                 Debug.Log("OnHeaderGeometryChanged > " + headerWidth + " Name header " + headerItem.name);
            }


            if (innerScrollView != null)
            {
                var innerContainer = innerScrollView.contentContainer;
                Debug.Log("Inner children: " + innerContainer.childCount);
                ChangeWidthListView(innerContainer.Children(), allWith);
            }
        };
    }

    private void ChangeWidthListView(IEnumerable<VisualElement> elementsListView ,  float[] allWith)
    {
        int index = 0;

        foreach (var item in elementsListView)
        {
  
            var row = item;
            var columns = row.Children().ToList(); 

            for(int i = 0; i < allWith.Length; i++)
            {
                var column = columns[i];
                var new_width = allWith[i];

                column.style.flexBasis = new_width;
                column.style.flexShrink = 0;
                column.MarkDirtyRepaint();
            }
        }
    }


    public void UpdateTableData(List<TableColumn> headersName)
    {
        if (_table == null)
        {
            Debug.LogError("Table is not created. Call CreateTable first.");
            return;
        }


        VisualElement containerBodyList = _table.Q<VisualElement>(_tableBodyListName);





        //RemoveFromHierarchy(containerBodyList);


        int index = 0;
        foreach (var headerName in headersName)
        {
            var column = containerBodyList.Q<VisualElement>("body_column_" + index);
            if (column != null)
            {
                IEnumerable<VisualElement> childrenRows = column.Children();
                List<VisualElement> childrenList = childrenRows.ToList();


                foreach (var row in childrenList)
                {
                    row.RemoveFromHierarchy();
                }


                List<VisualElement> rows = CreateRows(headerName);
                AddRowInListRows(column, rows);
                AlignItem(column, headerName);
            }
            index++;
        }

        _table.MarkDirtyRepaint();
    }

    // Add this method to clear the table completely if needed
    public void DestroyTable()
    {
        if (_table != null)
        {
            _content.Remove(_table);
            _table = null;
            _tableHeHeaderItem = null;
            containerHeader = null;
            _lastElementIndex = -1;
        }
    }

    public void ClearTable()
    {
        if (_table == null)
        {
            return;
        }

        VisualElement containerBodyList = _table.Q<VisualElement>(_tableBodyListName);
        RemoveFromHierarchy(containerBodyList);
    }

    public bool HasTable(VisualElement element)
    {
        if (element == null)
            return false;

      
        bool hasTableBody = element.Q<VisualElement>("table_body") != null;
        bool hasTableHeader = element.Q<VisualElement>("table_header") != null;

        bool hasTableClasses = element.ClassListContains("l2-table") ||
                              element.ClassListContains("table-container");

        return hasTableBody && hasTableHeader || hasTableClasses;
    }

    private void RemoveFromHierarchy(VisualElement containerBodyList)
    {
        foreach (var column in containerBodyList.Children().ToList())
        {

            foreach (var row in column.Children().ToList())
            {
                row.RemoveFromHierarchy();
            }
        }
    }


    private void CreateHeader(Label tableHeaderLabel , VisualElement bgElement , TableColumn headerName)
    {
        bgElement.RegisterCallback<MouseDownEvent>(evt => ClickMouse(evt, bgElement));
        bgElement.RegisterCallback<MouseUpEvent>(evt => UpClickMouse(evt, bgElement));
        tableHeaderLabel.text = headerName.NameColumn;
    }

    private void CreateListRows(VisualElement insideListRows , TableColumn headerName , ref int index)
    {
        var listRows = ToolTipsUtils.CloneOne(_templateRows);
        List<VisualElement> rows =  CreateRows(headerName);
        listRows.name = "body_column_" + index++;
        AddRowInListRows(listRows, rows);
        AlignItem(listRows, headerName);
        insideListRows.Add(listRows);
    }

    private void AddRowInListRows(VisualElement listRows , List<VisualElement> rows)
    {
        for (int i =0; i < rows.Count; i++)
        {
            var row = rows[i];
            int index = i;
            row.RegisterCallback<MouseDownEvent>(evt => ClickRow(evt , index));
            listRows.Add(row);
        }

    }

    private void AlignItem(VisualElement listRows, TableColumn headerName)
    {
        var rows = listRows.Children().ToList();
        for (int i = 0; i < rows.Count; i++)
        {
            var row = rows[i];

            if (headerName.AlignTextCenter)
            {
                row.style.alignItems = new StyleEnum<Align>(Align.Center);
            }
            else
            {
                var label = row.Q<Label>("labelText");
                label.style.paddingLeft = headerName.LeftIndent;
            }

            if(headerName.MaxTextSize != 0)
            {
                var label = row.Q<Label>("labelText");
                MaxLabelSize(label, headerName.MaxTextSize);
            }
        }
    }

    public void MaxLabelSize(Label label , int maxSize)
    {
        label.style.maxWidth = maxSize;
    }

    public VisualElement CreateRow(string text)
    {
        VisualElement row;
        row = GetNewRow(1);
        Label labelText = row.Q<Label>("labelText");
        CleapText(labelText);

        if (labelText != null)
        {
            labelText.text = text;
        }

        return row;
    }

    public List<VisualElement> CreateRows(TableColumn column)
    {
        List < VisualElement > rows = new List<VisualElement>();

        for (int i = 0; i < column.ListData.Count; i++)
        {
            VisualElement row;
            string text = column.ListData[i];
            row = GetNewRow( i);
            Label labelText = row.Q<Label>("labelText");
            CleapText(labelText);

            if (labelText !=  null)
            {
                labelText.text = text;
            }

            rows.Add(row);
        }

        return rows;
    }

    private void CleapText(Label rowLabel)
    {
        //rowLabel.style.width = 200;                   // фикс. ширина
        rowLabel.style.whiteSpace = WhiteSpace.NoWrap; // запретить перенос
        rowLabel.style.textOverflow = TextOverflow.Clip; // обрезать
    }

    private VisualElement GetNewRow(int i)
    {
        if (i % 2 == 0)
        {
            return  ToolTipsUtils.CloneOne(_templateRowWhite);
        }
        else
        {
            return  ToolTipsUtils.CloneOne(_templateRowBlack);
        }

    }
    public void ClickMouse(MouseDownEvent evt , VisualElement element)
    {
        element.RemoveFromClassList("l2-table-header-name-bg");
        element.AddToClassList("l2-table-header-name-bg-click");
    }

    public void ClickRow(MouseDownEvent evt , int index)
    {
        if(evt.target != null)
        {
            VisualElement row = evt.target as VisualElement;
            VisualElement hihtlite = row.parent;
            VisualElement rowContainer = hihtlite.parent;
            var _selectListColumn = rowContainer.parent;
           
            SetSelectElement(_selectListColumn, index);
            _lastElementIndex = index;
        }
    }

    private void SetSelectElement(VisualElement listColumn, int index)
    {
        for (int i = 0; i < listColumn.childCount; i++)
        {
            VisualElement column = listColumn[i];

    

            if (column.childCount > 0)
            {
                IEnumerable<VisualElement> childrenRows = column.Children();
                List<VisualElement> childrenList = childrenRows.ToList();

                if (_lastElementIndex != -1)
                {
                    VisualElement rowLast = childrenList[_lastElementIndex];
                    ResetSelect(rowLast , index);
                }


                VisualElement row1 = childrenList[index];

                ShowSelectHighLight(row1, listColumn.childCount - 1, i);
            }
        }
    }

    public void ResetSelectList(VisualElement listColumn, int index)
    {

        for (int i = 0; i < listColumn.childCount; i++)
        {
            VisualElement column = listColumn[i];

            if (column.childCount > 0)
            {
                IEnumerable<VisualElement> childrenRows = column.Children();
                List<VisualElement> childrenList = childrenRows.ToList();

                if (_lastElementIndex != -1)
                {
                    VisualElement rowLast = childrenList[_lastElementIndex];
                    ResetSelect(rowLast, index);
                }

            }
        }
    }
    private void ResetSelect(VisualElement rowLast , int index)
    {
        VisualElement highlightLast = rowLast.Q<VisualElement>("highlight");
        VisualElement highlightLastTile = rowLast.Q<VisualElement>("highlightTile");

        if (highlightLast != null) highlightLast.style.display = DisplayStyle.None;
        if (highlightLastTile != null) highlightLastTile.style.display = DisplayStyle.None;

    }
    private void ShowSelectHighLight(VisualElement row1 , int childCount , int i)
    {
       // row1.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0));
        VisualElement highlight = row1.Q<VisualElement>(i == childCount ? "highlightTile" : "highlight");
        highlight.style.display = DisplayStyle.Flex;
    }

    public void UpClickMouse(MouseUpEvent evt , VisualElement element)
    {
        element.RemoveFromClassList("l2-table-header-name-bg-click");
        element.AddToClassList("l2-table-header-name-bg");
    }

    public void SetSelectRow(int rowId)
    {
        if(rowId == -1)
        {
            //if(_selectListColumn != null)
            //{
             //   ResetSelectList(_selectListColumn, rowId);
             //   _lastElementIndex = rowId;
            //}

        }
    }
}



public class TableColumn
{
    public bool _alignTextCenter;
    public string _nameColumn;
    public float _manualWidth;
    public List<string> _listData;
    public float _leftIndent;
    public int _maxTextSize;

    public TableColumn(bool alignTextCenter, string nameColumn, float width , List<string> listData , float leftIndent)
    {
        _alignTextCenter = alignTextCenter;
        _nameColumn = nameColumn;
        _manualWidth = width;
        _listData = listData;
        _leftIndent = leftIndent;
        _maxTextSize = 0;
    }

    public TableColumn(bool alignTextCenter, string nameColumn, float width, List<string> listData, float leftIndent , int maxTextSize)
    {
        _alignTextCenter = alignTextCenter;
        _nameColumn = nameColumn;
        _manualWidth = width;
        _listData = listData;
        _leftIndent = leftIndent;
        _maxTextSize = maxTextSize;
    }


    public void SetData(List<string> listData)
    {
        if(_listData != null)
        {
            _listData.Clear();
            _listData = null;
        }
        _listData = listData;
    }
    public bool AlignTextCenter
    {
        get { return _alignTextCenter; }
        set { _alignTextCenter = value; }
    }

    public string NameColumn
    {
        get { return _nameColumn; }
        set { _nameColumn = value; }
    }

    public float ManualWidth
    {
        get { return _manualWidth; }
        set { _manualWidth = value; }
    }

    public int MaxTextSize
    {
        get { return _maxTextSize; }
        set { _maxTextSize = value; }
    }

    public List<string> ListData
    {
        get { return _listData; }
        set { _listData = value; }
    }

    public float LeftIndent
    {
        get { return _leftIndent; }
        set { _leftIndent = value; }
    }



}

