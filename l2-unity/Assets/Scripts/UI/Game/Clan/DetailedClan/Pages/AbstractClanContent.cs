using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class AbstractClanContent 
{
    protected DataProviderClanInfo _dataProvider;
    public VisualTreeAsset template;
    protected VisualElement content;
    public Action<int> OnClickHide;
    protected VisualElement LoadContent(VisualElement content, VisualElement detailedInfoElement)
    {
        if (content == null)
        {
            return detailedInfoElement.Query<VisualElement>("content");
        }

        return content;
    }

    protected void ClearContent(VisualElement content)
    {
        if (content != null)
        {
            content.Clear();
        }
    }

    protected void SubscribeCloseButton(Button cancelButton, VisualElement detailedInfoElement)
    {
        if (cancelButton != null)
        {
            void OnClick()
            {
                detailedInfoElement.style.display = DisplayStyle.None;
                cancelButton.clicked -= OnClick;  // Unsubscribe here
                OnClickHide?.Invoke(-1);
            }

            cancelButton.clicked += OnClick;
        }
    }
}
