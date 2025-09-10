using UnityEngine;

public class QuestInstance
{
    private int _questID;
    private int _flags;

    public int QuestID => _questID;
    public int Status => _flags;

    public QuestInstance(int qId, int flags)
    {
        _questID = qId;
        _flags = flags;
    }

    public string QuestName()
    {
        QuestName questNames = QuestNameTable.Instance.GetQuestName(_questID, _flags);
        if (questNames != null)
            return questNames.Main_name;
        return "Unknown";
    }

    public string QuestProgName()
    {
        QuestName questNames = QuestNameTable.Instance.GetQuestName(_questID, _flags);
        if (questNames != null)
            return questNames.ProgName;
        return "Unknown";
    }

    public string GetQuestSource()
    {
        QuestName questNames = QuestNameTable.Instance.GetQuestName(_questID, _flags);
        if (questNames != null)
            return questNames.GetSource();
        return "Unknown";
    }

    public string GetQuestEntity()
    {
        QuestName questNames = QuestNameTable.Instance.GetQuestName(_questID, _flags);
        if (questNames != null)
            return questNames.EntityName;
        return "Unknown";
    }
}