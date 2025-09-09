using UnityEngine;
using UnityEngine.UIElements;

public class SingleContentTab : IContent
{
    private VisualElement _container;
    private VisualElement _insideContent;
    private const string _contentSingleName = "inside_content";
    public void SetTemplateContent(VisualTreeAsset templateContainer)
    {
        _container = ToolTipsUtils.CloneOne(templateContainer);
    }
    public void SetTemplateInsideContent(VisualTreeAsset insideContentTemplate)
    {
        if (insideContentTemplate != null)
        {
            _insideContent = ToolTipsUtils.CloneOne(insideContentTemplate);
        }
    }
    public VisualElement GetOrCreateTab(VisualElement content)
    {
        if (_container != null && _insideContent != null)
        {
            content.Clear();
            AddContent(_container);
            content.Add(_container);
        }

        return _container;
    }

    private void AddContent(VisualElement _container)
    {
        var insideContent = GetElementById(_container, _contentSingleName);
        if (insideContent != null && _insideContent != null)
        {
            insideContent.Add(_insideContent);
        }
    }
    private VisualElement GetElementById(VisualElement content , string id)
    {
        var btn = content.Q<VisualElement>(id);
        if (btn == null)
        {
            Debug.LogError(id + " can't be found.");
            return null;
        }

        return btn;
    }
}
