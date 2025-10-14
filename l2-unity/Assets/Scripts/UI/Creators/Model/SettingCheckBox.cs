using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class SettingCheckBox 
{
    private bool _checked = false;
    private bool _disabled = false;
    private string _name;
    private VisualElement _element;
    private VisualElement _elementUnchecked;
    private VisualElement _elementChecked;
    public SettingCheckBox(string name, bool isChecked = false, bool isDisabled = false)
    {
        _name = name;
        _checked = isChecked;
        _disabled = isDisabled;
    }

    public void SetElement(VisualElement element)
    {
        _element = element;
    }

    public VisualElement GetElement()
    {
        return _element;
    }
    public void SetElementChecked(VisualElement element)
    {
        _elementChecked = element;
    }

    public VisualElement GetElementChecked()
    {
        return _elementChecked;
    }

    public void SetElementUnChecked(VisualElement element)
    {
        _elementUnchecked = element;
    }

    public VisualElement GetElementUnChecked()
    {
        return _elementUnchecked;
    }
    public void SetChecked(bool click_checked)
    {
        _checked = click_checked;
    }
    // Getters
    public bool IsChecked()
    {
        return _checked;
    }

    public bool IsDisabled()
    {
        return _disabled;
    }

    public string GetName()
    {
        return _name;
    }
}
