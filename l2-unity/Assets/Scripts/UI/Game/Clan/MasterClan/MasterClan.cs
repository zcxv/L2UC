using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MasterClan : MonoBehaviour
{

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


        //namesList[0] = "hector2";
        //conditionsList[0] = "11";
        //levelList[0] = "Data/UI/Clan/Role_create";
       // activity[0] = "Data/UI/Clan/Clan_sword_online";

        //namesList[1] = "gawric";
        //conditionsList[1] = "3";
        //levelList[1] = "Data/UI/Clan/Role_create";
        //activity[1] = "Data/UI/Clan/Clan_sword_offline";


        //namesList[2] = "party";
        //conditionsList[2] = "6";
        //levelList[2] = "Data/UI/Clan/Role_create";
        //activity[2] = "Data/UI/Clan/Clan_sword_offline";

        var name = new TableColumn(false, "Name", 100, namesList, 13);
        var lvl = new TableColumn(true, "Lv.", 0, conditionsList, 0 );
        var role = new TableColumn(true, "Role", 15, levelList, 0 , true);
        var act = new TableColumn(true, "Activity.", 20, activity, 0 , true);

        List<TableColumn> listTableColumn = new List<TableColumn> { name, lvl, role, act };
        _creatorTableWindows.CreateTable(listTableColumn);

    }





}
