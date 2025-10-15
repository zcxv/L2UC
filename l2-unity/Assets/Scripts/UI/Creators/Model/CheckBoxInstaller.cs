
using System.Collections.Generic;
using UnityEngine.UIElements;

public class CheckBoxRootElements
{
    private List<VisualElement> rootPanels;
    private List<List<SettingCheckBox>> _leftAll;
    private List<List<SettingCheckBox>> _rightAll;



    public CheckBoxRootElements(List<VisualElement> rootPanels, List<List<SettingCheckBox>> leftAll, List<List<SettingCheckBox>> rightAll)
    {
        this.rootPanels = rootPanels;
        this._leftAll = leftAll;
        this._rightAll = rightAll;
    }

  
    public List<VisualElement> GetRootPanels()
    {
        return rootPanels;
    }

    public List<SettingCheckBox> GetLeft(int index)
    {
        return _leftAll[index];
    }

    public List<SettingCheckBox> GetRight(int index)
    {
        return _rightAll[index];
    }

    public void Clear()
    {
        rootPanels = null;
        _leftAll = null;
        _rightAll = null;
    }
}