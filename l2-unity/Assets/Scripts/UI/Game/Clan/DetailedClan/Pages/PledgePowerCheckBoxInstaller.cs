using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PledgePowerCheckBoxInstaller 
{

    public static void CreateCheckboxUsePowerRanked(int powerRanked , ModelPowerCheckBox model)
    {
        if(powerRanked == 0) UsePowerGradeDefault(model);
        if(powerRanked == ClanPrivileges.CP_ALL) UsePowerGradeCpAll(model);
        int[] splitData = ByteUtils.SplitMask(powerRanked);
        UsePowerNotAll(splitData, model);
    }

    public static void UsePowerGradeCpAll(ModelPowerCheckBox model)
    {
        CheckBoxRootElements elements = new CheckBoxRootElements(
          new List<VisualElement> { model.Element[0], model.Element[1], model.Element[2] },
             CheckBoxInstaller.CreateAllLeft1(model.LeftCheckBoxes, true, false),
             CheckBoxInstaller.CreateAllRight1(model.RightCheckBoxes, true, false)
         );
        model.CreatePanelCheckBox.CreateTwoPanels(elements);
    }

    public static void UsePowerGradeDefault(ModelPowerCheckBox model)
    {
        CheckBoxRootElements elements = new CheckBoxRootElements(
          new List<VisualElement> { model.Element[0], model.Element[1], model.Element[2] },
             CheckBoxInstaller.CreateAllLeft1(model.LeftCheckBoxes, false, false),
             CheckBoxInstaller.CreateAllRight1(model.RightCheckBoxes, false, false)
         );
        model.CreatePanelCheckBox.CreateTwoPanels(elements);
    }

    public static void UsePowerNotAll(int[] splitData, ModelPowerCheckBox model)
    {
        var (left, right) = PrepareCheckBoxLists(model);
        ApplyPrivileges(splitData, left, right);
        CreateAndDisplayPanels(model, left, right);
    }

    private static (List<List<SettingCheckBox>> left, List<List<SettingCheckBox>> right) PrepareCheckBoxLists(ModelPowerCheckBox model)
    {
        List<List<SettingCheckBox>> left = CheckBoxInstaller.CreateAllLeft1(model.LeftCheckBoxes, false, false);
        List<List<SettingCheckBox>> right = CheckBoxInstaller.CreateAllRight1(model.RightCheckBoxes, false, false);
        return (left, right);
    }

    private static void ApplyPrivileges(int[] splitData, List<List<SettingCheckBox>> left, List<List<SettingCheckBox>> right)
    {
        foreach (int privilege in splitData)
        {
            if (!TrySetPrivilege(left, privilege))
            {
                TrySetPrivilege(right, privilege);
            }
        }
    }

    private static bool TrySetPrivilege(List<List<SettingCheckBox>> checkBoxLists, int privilege)
    {
        foreach (var checkBoxList in checkBoxLists)
        {
            var matchingCheckBox = checkBoxList.FirstOrDefault(cb => cb.GetData() == privilege);
            if (matchingCheckBox != null)
            {
                matchingCheckBox.SetChecked(true);
                return true;
            }
        }
        return false;
    }

    private static void CreateAndDisplayPanels(ModelPowerCheckBox model, List<List<SettingCheckBox>> left, List<List<SettingCheckBox>> right)
    {
        CheckBoxRootElements elements = new CheckBoxRootElements(
            new List<VisualElement> { model.Element[0], model.Element[1], model.Element[2] },
            left,
            right
        );

        model.CreatePanelCheckBox.CreateTwoPanels(elements);
    }


}


