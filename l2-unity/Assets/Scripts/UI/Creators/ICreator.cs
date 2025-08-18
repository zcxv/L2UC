using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;

public interface ICreator
{
    //public void InitTabs(string[] nameTabs);

    public void InitTradeTabs(string[] nameTabs);

    public void InitContentTabs(string[] nameTabs);

    public void CreateTabs(VisualElement _inventoryTabView, VisualTreeAsset _tabTemplate, VisualTreeAsset _tabHeaderTemplate);

    public void CreateTradeTabs(VisualElement _inventoryTabView, VisualTreeAsset _tabTemplate, VisualTreeAsset _tabHeaderTemplate);

    public void AddData(List<ItemInstance> allItems);

    public void AddOtherData(List<OtherModel> allItems);

    public event Action<int , ItemCategory , int> EventLeftClick;

    public void SetClickActiveTab(int position);

    public ItemInstance GetActiveByPosition(int position);

    public void InsertTablesIntoContent(ICreatorTables creatorTable , List<TableColumn> dataColumn, bool useAllTabs);


}
