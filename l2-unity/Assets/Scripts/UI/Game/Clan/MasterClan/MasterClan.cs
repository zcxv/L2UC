using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MasterClan : MonoBehaviour
{
    private const string ROLE_CREATE = "Data/UI/Clan/Role_create";
    private const string MEMBER_ONLINE = "Data/UI/Clan/Clan_sword_online";
    private const string MEMBER_OFFLINE = "Data/UI/Clan/Clan_sword_offline";
    public void ForEachClan( ICreatorTables _creatorTableWindows)
    {


        var namesList = new List<string>();
        var conditionsList = new List<string>();
        var levelList = new List<string>();
        var activity = new List<string>();


        for(int i=0; i < 7; i++)
        { 
            namesList.Add("");
            conditionsList.Add("");
            levelList.Add("");
            activity.Add("");
        }

        List<TableColumn> listTableColumn = GetColumns(namesList, conditionsList, levelList, activity);
        _creatorTableWindows.CreateTable(listTableColumn);

    }

    public List<string> SetDropdownList(DropdownField dropdown , string clanName)
    {
        if (string.IsNullOrEmpty(clanName))
        {
            dropdown.value = null;
            dropdown.choices = null;
            return null;
        }
        else
        {
            var _listDropDown = new List<string> { "Main Clan - " + clanName };
            dropdown.value = "Main Clan - " + clanName;
            dropdown.choices = _listDropDown;
            return _listDropDown;
        }

    }

    public void CreateMembersTable(List<ClanMember> listMemebers, ICreatorTables _creatorTableWindows)
    {

        var namesList = new List<string>();
        var lvlList = new List<string>();
        var roleList = new List<string>();
        var activity = new List<string>();


        PopulateMemberData(listMemebers, namesList, lvlList, roleList, activity);
        EnsureMinimumRowCount(namesList, lvlList, roleList, activity, 7);


        var tableColumns = GetColumns(namesList, lvlList, roleList, activity);
        SetOfflineMemberColors(listMemebers, tableColumns);

        _creatorTableWindows.UpdateTableData(tableColumns);
    }


    private void PopulateMemberData(List<ClanMember> members, List<string> names,
    List<string> levels, List<string> roles, List<string> activities)
    {
        foreach (var member in members)
        {
            names.Add(member.MemberName);
            levels.Add(member.Level.ToString());
            roles.Add(ROLE_CREATE);
            SetOnline(member, activities);
        }
    }

    private void EnsureMinimumRowCount(List<string> names, List<string> levels,
    List<string> roles, List<string> activities, int minimumCount)
    {
        int currentCount = names.Count;
        if (currentCount >= minimumCount) return;

        int emptySlots = minimumCount - currentCount;
        for (int i = 0; i < emptySlots; i++)
        {
            names.Add("");
            levels.Add("");
            roles.Add("");
            activities.Add("");
        }
    }

    private void SetOfflineMemberColors(List<ClanMember> members, List<TableColumn> columns)
    {
        foreach (var member in members)
        {
            if (member.Online == 0)
            {
                columns[0].SetColor(member.MemberName, "#B3B2B2");
                columns[1].SetColor(member.Level.ToString(), "#B3B2B2");
            }
        }
    }

    private void SetOnline(ClanMember member, List<string> activity)
    {
        if (member.Online == 0)
        {
            activity.Add(MEMBER_OFFLINE);

        }
        else
        {
            activity.Add(MEMBER_ONLINE);
        }
    }




    private List<TableColumn> GetColumns(List<string> namesList , List<string> lvlList, List<string> roleList, List<string> activity)
    {
        var name = new TableColumn(false, "Name", 100, namesList, 13);
        var lvl = new TableColumn(true, "Lv.", 0, lvlList, 0);
        var role = new TableColumn(true, "Role", 15, roleList, 0, true);
        var act = new TableColumn(true, "Activity.", 20, activity, 0, true);

        return new List<TableColumn> { name, lvl, role, act };
    }
}
