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

public class CreatorTableWindows : ICreatorTables
{
    private VisualElement _content;
    private const string _templateNameTable = "Data/UI/_Elements/Template/TableList";
    private const string _templateHeaderItem = "Data/UI/_Elements/Template/TableHeaderItem";
    private const string _templateColumnInside = "Data/UI/_Elements/Template/TableHeaderName";
    private const string _templateRowsName = "Data/UI/_Elements/Template/TableRows";

    private const string _tableBodyListName = "table_body";
    private const string _tableHeaderListName = "table_header";
    private const string _tableHeaderLabelName = "columnLabel";
    private const string _tableHeaderBorder = "columnBorder";


    private VisualTreeAsset _templateTable;
    private VisualTreeAsset _templateHeader;
    private VisualTreeAsset _templateInside;
    private VisualTreeAsset _templateRows;
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



    private void DistributeHeaderItems(List<VisualElement> headerItems, float totalWidth)
    {
        if (headerItems == null || headerItems.Count == 0)
        {
            Debug.LogWarning("Header items list is empty or null");
            return;
        }

        // Calculate width for each item (total width divided by number of items)
        float itemWidth = totalWidth / headerItems.Count;

        // Apply calculated width to each item
        foreach (var item in headerItems)
        {
            item.style.width = itemWidth;
        }
    }

    public void LoadAsset(Func<string, VisualTreeAsset> loaderFunc)
    {
        _templateTable = loaderFunc(_templateNameTable);
        _templateHeader = loaderFunc(_templateHeaderItem);
        _templateInside = loaderFunc(_templateColumnInside);
        _templateRows = loaderFunc(_templateRowsName);
    }

    public void CreateTable(List<TableColumn> headersName)
    {
        if(_templateTable == null || _templateHeader == null)
        {
            Debug.LogError("CreatorTableWindows not found TemplateTable or TemplateHeader");
            return;
        }

         _table = ToolTipsUtils.CloneOne(_templateTable);
        _tableHeHeaderItem = CreateHeaderItems(headersName);
         VisualElement containerHeaderList = _table.Q<VisualElement>(_tableHeaderListName);
         VisualElement containerBodyList = _table.Q<VisualElement>(_tableBodyListName);


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



    public List<VisualElement> CreateHeaderItems(List<TableColumn> headersName)
    {
        if (_templateHeader == null)
        {
            Debug.LogError("CreatorTableWindows not found TemplateHeader");
            return new List<VisualElement>();
        }
        //Create Header Box (Only BorderBox)
        containerHeader = ToolTipsUtils.CloneOne(_templateHeader);
        var headerBorder = containerHeader.Q<VisualElement>(_tableHeaderBorder);

        return headersName.Select(headerName =>
        {
            //Create Items Inside Header
            var rootContainerHeader = ToolTipsUtils.CloneOne(_templateInside);
            Label tableHeaderLabel = rootContainerHeader.Q<Label>(_tableHeaderLabelName);
            VisualElement containerbgElement = rootContainerHeader.Q<VisualElement>("bgElement");
            VisualElement containerListRows = rootContainerHeader.Q<VisualElement>("columnInsideListRows");

            CreateHeader(tableHeaderLabel, containerbgElement, headerName);
            CreateListRows(containerListRows , headerName);

            headerBorder.Add(rootContainerHeader);
            return containerHeader;
        }).ToList();
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
        AlignItem(listRows, headerName);
        insideListRows.Add(listRows);
    }

    private void AlignItem(VisualElement listRows, TableColumn headerName)
    {
        foreach (VisualElement row in listRows.Children())
        {
            if (headerName.AlignTextCenter)
            {
                row.style.alignItems = new StyleEnum<Align>(Align.Center);
            }
            else
            {
                row.style.paddingLeft = 13;
            }
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
    public bool AlignTextCenter;
    public string NameColumn;
    public float manualWidth;

    public TableColumn(bool alignTextCenter, string nameColumn, float width)
    {
        AlignTextCenter = alignTextCenter;
        NameColumn = nameColumn;
        manualWidth = width;
    }
}

