using System.Collections.Generic;
using UnityEngine;

public class SettingCheckBox 
{
    private bool _checked = false;
    private bool _disabled = false;
    private string _name;
 
    public SettingCheckBox(string name, bool isChecked = false, bool isDisabled = false)
    {
        _name = name;
        _checked = isChecked;
        _disabled = isDisabled;
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
