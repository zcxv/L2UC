using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PledgePowerCheckBoxInstaller 
{

    public static void CreateCheckboxUsePowerGrade(int powerGrade, ModelPowerCheckBox model, bool defaultChecked, bool defaultDisabled)
    {
        if (powerGrade == 0) UsePowerGradeDefault(model , false , true);
        if (powerGrade == ClanPrivileges.CP_ALL) UsePowerGradeCpAll(model , true , true);
        int[] splitData = ByteUtils.SplitMask(powerGrade);
        UseGradeNotAll(splitData, model, defaultChecked, defaultDisabled);
    }

    public static void CreateCheckboxUsePowerRanked(int powerRanked , ModelPowerCheckBox model , bool defaultChecked, bool defaultDisabled)
    {
        if(powerRanked == 0) UsePowerGradeDefault(model, false, false);
        if (powerRanked == ClanPrivileges.CP_ALL) UsePowerGradeCpAll(model , true , false);

        if(powerRanked != 0)
        {
            int[] splitData = ByteUtils.SplitMask(powerRanked);
            UsePowerNotAll(splitData, model, defaultChecked, defaultDisabled);
        }

    }

    public static void UsePowerGradeCpAll(ModelPowerCheckBox model, bool defaultChecked, bool defaultDisabled)
    {
        CheckBoxRootElements elements = new CheckBoxRootElements(
          new List<VisualElement> { model.Element[0], model.Element[1], model.Element[2] },
             CheckBoxInstaller.CreateAllLeft1(model.LeftCheckBoxes, defaultChecked, defaultDisabled),
             CheckBoxInstaller.CreateAllRight1(model.RightCheckBoxes, defaultChecked, defaultDisabled)
         );
        model.CreatePanelCheckBox.CreateTwoPanels(elements);
    }



    public static void UsePowerGradeDefault(ModelPowerCheckBox model ,  bool defaultChecked, bool defaultDisabled)
    {
        CheckBoxRootElements elements = new CheckBoxRootElements(
          new List<VisualElement> { model.Element[0], model.Element[1], model.Element[2] },
             CheckBoxInstaller.CreateAllLeft1(model.LeftCheckBoxes, defaultChecked, defaultDisabled),
             CheckBoxInstaller.CreateAllRight1(model.RightCheckBoxes, defaultChecked, defaultDisabled)
         );
        model.CreatePanelCheckBox.CreateTwoPanels(elements);
    }

    public static void UsePowerNotAll(int[] splitData, ModelPowerCheckBox model, bool defaultChecked, bool defaultDisabled)
    {
        var (left, right) = PrepareCheckBoxLists(model , false , false);
        ApplyPrivileges(splitData, left, right , defaultChecked , defaultDisabled);
        CreateAndDisplayPanels(model, left, right);
    }

    public static void UseGradeNotAll(int[] splitData, ModelPowerCheckBox model, bool defaultChecked, bool defaultDisabled)
    {
        var (left, right) = PrepareCheckBoxLists(model, false, true);
        ApplyPrivileges(splitData, left, right, defaultChecked, defaultDisabled);
        CreateAndDisplayPanels(model, left, right);
    }

    private static (List<List<SettingCheckBox>> left, List<List<SettingCheckBox>> right) PrepareCheckBoxLists(ModelPowerCheckBox model , bool defaultChecked, bool defaultDisabled)
    {
        List<List<SettingCheckBox>> left = CheckBoxInstaller.CreateAllLeft1(model.LeftCheckBoxes, defaultChecked, defaultDisabled);
        List<List<SettingCheckBox>> right = CheckBoxInstaller.CreateAllRight1(model.RightCheckBoxes, defaultChecked, defaultDisabled);
        return (left, right);
    }



    private static void ApplyPrivileges(int[] splitData, List<List<SettingCheckBox>> left, List<List<SettingCheckBox>> right, bool defaultChecked, bool defaultDisabled)
    {
        foreach (int privilege in splitData)
        {
            if (!TrySetPrivilege(left, privilege , defaultChecked , defaultDisabled))
            {
                TrySetPrivilege(right, privilege, defaultChecked, defaultDisabled);
            }
        }
    }

    private static bool TrySetPrivilege(List<List<SettingCheckBox>> checkBoxLists, int privilege , bool defaultChecked , bool defaultDisabled)
    {
        foreach (var checkBoxList in checkBoxLists)
        {
            var matchingCheckBox = checkBoxList.FirstOrDefault(cb => cb.GetData() == privilege);
            if (matchingCheckBox != null)
            {
                var header = matchingCheckBox.GetMyHeaderSetting();
                header?.SetChecked(defaultChecked);
                header?.SetDisabled(defaultDisabled);

                matchingCheckBox.SetChecked(true);
                matchingCheckBox?.SetChecked(defaultChecked);
                matchingCheckBox?.SetDisabled(defaultDisabled);

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


