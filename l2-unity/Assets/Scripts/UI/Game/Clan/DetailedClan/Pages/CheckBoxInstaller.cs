using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckBoxInstaller
{

    public static List<SettingCheckBox> InitChecBoxPrivilegesLeft(bool isChecked, bool isDisabled)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("System Privileges", isChecked, isDisabled ),
            new SettingCheckBox("Invite", isChecked, isDisabled),
            new SettingCheckBox("Manage Titles", isChecked, isDisabled),
            new SettingCheckBox("Warehouse Search", isChecked, isDisabled),
            new SettingCheckBox("Manage Ranks", isChecked, isDisabled),
            new SettingCheckBox("ClanWar", isChecked, isDisabled),

        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxPrivilegesRight(bool isChecked, bool isDisabled)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Dismiss", isChecked, isDisabled),
            new SettingCheckBox("Edit Crest", isChecked, isDisabled)
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxClanHallLeft(bool isChecked, bool isDisabled)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Clan Hall Privileges", isChecked, isDisabled ),
            new SettingCheckBox("Entry/Exit Rights", isChecked, isDisabled),
            new SettingCheckBox("Use Functions", isChecked, isDisabled),
            new SettingCheckBox("Auction", isChecked, isDisabled),
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxClanHallRight(bool isChecked, bool isDisabled)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Right to Dismiss", isChecked, isDisabled),
            new SettingCheckBox("Set Functions", isChecked, isDisabled)
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxCastleLeft(bool isChecked, bool isDisabled)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Castle/Fortress", isChecked, isDisabled ),
            new SettingCheckBox("Entry/Exit Rights", isChecked, isDisabled),
            new SettingCheckBox("Siege War", isChecked, isDisabled),
            new SettingCheckBox("Use Functions", isChecked, isDisabled),
            new SettingCheckBox("Set Functions", isChecked, isDisabled),
        };
        return list;
    }

    public static List<SettingCheckBox> InitChecBoxCastleRight(bool isChecked , bool isDisabled)
    {
        List<SettingCheckBox> list = new List<SettingCheckBox>
        {
            new SettingCheckBox("Right to Dismiss", isChecked, isDisabled),
            new SettingCheckBox("Manage Taxes", isChecked, isDisabled),
             new SettingCheckBox("Mercenaries", isChecked, isDisabled),
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
        CheckBoxRootElements elements = new CheckBoxRootElements(
         new List<VisualElement> { element[0], element[1], element[2] },
             CreateAllLeftMinus1(_leftCheckBoxes),
             CreateAllRightMinus1(_rightCheckBoxes)
        );
        _createPanelCheckBox.CreateTwoPanels(elements);

    }

    public static List<List<SettingCheckBox>> CreateAllLeft1(List<List<SettingCheckBox>> _leftCheckBoxes , bool isChecked , bool isDisabled)
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


    private static List<List<SettingCheckBox>> CreateAllLeftMinus1(List<List<SettingCheckBox>> _leftCheckBoxes)
    {
        _leftCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesLeft(false, false),
            CheckBoxInstaller.InitChecBoxClanHallLeft(false, false),
            CheckBoxInstaller.InitChecBoxCastleLeft(false, false)
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
