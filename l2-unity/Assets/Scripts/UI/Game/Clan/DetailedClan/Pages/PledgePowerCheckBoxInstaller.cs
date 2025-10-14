using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PledgePowerCheckBoxInstaller 
{

    public static void CreateCheckboxByPowerRanked(int powerRanked , ModelPowerCheckBox model)
    {
        switch (powerRanked)
        {
            case ClanPrivileges.CP_ALL:
                UsePowerGradeCpAll(model);
                break;
            default:
                UsePowerGradeDefault(model);
                break;
        }
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


}


