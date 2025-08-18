using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CreatorTabsWindows : AbstractCreator, ICreator
{
    private bool _useAllTabs;
    private ICreatorTables _creatorTable;
    private List<TableColumn> _dataColumn;
    public CreatorTabsWindows()
    {
        EventSwitchOut += OnSwitchEventOut;
    }
    public void AddData(List<ItemInstance> allItems)
    {
        throw new System.NotImplementedException();
    }

    public void AddOtherData(List<OtherModel> allItems)
    {
        throw new System.NotImplementedException();
    }

    public ItemInstance GetActiveByPosition(int position)
    {
        throw new System.NotImplementedException();
    }

    public void InsertTablesIntoContent(ICreatorTables creatorTable, List<TableColumn> dataColumn , bool useAllTabs)
    {
        VisualElement element = GetActiveContent();

        _useAllTabs = useAllTabs;
        _creatorTable = creatorTable;
        _dataColumn = dataColumn;

        if (element != null)
        {
            _creatorTable.InitTable(element);
            _creatorTable.CreateTable(dataColumn);
        }
        else
        {
            Debug.LogError("CreatorTabsWindows >>> InsertTablesIntoContent Not Found Content Element!!!");
        }

    }

    public void SetClickActiveTab(int position)
    {
        throw new System.NotImplementedException();
    }

    public void OnSwitchEventOut(ITab tab, bool isTrade)
    {
        if (_useAllTabs)
        {
            VisualElement element = GetActiveContent();

            if (element != null && !_creatorTable.HasTable(element))
            {
                _creatorTable.InitTable(element);
                _creatorTable.CreateTable(_dataColumn);
            }
            else if(element != null && _creatorTable.HasTable(element))
            {
                _creatorTable.UpdateTableData(_dataColumn);
            }
            else
            {
                Debug.LogError("CreatorTabsWindows >>> InsertTablesIntoContent Not Found Content Element!!!");
            }
        }

    }
}
