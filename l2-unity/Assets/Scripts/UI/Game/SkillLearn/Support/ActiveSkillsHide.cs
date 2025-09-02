using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.tvOS;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.DebugUI.Table;

public class ActiveSkillsHide : AbstractSkills
{
    private SkillListWindow _skillLearn;
    private VisualElement _activeSkillPanel;
    private VisualTreeAsset _templateBoxPanel;
    private VisualTreeAsset _templatePanel8x1;
    private List<VisualElement> _boxPanelsPhysical;
    private List<VisualElement> _boxPanelsMagic;

    private const string _rowNamePhysical = "RowPhysical";
    private const string _rowNameMagical = "RowMagical";

    private const string _allContentPhysical = "PhysicalContent";
    private const string _allContentMagical = "MagicContent";
    private List<SkillInstance> _tempList;
    private VisualTreeAsset _templateSlotSkill;
    public ActiveSkillsHide(SkillListWindow _skillLearn)
    {
        this._skillLearn = _skillLearn;
    }

    public void SetActiveSkillTemplate(VisualTreeAsset templateActiveSkill , VisualTreeAsset templateBoxPanel ,  VisualTreeAsset templatePanel8x1 , VisualTreeAsset templateSlotSkill)
    {
        _activeSkillPanel = ToolTipsUtils.CloneOne(templateActiveSkill);
        _templateBoxPanel = templateBoxPanel;
        _templatePanel8x1 = templatePanel8x1;
        _templateSlotSkill = templateSlotSkill;
        _boxPanelsPhysical = new List<VisualElement>();
        _boxPanelsMagic = new List<VisualElement>();
    }


    public VisualElement GetOrCreateTab(VisualElement content)
    {
        if(_activeSkillPanel != null)
        {
            content.Clear();
            content.Add(_activeSkillPanel);
        }

        return _activeSkillPanel;
    }


    public void CreateSlots(List<SkillInstance> list)
    {
        int panelCount = CalculatePanelCount(list);

        var activeSkills = list.Where(s => !s.IsMagic()).ToList();
        var magicSkills = list.Where(s => s.IsMagic()).ToList();



        var rowPhysical = _activeSkillPanel.Q(_rowNamePhysical);
        var rowMagical = _activeSkillPanel.Q(_rowNameMagical);

        var allContentPhysical = _activeSkillPanel.Q(_allContentPhysical);
        var allContentMagical = _activeSkillPanel.Q(_allContentMagical);

        ShowPanelIfCount1(activeSkills, rowPhysical, allContentPhysical);
        ShowPanelIfCount1(magicSkills, rowMagical , allContentMagical);

        CreatePhysicalSlots(activeSkills, rowPhysical);
        CreateMagicalSlots(magicSkills, rowMagical);

        HidePanelIfCount0(activeSkills, rowPhysical, allContentPhysical);
        HidePanelIfCount0(magicSkills, rowMagical, allContentMagical);
        _tempList = list;
    }

    public void UpdateSlots(List<SkillInstance> list)
    {

        var allContentPhysical = _activeSkillPanel.Q(_allContentPhysical);
        var allContentMagical = _activeSkillPanel.Q(_allContentMagical);

        if (list == null | list.Count == 0)
        {
            //var rowPhysical_hide = _activeSkillPanel.Q(_rowNamePhysical);
            //var rowMagical_hide = _activeSkillPanel.Q(_rowNameMagical);

            HidePanels(list, allContentPhysical, allContentMagical);
            return;
        }



        List<SkillInstance> added;
        List<SkillInstance> removed;

        CompareSkillLists(list, _tempList, out added, out removed);

        var rowPhysical = _activeSkillPanel.Q(_rowNamePhysical);
        var rowMagical = _activeSkillPanel.Q(_rowNameMagical);



        var addActiveSkills = added.Where(s => !s.IsMagic()).ToList();
        var addMagicSkills = added.Where(s => s.IsMagic()).ToList();


        var rowsVirtualPhysical = rowPhysical.Query<VisualElement>(name: "RowsVirtual").ToList();
        var rowsVirtualMagic = rowMagical.Query<VisualElement>(name: "RowsVirtual").ToList();


        ShowActiveSkillsPanels(list, allContentPhysical);
        ShowMagicSkillsPanels(list, allContentMagical);


        UpdateSkills(addActiveSkills, removed, list, rowsVirtualPhysical, _templateSlotSkill , _templatePanel8x1 , _boxPanelsPhysical[0]);
        UpdateSkills(addMagicSkills, removed, list, rowsVirtualMagic, _templateSlotSkill, _templatePanel8x1, _boxPanelsMagic[0]);
        _tempList = list;

        HidePanelActiveSkillsPanel(list, allContentPhysical);
        HidePanelMagicSkillsPanel(list, allContentMagical);
    }

    

    private void ShowActiveSkillsPanels(List<SkillInstance> list, VisualElement allContentMagical)
    {
        var activeSkills = list.Where(s => !s.IsMagic()).ToList();


        if (activeSkills.Count > 0)
        {
            allContentMagical.style.display = DisplayStyle.Flex;
        }
    }

    private void ShowMagicSkillsPanels(List<SkillInstance> list, VisualElement allContentMagical)
    {

        var magicSkills = list.Where(s => s.IsMagic()).ToList();

        if (magicSkills.Count > 0)
        {
            allContentMagical.style.display = DisplayStyle.Flex;
        }
    }

    private void HidePanels(List<SkillInstance> list, VisualElement allContentMagical, VisualElement allContentPhysical)
    {

        if (list == null | list.Count == 0)
        {
            allContentPhysical.style.display = DisplayStyle.None;
            allContentMagical.style.display = DisplayStyle.None;
        }
    }

