using System;
using UnityEngine;
using UnityEngine.UIElements;
using static L2Slot;

public class CreatorSimpleTrade : AbstractTabCreator, ICreatorSimpleTrade
{
    private TradeTab _tradeTab;
    private const string containerTabName = "Data/UI/_Elements/Template/SimpleTab/TabContainer";
    private VisualElement elementContainerTab;
    //240px
    private int _defaultMaximumWidthPx = 240;
    public void CreateSlots(VisualElement container, SlotType slotType, bool isDragged = false)
    {
        elementContainerTab = container;
        container.Add(GetTemplateById(0));

        if(elementContainerTab != null)
        {
            CreateSimpleTradeTab(container, slotType, isDragged);
        }
        else
        {
             Debug.LogError("CreatorSimpleTrade>CreateSlots : Element Container Tab is null");
        }

    }

    public void SetContent(int idTemplate)
    {
        throw new NotImplementedException();
    }

    public void LoadAsset(Func<string, VisualTreeAsset> loaderFunc)
    {
        base.LoadAsset(loaderFunc, new string[1] { containerTabName });
    }
}
