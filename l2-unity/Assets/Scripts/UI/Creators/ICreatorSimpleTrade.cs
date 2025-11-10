using System;
using UnityEngine;
using UnityEngine.UIElements;
using static L2Slot;

public interface ICreatorSimpleTrade
{
    public void CreateSlots(VisualElement container, SlotType slotType, bool isDragged = false);
    public void SetContent(int idTemplate);
    public void LoadAsset(Func<string, VisualTreeAsset> loaderFunc);
}
