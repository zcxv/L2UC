using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using static UnityEngine.Rendering.DebugUI.Table;

public class CreatorTableWindows : ICreatorTables
{
    private VisualElement _content;
    private const string _templateNameTable = "Data/UI/_Elements/Template/TableList";
    private const string _templateHeaderItem = "Data/UI/_Elements/Template/TableHeaderItem";
    private const string _templateColumnInside = "Data/UI/_Elements/Template/TableHeaderName";
    private const string _templateRowsName = "Data/UI/_Elements/Template/TableRows";
    private const string _templateRowBlackName = "Data/UI/_Elements/Template/TableRowBlack";
    private const string _templateRowWhiteName = "Data/UI/_Elements/Template/TableRowWhite";

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

    private float width;
    private float height;


    private VisualElement _table;
    private List<VisualElement> _tableHeHeaderItem;
    private VisualElement containerHeader;
    public void InitTable(VisualElement content, VisualElement windowEle)
    {
        _content = content;
        //root.RegisterCallback<GeometryChangedEvent>(OnGeometryCallback);
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

    public void CreateTable(List<TableColumn> headersName)
    {
        if(_templateTable == null || _templateHeader == null)
        {
            Debug.LogError("CreatorTableWindows not found TemplateTable or TemplateHeader");
            return;
        }

         _table = ToolTipsUtils.CloneOne(_templateTable);
        VisualElement containerBodyList = _table.Q<VisualElement>(_tableBodyListName);
        _tableHeHeaderItem = CreateHeaderItems(headersName , containerBodyList);
         VisualElement containerHeaderList = _table.Q<VisualElement>(_tableHeaderListName);



        InjectHeaderList(_tableHeHeaderItem, containerHeaderList);



        //InjectBodyList(headersName , containerBodyList);

        _content.Add(_table);
    }


    private void InjectHeaderList(List<VisualElement> _tableHeHeaderItem , VisualElement containerHeaderList)
    {
        foreach (var header in _tableHeHeaderItem)
        {
            containerHeaderList.Add(header);
        }
    }



    public List<VisualElement> CreateHeaderItems(List<TableColumn> headersName , VisualElement containerBodyList)
    {
        if (_templateHeader == null)
        {
            Debug.LogError("CreatorTableWindows not found TemplateHeader");
            return new List<VisualElement>();
        }
        //Create Header Box (Only BorderBox)
        containerHeader = ToolTipsUtils.CloneOne(_templateHeader);
        var headerBorder = containerHeader.Q<VisualElement>(_tableHeaderBorder);

        headerBorder.RegisterCallback<GeometryChangedEvent>(evt => OnHeaderGeometryChanged(evt , headerBorder , containerBodyList));

        return headersName.Select(headerName =>
        {
            //Create Items Inside Header
            var rootContainerHeader = ToolTipsUtils.CloneOne(_templateInside);
            Label tableHeaderLabel = rootContainerHeader.Q<Label>(_tableHeaderLabelName);
            VisualElement containerBgElement = rootContainerHeader.Q<VisualElement>("bgElement");
            //VisualElement containerListRows = rootContainerHeader.Q<VisualElement>("columnInsideListRows");

            CreateHeader(tableHeaderLabel, containerBgElement, headerName);
            CreateListRows(containerBodyList, headerName);

            headerBorder.Add(rootContainerHeader);

            return containerHeader;
        }).ToList();
    }


    void OnHeaderGeometryChanged(GeometryChangedEvent evt , VisualElement containerHeader , VisualElement containerBodyList)
    {
        for (int i=0; i < containerHeader.childCount; i++)
        {
            var headerItem = containerHeader[i];
            var bodyItem = containerBodyList[i];

            float width = headerItem.layout.width;
            bodyItem.style.width = width;

            Debug.Log($"Layout iteration Width: {width}, Height: {height} " + "container body list " + containerBodyList.childCount);
        }
    }
    private void CreateHeader(Label tableHeaderLabel , VisualElement bgElement , TableColumn headerName)
    {
        bgElement.RegisterCallback<MouseDownEvent>(evt => ClickMouse(evt, bgElement));
        bgElement.RegisterCallback<MouseUpEvent>(evt => UpClickMouse(evt, bgElement));
        tableHeaderLabel.text = headerName.NameColumn;
    }

    private void CreateListRows(VisualElement insideListRows , TableColumn headerName)
    {
        var listRows = ToolTipsUtils.CloneOne(_templateRows);
        List<VisualElement> rows =  CreateRows(headerName);

        AddRowInListRows(listRows, rows);
        AlignItem(listRows, headerName);
        insideListRows.Add(listRows);
    }

    private void AddRowInListRows(VisualElement listRows , List<VisualElement> rows)
    {
        foreach (var row in rows)
        {
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
                row.style.paddingLeft = headerName.LeftIndent;
            }
        }
    }

    public List<VisualElement> CreateRows(TableColumn headerName)
    {
        List < VisualElement > rows = new List<VisualElement>();

        for (int i = 0; i < headerName.ListData.Count; i++)
        {
            VisualElement row;
            string text = headerName.ListData[i];
            row = GetNewRow( i);
            Label labelText = row.Q<Label>("labelText");

            if (labelText !=  null)
            {
                labelText.text = text;
            }

            rows.Add(row);
        }

        return rows;
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

    public void UpClickMouse(MouseUpEvent evt , VisualElement element)
    {
        element.RemoveFromClassList("l2-table-header-name-bg-click");
        element.AddToClassList("l2-table-header-name-bg");
    }


}

public class TableColumn
{
    public bool _alignTextCenter;
    public string _nameColumn;
    public float _manualWidth;
    public List<string> _listData;
    public float _leftIndent;

    public TableColumn(bool alignTextCenter, string nameColumn, float width , List<string> listData , float leftIndent)
    {
        _alignTextCenter = alignTextCenter;
        _nameColumn = nameColumn;
        _manualWidth = width;
        _listData = listData;
        _leftIndent = leftIndent;

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

