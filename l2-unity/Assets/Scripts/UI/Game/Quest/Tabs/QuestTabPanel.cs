using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestTabPanel : AbstractToggle , IContent 
{
    private VisualElement _container;
    private VisualElement _insideContent;
    private const string _contentSingleName = "inside_content";
    private VisualTreeAsset _templateToggleButton;
    private const string _dfButtonName = "DF_Button";
    private const string _dataElementName = "ListData";
    private int[] _selectedData;
    private  List<QuestInstance> _questList;
    private  List<EventCallback<ClickEvent>> _questListCallback;
    private  List<VisualElement> _questListButtons;
    public void SetTemplateContent(VisualTreeAsset templateContainer , List<VisualTreeAsset> otherElement)
    {
        var _templateInsideContent = otherElement[0];
        _container = ToolTipsUtils.CloneOne(templateContainer);
        _insideContent = ToolTipsUtils.CloneOne(_templateInsideContent);
        _questList = new List<QuestInstance>();
        _questListCallback = new List<EventCallback<ClickEvent>>();
        _questListButtons = new List<VisualElement>();
        _templateToggleButton = otherElement[1];
    }

    // rootElement.Q<Button>("DF_Button")?.RegisterCallback<ClickEvent>(evt => ClickDfPhysical((Button)evt.target, _arrDfActiveSelect));
    public void AddElementsToContent<T>(params T[] elements)
    {
        destroyRegisterCallBack(_questListButtons, _questListCallback);
        clearOldList(_questList);

        _questList = GetList(elements);
        _selectedData = new int[_questList.Count];
        _insideContent.Clear();

        if (_questList != null)
        {
            var countQuestLabel = _container.Q<Label>("LabelCountQuest");

            if(countQuestLabel != null) countQuestLabel.text = "("+ _questList.Count.ToString()+"/"+"40"+")";

            for (int i=0; i < _questList.Count; i++)
            {
                var quest = _questList[i];

                var buttonElement = ToolTipsUtils.CloneOne(_templateToggleButton);

                var nameLabel = buttonElement.Q<Label>("NameLabel");
                var dataLabel = buttonElement.Q<Label>("DataLabel");
                VisualElement data =  buttonElement.Q<VisualElement>(_dataElementName);

                var eventButton = RegisterClickButton(buttonElement, _selectedData, data , i);
                AddCallback(_questListCallback, _questListButtons, buttonElement, eventButton);
                SetData(quest, nameLabel, dataLabel);

                _insideContent.Add(buttonElement);
            }
        }

        elements = null;
    }


    private void AddCallback(List<EventCallback<ClickEvent>> questListCallback , List<VisualElement> questListButtons , VisualElement buttonElement , EventCallback<ClickEvent>  eventButton)
    {
        _questListCallback.Add(eventButton);
        _questListButtons.Add(buttonElement);
    }

    private void SetData(QuestInstance quest , Label nameLabel , Label dataLabel)
    {
        if(nameLabel !=null && dataLabel != null)
        {
            string name = quest.QuestName();
            //string progName = quest.QuestProgName();
            string sourceName = quest.GetQuestSource();
            //string entityName = quest.GetQuestEntity();
            nameLabel.text = name;
            dataLabel.text = sourceName;
        }

    }
    private void destroyRegisterCallBack(List<VisualElement> questListButtons, List<EventCallback<ClickEvent>> questListCallback)
    {
        if (questListButtons != null && questListCallback != null)
        {
            for (int i = 0; i < questListButtons.Count && i < questListCallback.Count; i++)
            {
                var button = questListButtons[i].Q<Button>(_dfButtonName);
                var callback = questListCallback[i];
                if (button != null && callback != null)
                {
                    button.UnregisterCallback(callback);
                }
            }
            questListCallback.Clear();
            questListButtons.Clear();
        }
    }

    private void clearOldList(List<QuestInstance> questList)
    {
        if(questList != null && questList.Count >0) questList.Clear();
    }

    private EventCallback<ClickEvent> RegisterClickButton(VisualElement buttonElement, int[] selectedData, VisualElement dataElement, int indexButton)
    {
        if (dataElement != null)
        {
            EventCallback<ClickEvent> callback = evt => ClickDf((Button)evt.target, selectedData, dataElement, indexButton);
            buttonElement.Q<Button>(_dfButtonName)?.RegisterCallback(callback);
            return callback;
        }
        return null;
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
