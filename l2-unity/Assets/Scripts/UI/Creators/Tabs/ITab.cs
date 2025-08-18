using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface ITab
{
    void UnselectTabContainerClass();
    void SelectTabContainerClass();
    void SetMainTab(bool main);

    public event Action<ITab , bool> EventSwitch;
    public event Action<int, ItemCategory, int> EventLeftClick;
    void OnClickLeftEvent(int position);

    void AddDataTrade(List<ItemInstance> allItems);
    ItemInstance GetSlotByPosition(int position);

    public VisualElement GetContentElement();
}
