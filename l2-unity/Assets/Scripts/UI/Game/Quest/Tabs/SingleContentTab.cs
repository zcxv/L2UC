using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SingleContentTab : IContent
{
    private VisualElement _container;
    private VisualElement _insideContent;
    private const string _contentSingleName = "inside_content";
    private VisualTreeAsset _templateToggleButton;
    public void SetTemplateContent(VisualTreeAsset templateContainer , List<VisualTreeAsset> otherElement)
    {
        var _templateInsideContent = otherElement[0];
        _container = ToolTipsUtils.CloneOne(templateContainer);
        _insideContent = ToolTipsUtils.CloneOne(_templateInsideContent);

        _templateToggleButton = otherElement[1];
    }


    public void AddElementsToContent<T>(params T[] elements)
    {
        List<QuestInstance> questList = GetList(elements);

        if (questList != null)
        {
            _insideContent.Clear();

            var countQuestLabel = _container.Q<Label>("LabelCountQuest");

            if(countQuestLabel != null) countQuestLabel.text = "("+ questList .Count.ToString()+"/"+"40"+")";

            foreach (QuestInstance quest in questList)
            {
                var buttonElement = ToolTipsUtils.CloneOne(_templateToggleButton);

                var nameLabel = buttonElement.Q<Label>("NameLabel");
                var dataLabel = buttonElement.Q<Label>("DataLabel");

                string name = quest.QuestName();
                //string progName = quest.QuestProgName();
                 string sourceName = quest.GetQuestSource();
                //string entityName = quest.GetQuestEntity();
                nameLabel.text = name;
                dataLabel.text = sourceName;

                _insideContent.Add(buttonElement);
            }
        }

        elements = null;
    }


    private List<QuestInstance> GetList<T>(params T[] elements)
    {
        return elements
          .SelectMany<T, QuestInstance>(e => e is IEnumerable<QuestInstance> enumerable ? enumerable : new[] { e as QuestInstance })
          .Where(q => q != null)
          .ToList();
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
