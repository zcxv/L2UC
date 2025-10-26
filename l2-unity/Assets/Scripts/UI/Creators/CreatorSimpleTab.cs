using System;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class CreatorSimpleTab : AbstractCreator, ICreatorSimpleTab
{
    public event Action<int> EventSwitchTabByIndexOfTab;
    private VisualTreeAsset[] _templates;
    private VisualElement _injectTemplate;
    public void SetContent(int idTemplate)
    {
        VisualElement tabContent =  GetActiveContent();
        _injectTemplate = GetTemplateById(idTemplate);

        if (_injectTemplate == null)
        {
            Debug.LogWarning("CreatorSimpleTab> SetContent: Not Fount Inject Template");
        }

        if (tabContent != null & _injectTemplate != null)
        {
            tabContent.Clear();
            tabContent.Add(_injectTemplate);
        }

    }

    public void SetClickActiveTab(int position)
    {
        throw new NotImplementedException();
    }

    private VisualElement GetTemplateById(int idTemplate)
    {

        if (!ArrayUtils.IsValidIndexArray(_templates, idTemplate))
        {
            return null;
        }

        VisualTreeAsset template = _templates[idTemplate];
        return ToolTipsUtils.CloneOne(template);
    }


    public void LoadAsset(Func<string, VisualTreeAsset> loaderFunc , string[] loadTemplate)
    {
        _templates = new VisualTreeAsset[loadTemplate.Length];

        for (int i = 0; i < loadTemplate.Length; i++)
        {
            _templates[i] = loaderFunc(loadTemplate[i]);
        }
    }

    public VisualElement[] GetVisualElements(string[] names)
    {
        VisualElement[] elements = new VisualElement[names.Length];

        for (int i =0; i< names.Length; i++)
        {
            string name = names[i];
            if (name == null) continue;
            elements[i] = _injectTemplate.Q<VisualElement>(name);
        }

        return elements;
    }
}
