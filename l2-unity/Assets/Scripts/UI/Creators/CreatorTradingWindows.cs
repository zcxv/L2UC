using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CreatorTradingWindows : AbstractCreator , ICreator
{

    public void SetClickActiveTab(int position)
    {
        if(_activeTab != null)
        {
            _activeTab.OnClickLeftEvent(position);
        }
    }

    public TradeTab GetMainTab()
    {
        foreach (TradeTab tab in _tabsTradeTabs)
        {
            if(tab != null & tab.GetMainTab() == true)
            {
                return tab;
            }
        }

        return null;
    }


    public void AddData(List<ItemInstance> allItems)
    {
        List<ItemInstance> allItemsInstance =  allItems.OfType<ItemInstance>().ToList();

        if (_activeTab != null)
        {
            _activeTab.AddDataTrade(allItemsInstance);
        }
    }

    public ItemInstance GetActiveByPosition(int position)
    {
        if(_activeTab != null)
        {
           return _activeTab.GetSlotByPosition(position);
        }

        return null;
    }


    public void AddOtherData(List<OtherModel> allItems)
    {
        throw new NotImplementedException();
    }
    //not use in TradeWindow
    public void CreateTabs(VisualElement _inventoryTabView, VisualTreeAsset _tabTemplate, VisualTreeAsset _tabHeaderTemplate)
    {
        throw new NotImplementedException();
    }
    //not use in TradeWindow
    public void InsertTablesIntoContent(ICreatorTables creatorTable, List<TableColumn> dataColumn, bool useAllTabs)
    {
        throw new NotImplementedException();
    }
}
