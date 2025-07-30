using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestNameTable
{
    private static QuestNameTable _instance;
    private static Dictionary<int, List<QuestName>> _questNames;

    private readonly int indexQuestId = 1;
    private readonly int indexQuestProg = 2;
    private readonly int indexMainName = 3;
    private readonly int indexProgname = 4;
    private readonly int indexDescription = 5;
    private readonly int indexDescriptionToolTips = 53;
    private readonly int indexQuestType = 6;

    private readonly int indexLvlMin = 39;
    private readonly int indexLvlMax = 40;
    private readonly int indexRequirements = 50;

    private readonly int indexEntity_name = 42;
    //maybe not true
    private readonly int indexRepaet= 41;
    private readonly int indexAreaId = 131;
    public static QuestNameTable Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new QuestNameTable();
                _questNames = new Dictionary<int, List<QuestName>>();
            }

            return _instance;
        }
    }

    

    public void Initialize()
    {
        ReadArmorInterlude();
        DebugPrint();
    }

    public void ReadArmorInterlude()
    {
        string dataPath = Path.Combine(Application.streamingAssetsPath, "Data/Meta/Questname-e_interlude.txt");

        using (StreamReader reader = new StreamReader(dataPath))
        {
            string line;
            int index = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (index != 0)
                {
                    string[] ids = line.Split('\t');
                    int id = Int32.Parse(ids[indexQuestId]);
                    QuestName quest =  CreateModel(ids);

                    if(id == 257)
                    {
                        Debug.Log("");
                    }

                    if (!_questNames.ContainsKey(id))
                    {
                        var  list = new List<QuestName>{quest};
                        _questNames.Add(id , list);
                    }
                    else
                    {
                        _questNames[id].Add(quest);
                    }
                }
                index++;
            }

        }

    }

    private QuestName CreateModel(string[] ids)
    {
        QuestName quest = new QuestName();

        int questProg = int.Parse(ids[indexQuestProg]);
        string mainName = DatUtils.CleanupStringOldData(ids[indexMainName]);
        string progName = DatUtils.CleanupStringOldData(ids[indexProgname]);
        string description = DatUtils.CleanupStringOldData(ids[indexDescription]);
        string descriptionToolTips = DatUtils.CleanupStringOldData(ids[indexDescriptionToolTips]);
        int questType = int.Parse(ids[indexQuestType]);
        int lvlMin = int.Parse(ids[indexLvlMin]);
        int lvlMax = int.Parse(ids[indexLvlMax]);
        string requirements = DatUtils.CleanupStringOldData(ids[indexRequirements]);
        string entity_name = DatUtils.CleanupStringOldData(ids[indexEntity_name]);
        int repeat = int.Parse(ids[indexRepaet]);
        int areaId = int.Parse(ids[indexAreaId]);


        quest.QuestProg = questProg;
        quest.Main_name = mainName;
        quest.ProgName = progName;
        quest.Description = description;
        quest.DescriptionToolTip = descriptionToolTips;
        quest.QuestType = questType;
        quest.LvlMin = lvlMin;
        quest.LvlMax = lvlMax;
        quest.Requirements = requirements;
        quest.EntityName = entity_name;
        quest.Repeat = repeat;
        quest.AreaId = areaId;

        return quest;
    }

    private void DebugPrint()
    {
        foreach (KeyValuePair<int, List<QuestName>> entry in _questNames)
        {
            foreach(QuestName name in entry.Value)
            {
                //EntityName берем последнего из списка он и будет  лиентом в форме или источником
                Debug.Log(" ID Ќомер quest " + entry.Key + " value " + name.Main_name + " entityName " + name.EntityName + " questType " + name.Repeat);
            }
            
        }
    }
}
