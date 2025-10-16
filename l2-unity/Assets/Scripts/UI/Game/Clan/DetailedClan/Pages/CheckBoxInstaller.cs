using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckBoxInstaller
{
    private static SettingCheckBox headerPrivileges = new SettingCheckBox("System Privileges", false, false);
    private static SettingCheckBox headerClanHall = new SettingCheckBox("Clan Hall Privileges", false, false);
    private static SettingCheckBox headerFortress  = new SettingCheckBox("Castle/Fortress", false, false);

    public static List<SettingCheckBox> InitChecBoxPrivilegesLeft(bool isChecked, bool isDisabled)
    {
        headerPrivileges.SetChecked(isChecked);
        headerPrivileges.SetDisabled(isDisabled);

        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            headerPrivileges,
            new SettingCheckBox("Invite", isChecked, isDisabled , ClanPrivileges.CP_CL_JOIN_CLAN , headerPrivileges),
            new SettingCheckBox("Manage Titles", isChecked, isDisabled , ClanPrivileges.CP_CL_GIVE_TITLE , headerPrivileges),
            new SettingCheckBox("Warehouse Search", isChecked, isDisabled , ClanPrivileges.CP_CL_VIEW_WAREHOUSE , headerPrivileges),
            new SettingCheckBox("Manage Ranks", isChecked, isDisabled , ClanPrivileges.CP_CL_MANAGE_RANKS, headerPrivileges),
            new SettingCheckBox("ClanWar", isChecked, isDisabled , ClanPrivileges.CP_CL_PLEDGE_WAR, headerPrivileges),

        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxPrivilegesRight(bool isChecked, bool isDisabled)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Dismiss", isChecked, isDisabled , ClanPrivileges.CP_CL_DISMISS , headerPrivileges),
            new SettingCheckBox("Edit Crest", isChecked, isDisabled , ClanPrivileges.CP_CL_REGISTER_CREST , headerPrivileges),
            new SettingCheckBox("Basic rights", isChecked, isDisabled , ClanPrivileges.CP_CL_MASTER_RIGHTS , headerPrivileges),
            new SettingCheckBox("Manage levels", isChecked, isDisabled , ClanPrivileges.CP_CL_MANAGE_LEVELS , headerPrivileges),
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxClanHallLeft(bool isChecked, bool isDisabled)
    {
        headerClanHall.SetChecked(isChecked);
        headerClanHall.SetDisabled(isDisabled);

        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            headerClanHall,
            new SettingCheckBox("Entry/Exit Rights", isChecked, isDisabled , ClanPrivileges.CP_CH_OPEN_DOOR , headerClanHall),
            new SettingCheckBox("Use Functions", isChecked, isDisabled, ClanPrivileges.CP_CH_USE_FUNCTIONS , headerClanHall),
            new SettingCheckBox("Auction", isChecked, isDisabled, ClanPrivileges.CP_CH_AUCTION , headerClanHall),
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxClanHallRight(bool isChecked, bool isDisabled)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Right to Dismiss", isChecked, isDisabled , ClanPrivileges.CP_CH_DISMISS, headerClanHall),
            new SettingCheckBox("Set Functions", isChecked, isDisabled , ClanPrivileges.CP_CH_SET_FUNCTIONS, headerClanHall)
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxCastleLeft(bool isChecked, bool isDisabled)
    {
        headerFortress.SetChecked(isChecked);
        headerFortress.SetDisabled(isDisabled);

        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            headerFortress,
            new SettingCheckBox("Entry/Exit Rights", isChecked, isDisabled, ClanPrivileges.CP_CS_OPEN_DOOR , headerFortress),
            new SettingCheckBox("Siege War", isChecked, isDisabled , ClanPrivileges.CP_CS_MANAGE_SIEGE, headerFortress),
            new SettingCheckBox("Use Functions", isChecked, isDisabled , ClanPrivileges.CP_CS_USE_FUNCTIONS, headerFortress),
            new SettingCheckBox("Set Functions", isChecked, isDisabled ,ClanPrivileges.CP_CS_SET_FUNCTIONS, headerFortress),
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxCastleRight(bool isChecked , bool isDisabled)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Right to Dismiss", isChecked, isDisabled , ClanPrivileges.CP_CS_DISMISS, headerFortress),
            new SettingCheckBox("Manage Taxes", isChecked, isDisabled , ClanPrivileges.CP_CS_TAXES, headerFortress),
            new SettingCheckBox("Mercenaries", isChecked, isDisabled , ClanPrivileges.CP_CS_MERCENARIES, headerFortress),
        };
        return list;
    }



    public static void UsePowerGrade1(ICreatorPanelCheckBox _createPanelCheckBox, List<List<SettingCheckBox>> _leftCheckBoxes, List<List<SettingCheckBox>> _rightCheckBoxes, VisualElement[] element)
    {
        CheckBoxRootElements elements = new CheckBoxRootElements(
          new List<VisualElement> { element[0], element[1], element[2] },
             CreateAllLeft1(_leftCheckBoxes , true , true),
             CreateAllRight1(_rightCheckBoxes, true, true)
         );
        _createPanelCheckBox.CreateTwoPanels(elements);
    }
    //panelPrivilages, VisualElement panelClanHall, VisualElement panelCastle
    public static void UsePowerGrade6(ICreatorPanelCheckBox _createPanelCheckBox , List<List<SettingCheckBox>> _leftCheckBoxes , List<List<SettingCheckBox>> _rightCheckBoxes, VisualElement[] element)
    {


        CheckBoxRootElements elements = new CheckBoxRootElements(
         new List<VisualElement> { element[0], element[1], element[2] },
             CreateAllLeft6(_leftCheckBoxes),
             CreateAllRight6(_rightCheckBoxes)
        );
        _createPanelCheckBox.CreateTwoPanels(elements);

    }

    public static void UsePowerGradeMinus1(ICreatorPanelCheckBox _createPanelCheckBox, List<List<SettingCheckBox>> _leftCheckBoxes, List<List<SettingCheckBox>> _rightCheckBoxes, VisualElement[] element)
    {
        bool isChecked = false;
        bool isDisabled = false;



        CheckBoxRootElements elements = new CheckBoxRootElements(
         new List<VisualElement> { element[0], element[1], element[2] },
             CreateAllLeftMinus1(_leftCheckBoxes , isChecked, isDisabled),
             CreateAllRightMinus1(_rightCheckBoxes)
        );
        _createPanelCheckBox.CreateTwoPanels(elements);

    }

    public static List<List<SettingCheckBox>> CreateAllLeft1( List<List<SettingCheckBox>> _leftCheckBoxes , bool isChecked , bool isDisabled)
    {


        _leftCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesLeft(isChecked, isDisabled),
            CheckBoxInstaller.InitChecBoxClanHallLeft(isChecked, isDisabled),
            CheckBoxInstaller.InitChecBoxCastleLeft(isChecked, isDisabled)
        });

        return _leftCheckBoxes;

    }

    public static List<List<SettingCheckBox>> CreateAllRight1(List<List<SettingCheckBox>> _rightCheckBoxes , bool isChecked, bool isDisabled)
    {
        _rightCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesRight(isChecked, isDisabled),
            CheckBoxInstaller.InitChecBoxClanHallRight(isChecked, isDisabled),
            CheckBoxInstaller.InitChecBoxCastleRight(isChecked, isDisabled)
        });

        return _rightCheckBoxes;
    }

    public static List<List<SettingCheckBox>> CreateAllLeft6(List<List<SettingCheckBox>> _leftCheckBoxes)
    {
        _leftCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesLeft(false, true),
            CheckBoxInstaller.InitChecBoxClanHallLeft(false, true),
            CheckBoxInstaller.InitChecBoxCastleLeft(false, true)
        });

        return _leftCheckBoxes;

    }

    private static List<List<SettingCheckBox>> CreateAllRight6(List<List<SettingCheckBox>> _rightCheckBoxes)
    {
        _rightCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesRight(false, true),
            CheckBoxInstaller.InitChecBoxClanHallRight(false, true),
            CheckBoxInstaller.InitChecBoxCastleRight(false , true)
        });

        return _rightCheckBoxes;
    }


    private static List<List<SettingCheckBox>> CreateAllLeftMinus1(List<List<SettingCheckBox>> _leftCheckBoxes , bool isChecked , bool isDisabled)
    {
        _leftCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesLeft(isChecked, isDisabled),
            CheckBoxInstaller.InitChecBoxClanHallLeft(isChecked, isDisabled),
            CheckBoxInstaller.InitChecBoxCastleLeft(isChecked, isDisabled)
        });

        return _leftCheckBoxes;

    }

    private static List<List<SettingCheckBox>> CreateAllRightMinus1(List<List<SettingCheckBox>> _rightCheckBoxes)
    {
        _rightCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesRight(false, false),
            CheckBoxInstaller.InitChecBoxClanHallRight(false, false),
            CheckBoxInstaller.InitChecBoxCastleRight(false , false)
        });

        return _rightCheckBoxes;
    }



}
