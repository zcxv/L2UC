using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;
using static UnityEngine.Rendering.DebugUI;

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

    public void InitTable(VisualElement content)
    {
       _content = content;
    }

    public void LoadAsset(Func<string, VisualTreeAsset> loaderFunc)
    {
        _templateTable = loaderFunc(_templateNameTable);
        _templateHeader = loaderFunc(_templateHeaderItem);
        _templateInside = loaderFunc(_templateColumnInside);
        _templateRows = loaderFunc(_templateRowsName);
    }

    public void CreateTable(List<string>headersName)
    {
        if(_templateTable == null || _templateHeader == null)
        {
            Debug.LogError("CreatorTableWindows not found TemplateTable or TemplateHeader");
            return;
        }

         var _table = ToolTipsUtils.CloneOne(_templateTable);
         var _tableHeHeaderItem = CreateHeaderItems(headersName);
         VisualElement containerHeaderList = _table.Q<VisualElement>(_tableHeaderListName);
         VisualElement containerBodyList = _table.Q<VisualElement>(_tableBodyListName);


        InjectHeaderList(_tableHeHeaderItem, containerHeaderList);

        var listRows = ToolTipsUtils.CloneOne(_templateRows);
        InjectBodyList(containerBodyList, listRows);

        _content.Add(_table);
    }


    private void InjectHeaderList(List<VisualElement> _tableHeHeaderItem , VisualElement containerHeaderList)
    {
        foreach (var header in _tableHeHeaderItem)
        {
            containerHeaderList.Add(header);
        }
    }

    private void InjectBodyList(VisualElement containerBodyList , VisualElement litRows)
    {
        containerBodyList.Add(litRows);
    }

    public List<VisualElement> CreateHeaderItems(List<string> headersName)
    {
        if (_templateHeader == null)
        {
            Debug.LogError("CreatorTableWindows not found TemplateHeader");
            return new List<VisualElement>();
        }
        //Create Header Box (Only BorderBox)
        var headerItem = ToolTipsUtils.CloneOne(_templateHeader);
        var headerBorder = headerItem.Q<VisualElement>(_tableHeaderBorder);
        return headersName.Select(headerName =>
        {
            //Create Items Inside Header
            var inside = ToolTipsUtils.CloneOne(_templateInside);
            Label tableHeaderLabel = inside.Q<Label>(_tableHeaderLabelName);
            VisualElement bgElement = inside.Q<VisualElement>("bgElement");
            bgElement.RegisterCallback<MouseDownEvent>(evt=> ClickMouse(evt , bgElement));
            bgElement.RegisterCallback<MouseUpEvent>(evt =>  UpClickMouse(evt, bgElement));
            tableHeaderLabel.text = headerName;
            headerBorder.Add(inside);
            return headerItem;
        }).ToList();
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
