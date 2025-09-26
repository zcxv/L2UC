using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface ICreatorPanelCheckBox
{
    void LoadAsset(Func<string, VisualTreeAsset> loaderFunc);
    public void CreateTwoPanels(VisualElement rootPanel , List<SettingCheckBox> left , List<SettingCheckBox> right);

    
}
