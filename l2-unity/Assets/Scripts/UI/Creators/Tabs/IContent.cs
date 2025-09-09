using UnityEngine;
using UnityEngine.UIElements;

public interface IContent 
{
    void SetTemplateContent(VisualTreeAsset templateContent);
    VisualElement GetOrCreateTab(VisualElement content);
}
