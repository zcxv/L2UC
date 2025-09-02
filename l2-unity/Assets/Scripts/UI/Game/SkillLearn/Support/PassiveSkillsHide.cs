using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PassiveSkillsHide : AbstractSkills
{
    private SkillListWindow _skillLearn;

    private VisualElement _passiveSkillPanel;
    private VisualTreeAsset _templateBoxPanel;
    private VisualTreeAsset _templatePanel8x1;
    private List<VisualElement> _boxPanelsAbility;
    private List<VisualElement> _boxPanelsSubject;

    private const string _rowNameAbility = "RowAbility";
    private const string _rowNameSubject = "RowSubject";

    private const string _allContentPhysical = "AbilityContent";
    private const string _allContentMagical = "SubjectContent";
    private List<SkillInstance> _tempList;
    private VisualTreeAsset _templateSlotSkill;


    public PassiveSkillsHide(SkillListWindow _skillLearn)
    {
        this._skillLearn = _skillLearn;
    }

    public void SetPassiveSkillTemplate(VisualTreeAsset templatePassiveSkill, VisualTreeAsset templateBoxPanel, VisualTreeAsset templatePanel8x1, VisualTreeAsset templateSlotSkill)
    {
        _passiveSkillPanel = ToolTipsUtils.CloneOne(templatePassiveSkill);
        _templateBoxPanel = templateBoxPanel;
        _templatePanel8x1 = templatePanel8x1;
        _templateSlotSkill = templateSlotSkill;
        _boxPanelsAbility = new List<VisualElement>();
        _boxPanelsSubject = new List<VisualElement>();
    }


    public VisualElement GetOrCreateTab(VisualElement content)
    {
        if (_passiveSkillPanel != null)
        {
            content.Clear();
            content.Add(_passiveSkillPanel);
        }

        return _passiveSkillPanel;
    }

    public void CreateSlots(List<SkillInstance> list)
    {
        int panelCount = CalculatePanelCount(list);

        var abilitySkills = list.Where(s => s.IsPassive).ToList();
        var subjectSkills = new List<SkillInstance>();

        var rowPhysical = _passiveSkillPanel.Q(_rowNameAbility);
        var rowMagical = _passiveSkillPanel.Q(_rowNameSubject);

        var allContentPhysical = _passiveSkillPanel.Q(_allContentPhysical);
        var allContentMagical = _passiveSkillPanel.Q(_allContentMagical);

        ShowPanelIfCount1(abilitySkills, rowPhysical, allContentPhysical);
        ShowPanelIfCount1(subjectSkills, rowMagical, allContentMagical);

        CreateAbilitySlots(abilitySkills, rowPhysical);
        CreateSubjectSlots(subjectSkills, rowMagical);

        HidePanelIfCount0(abilitySkills, rowPhysical, allContentPhysical);
        HidePanelIfCount0(subjectSkills, rowMagical, allContentMagical);
        _tempList = list;
    }





    public void CreateAbilitySlots(List<SkillInstance> list, VisualElement rowPhysical)
    {
        if (rowPhysical != null)
        {
            var boxPanel = ToolTipsUtils.CloneOne(_templateBoxPanel);
            var panels = base.CreateSlots(list, _templatePanel8x1, _templateSlotSkill, boxPanel);
            _boxPanelsAbility.Add(panels);
            rowPhysical.Add(panels);
        }
        else
        {
            Debug.LogWarning("ActiveSkillsHide > not found root panels ");
        }
    }

    public void CreateSubjectSlots(List<SkillInstance> list, VisualElement rowMagical)
    {
        if (rowMagical != null)
        {
            var _boxPanel = ToolTipsUtils.CloneOne(_templateBoxPanel);
            var panels = base.CreateSlots(list, _templatePanel8x1, _templateSlotSkill, _boxPanel);
            _boxPanelsSubject.Add(panels);
            rowMagical.Add(panels);
        }
        else
        {
            Debug.LogWarning("ActiveSkillsHide > not found root panels ");
        }
    }







    public void clickDfAbiliti(UnityEngine.UIElements.Button btn, VisualElement _activeTab_debilitatingContent, int[] _arrDfPassiveSelect)
    {
        if (_arrDfPassiveSelect[0] == 0)
        {
            ChangeDfBox(btn, fillBackgroundDf[0]);
            _arrDfPassiveSelect[0] = 1;
            HideSkillbar(true, _activeTab_debilitatingContent , _skillLearn);
        }
        else
        {
            ChangeDfBox(btn, fillBackgroundDf[1]);
            _arrDfPassiveSelect[0] = 0;
            HideSkillbar(false, _activeTab_debilitatingContent ,  _skillLearn);
        }
    }

    public void clickDfSubject(UnityEngine.UIElements.Button btn, VisualElement _activeTab_debilitatingContent, int[] _arrDfPassiveSelect)
    {
        if (_arrDfPassiveSelect[1] == 0)
        {
            ChangeDfBox(btn, fillBackgroundDf[0]);
            _arrDfPassiveSelect[1] = 1;
            HideSkillbar(true, _activeTab_debilitatingContent, _skillLearn);
        }
        else
        {
            ChangeDfBox(btn, fillBackgroundDf[1]);
            _arrDfPassiveSelect[1] = 0;
            HideSkillbar(false, _activeTab_debilitatingContent, _skillLearn);
        }
    }


}
