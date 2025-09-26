using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;


public class CreatePanelCheckBoxWindows : ICreatorPanelCheckBox
{
    private List<SettingCheckBox> _left;
    private List<SettingCheckBox> _right;
    private const string _checkBoxTemplateName = "Data/UI/_Elements/Template/Elements/Checkbox/DefaultCheckBox";
    private const string _twoPanelsTemplateName = "Data/UI/_Elements/Template/Clan/TwoPanelsCheckBox";
    private List<VisualTreeAsset> _listTemplate  = new List<VisualTreeAsset>();
    private  string DEFAULT_DISABLED_COLOR = "#7f7f7f";
    private Color _colorTextDisabled;

    public CreatePanelCheckBoxWindows()
    {
 
        ColorUtility.TryParseHtmlString(DEFAULT_DISABLED_COLOR, out _colorTextDisabled);
    }
    public void LoadAsset(Func<string, VisualTreeAsset> loaderFunc)
    {
        foreach (string templateName in GetTemplateElementName())
        {
            _listTemplate.Add(loaderFunc(templateName));
        }
    }

    private List<string> GetTemplateElementName()
    {
        return new List<string> { _twoPanelsTemplateName, _checkBoxTemplateName };
    }

    public void CreateTwoPanels(VisualElement rootPanel, List<SettingCheckBox> left, List<SettingCheckBox> right)
    {
        _left = left;
        _right = right;

        VisualElement twoPanel = AddTwoPanels(rootPanel);
        VisualTreeAsset checkBoxTemplate = _listTemplate[1];
        SetCheckBoxHeaderPanel(twoPanel, _listTemplate[1] , left[0]);
        left.RemoveAt(0);
        SetCheckBoxLeftPanel(twoPanel, checkBoxTemplate , left);
        SetCheckBoxRightPanel(twoPanel, checkBoxTemplate , right);
    }
    private VisualElement AddTwoPanels(VisualElement panel)
    {


        if(panel != null)
        {
            VisualElement twoPanels = CreateTwoPanels(_listTemplate[0]);
            panel.Add(twoPanels);
            return twoPanels;
        }
        else
        {
            Debug.LogError("CreatePanelCheckBoxWindows-> Not Found Panel");
            return null;
        }

    }

    private void SetCheckBoxHeaderPanel(VisualElement panel, VisualTreeAsset checkboxTemplate , SettingCheckBox setting)
    {
        VisualElement checkbox = CreateCheckBox(checkboxTemplate);
        VisualElement header = panel.Q<VisualElement>("GroupHeaderTwoPanels");
        SetSetting(checkbox, setting);
        header.Add(checkbox);
    }

    private void SetCheckBoxLeftPanel(VisualElement panel, VisualTreeAsset checkboxTemplate, List<SettingCheckBox> listSetting)
    {
        VisualElement subLeftPanel = panel.Q<VisualElement>("GroupLeftListCheckBox");
        AutoCreateCheckBox(subLeftPanel, listSetting, checkboxTemplate);
    }

    public void SetCheckBoxRightPanel(VisualElement panel, VisualTreeAsset checkboxTemplate, List<SettingCheckBox> listSetting)
    {
        VisualElement subRightPanel = panel.Q<VisualElement>("GroupRightListCheckBox");
        AutoCreateCheckBox(subRightPanel, listSetting, checkboxTemplate);
    }


    public VisualElement CreateTwoPanels(VisualTreeAsset template)
    {
        return ToolTipsUtils.CloneOne(template);
    }

    public VisualElement CreateCheckBox(VisualTreeAsset template)
    {
        return ToolTipsUtils.CloneOne(template);
    }

    private void AutoCreateCheckBox(VisualElement panel, List<SettingCheckBox> listSettings, VisualTreeAsset checkboxTemplate)
    {
        for (int i = 0; i < listSettings.Count; i++)
        {
            SettingCheckBox setting = listSettings[i];
            VisualElement checkbox = CreateCheckBox(checkboxTemplate);
            SetSetting(checkbox , setting);
            panel.Add(checkbox);
        }
    }

    private VisualElement SetSetting(VisualElement checkbox , SettingCheckBox setting)
    {
        if (checkbox == null) return null;

        VisualElement uncheked = checkbox.Q<VisualElement>("imageUnchecked");
        VisualElement cheked = checkbox.Q<VisualElement>("imageChecked");
        VisualElement checkedDisabled = checkbox.Q<VisualElement>("imageCheckedDisabled");
        Label label = checkbox.Q<Label>("labelCheckBox");

        SetChecked(checkbox, setting.IsChecked() , uncheked, cheked);
        SetDisabled(checkbox, setting.IsDisabled(), uncheked, cheked, checkedDisabled);
        SetText(label, setting.GetName());
        SetColorText(label, setting.IsDisabled());

        return checkbox;

    }

    private void SetChecked(VisualElement checkbox, bool isChecked , VisualElement uncheked , VisualElement cheked)
    {

        if (isChecked)
        {
            uncheked.style.display = DisplayStyle.None;
            cheked.style.display = DisplayStyle.Flex;
        }
        else
        {
            cheked.style.display = DisplayStyle.None;
            uncheked.style.display = DisplayStyle.Flex;
        }
    }

    private void SetText(Label label, string name )
    {
        label.text = name;
    }

    private void SetColorText(Label label , bool isDisabled)
    {
        if (isDisabled)
        {
            label.style.color = _colorTextDisabled;
        }
        else
        {
            label.style.color = Color.white;
        }
    }

    private void SetDisabled(VisualElement checkbox, bool isDisabled, VisualElement uncheked, VisualElement cheked , VisualElement  checkedDisabled)
    {


        if (isDisabled)
        {
            uncheked.style.display = DisplayStyle.None;
            cheked.style.display = DisplayStyle.None;
            checkedDisabled.style.display = DisplayStyle.Flex;
        }
    }
}
