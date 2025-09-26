using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CheckBoxInstaller : MonoBehaviour
{

    public static List<SettingCheckBox> InitChecBoxPrivilegesLeft()
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("System Privileges", true, true ),
            new SettingCheckBox("Invite", true, true),
            new SettingCheckBox("Manage Titles", true, true),
            new SettingCheckBox("Warehouse Search", true, true),
            new SettingCheckBox("Manage Ranks", true, true),
            new SettingCheckBox("ClanWar", true, true),

        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxPrivilegesRight()
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Dismiss", true, true),
            new SettingCheckBox("Edit Crest", true, true)
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxClanHallLeft()
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Clan Hall Privileges", true, true ),
            new SettingCheckBox("Entry/Exit Rights", true, true),
            new SettingCheckBox("Use Functions", true, true),
            new SettingCheckBox("Auction", true, true),
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxClanHallRight()
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Right to Dismiss", true, true),
            new SettingCheckBox("Set Functions", true, true)
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxCastleLeft()
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Castle/Fortress", true, true ),
            new SettingCheckBox("Entry/Exit Rights", true, true),
            new SettingCheckBox("Siege War", true, true),
            new SettingCheckBox("Use Functions", true, true),
            new SettingCheckBox("Set Functions", true, true),
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxCastleRight()
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Right to Dismiss", true, true),
            new SettingCheckBox("Manage Taxes", true, true),
             new SettingCheckBox("Mercenaries", true, true),
        };
        return list;
    }


}
