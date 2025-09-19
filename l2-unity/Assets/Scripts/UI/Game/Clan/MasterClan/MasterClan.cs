using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MasterClan : MonoBehaviour
{
    private DropdownField _dropDown;

    public void ForEachClan( ICreatorTables _creatorTableWindows)
    {


        var namesList = new List<string>();
        var conditionsList = new List<string>();
        var levelList = new List<string>();
        var repeatableList = new List<string>();


        for(int i=0; i < 7; i++)
        { 
            namesList.Add("");
            conditionsList.Add("");
            levelList.Add("");
            repeatableList.Add("");
        }


        namesList[0] = "hector2";
        conditionsList[0] = "11";
        levelList[0] = "2";
        repeatableList[0] = "0";

        var name = new TableColumn(false, "Name", 0, namesList, 13);
        var lvl = new TableColumn(true, "Lv.", 0, conditionsList, 0);
        var role = new TableColumn(true, "Role", 0, levelList, 0);
        var act = new TableColumn(true, "Activity.", 0, repeatableList, 0);

        List<TableColumn> listTableColumn = new List<TableColumn> { name, lvl, role, act };
        _creatorTableWindows.CreateTable(listTableColumn);


        //_creatorTableWindows.CreateTable(new List<TableColumn> { new TableColumn(false, "Mission Name", 13 ,  new List<string> { "Letters of Love" , "What Women Want", "Will the Seal Be Broken" } , 13) ,
        //  new TableColumn(false, "Conditions", 0, new List<string> { "No Requirements" , "Elf,Human", "Dark Elf" } ,13),
        //  new TableColumn(true, "Level", 0, new List<string> { "2-5" , "2-5" , "16-26" } , 0),
        //  new TableColumn(true, "Repeatable", 0 , new List<string> { "1"  , "1"  , "1" } , 0),
        //  new TableColumn(false, "Source", 0 , new List<string> { "Darin" , "Arujien", "Talloth" }, 18)});
    }




   // public void AddList(Dictionary<string, int> players)
    //{
    //    _dropdown.value = "";
    //    var list = players.Keys.ToList();
     //   _dropdown.choices = list;
     //   _players = players;
    //}

}
