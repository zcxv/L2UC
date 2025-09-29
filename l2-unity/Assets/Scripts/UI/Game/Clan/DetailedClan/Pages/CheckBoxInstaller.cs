using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CheckBoxInstaller
{

    public static List<SettingCheckBox> InitChecBoxPrivilegesLeft(bool isChecked)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("System Privileges", isChecked, true ),
            new SettingCheckBox("Invite", isChecked, true),
            new SettingCheckBox("Manage Titles", isChecked, true),
            new SettingCheckBox("Warehouse Search", isChecked, true),
            new SettingCheckBox("Manage Ranks", isChecked, true),
            new SettingCheckBox("ClanWar", isChecked, true),

        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxPrivilegesRight(bool isChecked)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Dismiss", isChecked, true),
            new SettingCheckBox("Edit Crest", isChecked, true)
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxClanHallLeft(bool isChecked)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Clan Hall Privileges", isChecked, true ),
            new SettingCheckBox("Entry/Exit Rights", isChecked, true),
            new SettingCheckBox("Use Functions", isChecked, true),
            new SettingCheckBox("Auction", isChecked, true),
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxClanHallRight(bool isChecked)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Right to Dismiss", isChecked, true),
            new SettingCheckBox("Set Functions", isChecked, true)
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxCastleLeft(bool isChecked)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Castle/Fortress", isChecked, true ),
            new SettingCheckBox("Entry/Exit Rights", isChecked, true),
            new SettingCheckBox("Siege War", isChecked, true),
            new SettingCheckBox("Use Functions", isChecked, true),
            new SettingCheckBox("Set Functions", isChecked, true),
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxCastleRight(bool isChecked)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Right to Dismiss", isChecked, true),
            new SettingCheckBox("Manage Taxes", isChecked, true),
             new SettingCheckBox("Mercenaries", isChecked, true),
        };
        return list;
    }


}