    private void HidePanelActiveSkillsPanel(List<SkillInstance> list,  VisualElement allContent)
    {
        var activeSkills = list.Where(s => !s.IsMagic()).ToList();

        if (activeSkills.Count == 0)
        {
            allContent.style.display = DisplayStyle.None;
        }
    }

    private void HidePanelMagicSkillsPanel(List<SkillInstance> list, VisualElement allContent)
    {
        var magicSkills = list.Where(s => s.IsMagic()).ToList();

        if (magicSkills.Count == 0)
        {
            allContent.style.display = DisplayStyle.None;
        }
    }

    public void CreatePhysicalSlots(List<SkillInstance> list , VisualElement rowPhysical)
    {
        if (rowPhysical != null)
        {
            var boxPanel = ToolTipsUtils.CloneOne(_templateBoxPanel);
            var panels = base.CreateSlots(list, _templatePanel8x1, _templateSlotSkill, boxPanel);
            _boxPanelsPhysical.Add(panels);
            rowPhysical.Add(panels);
        }
        else
        {
            Debug.LogWarning("ActiveSkillsHide > not found root panels ");
        }
    }

    public void CreateMagicalSlots(List<SkillInstance> list, VisualElement rowMagical)
    {
        if (rowMagical != null)
        {
            var _boxPanel = ToolTipsUtils.CloneOne(_templateBoxPanel);
            var panels = base.CreateSlots(list, _templatePanel8x1, _templateSlotSkill, _boxPanel);
            _boxPanelsMagic.Add(panels);
            rowMagical.Add(panels);
        }
        else
        {
            Debug.LogWarning("ActiveSkillsHide > not found root panels ");
        }
    }







  





    public void clickDfPhysical(UnityEngine.UIElements.Button btn , VisualElement _activeTab_physicalContent , int[] _arrDfSelect)
    {
        if (_arrDfSelect[0] == 0)
        {
            ChangeDfBox(btn, fillBackgroundDf[0]);
            _arrDfSelect[0] = 1;
            HideSkillbar(true, _activeTab_physicalContent , _skillLearn);
        }
        else
        {
            ChangeDfBox(btn, fillBackgroundDf[1]);
            _arrDfSelect[0] = 0;
            HideSkillbar(false, _activeTab_physicalContent, _skillLearn);
        }
    }


    public void clickDfMagic(UnityEngine.UIElements.Button btn , VisualElement _activeTab_magicContent, int[] _arrDfSelect)
    {
        if (_arrDfSelect[1] == 0)
        {
            ChangeDfBox(btn, fillBackgroundDf[0]);
            _arrDfSelect[1] = 1;
            HideSkillbar(true, _activeTab_magicContent, _skillLearn);
        }
        else
        {
            ChangeDfBox(btn, fillBackgroundDf[1]);
            _arrDfSelect[1] = 0;
            HideSkillbar(false, _activeTab_magicContent, _skillLearn);
        }
    }

    public void clickDfEnhancing(UnityEngine.UIElements.Button btn, VisualElement _activeTab_magicContent, int[] _arrDfSelect)
    {
        if (_arrDfSelect[2] == 0)
        {
            ChangeDfBox(btn, fillBackgroundDf[0]);
            _arrDfSelect[2] = 1;
            HideSkillbar(true, _activeTab_magicContent, _skillLearn);
        }
        else
        {
            ChangeDfBox(btn, fillBackgroundDf[1]);
            _arrDfSelect[2] = 0;
            HideSkillbar(false, _activeTab_magicContent, _skillLearn);
        }
    }

    public void clickDfDebilitating(UnityEngine.UIElements.Button btn, VisualElement _activeTab_debilitatingContent, int[] _arrDfSelect)
    {
        if (_arrDfSelect[3] == 0)
        {
            ChangeDfBox(btn, fillBackgroundDf[0]);
            _arrDfSelect[3] = 1;
            HideSkillbar(true, _activeTab_debilitatingContent, _skillLearn);
        }
        else
        {
            ChangeDfBox(btn, fillBackgroundDf[1]);
            _arrDfSelect[3] = 0;
            HideSkillbar(false, _activeTab_debilitatingContent, _skillLearn);
        }
    }

   
    public void clickDfClan(UnityEngine.UIElements.Button btn, VisualElement _activeTab_ClanContent, int[] _arrDfSelect)
    {
        if (_arrDfSelect[4] == 0)
        {
            ChangeDfBox(btn, fillBackgroundDf[0]);
            _arrDfSelect[4] = 1;
            HideSkillBarClan(true, _activeTab_ClanContent , "SkillBar0");
            HideSkillBarClan(true, _activeTab_ClanContent, "SkillBar1");
        }
        else
        {
            ChangeDfBox(btn, fillBackgroundDf[1]);
            _arrDfSelect[4] = 0;
            HideSkillBarClan(false, _activeTab_ClanContent, "SkillBar0");
            HideSkillBarClan(false, _activeTab_ClanContent, "SkillBar1");

        }
    }



   

    private void HideSkillBarClan(bool hide, VisualElement content , string skillBarName)
    {
        var skillBar = GetSkillBarByName(content, skillBarName);
        if(skillBar != null) _skillLearn.HideElement(hide, skillBar);
    }

   

    private VisualElement GetSkillBarByName(VisualElement content , string skillBarName)
    {
        var childreb = content.Children();

        foreach (VisualElement item in childreb)
        {
            if (item.name.Equals(skillBarName))
            {
                return item;
            }
        }

        return null;
    }



   
}
