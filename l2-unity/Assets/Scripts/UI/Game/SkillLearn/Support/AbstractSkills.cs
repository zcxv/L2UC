using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static L2Slot;

public class AbstractSkills
{
    protected string[] fillBackgroundDf = { "Data/UI/Window/Skills/QuestWndPlusBtn_v2", "Data/UI/Window/Skills/Button_DF_Skills_Down_v3" };
    private const string _rowNameInnerPanel = "RowsVirtual";
    protected void ChangeDfBox(Button btn, string texture)
    {
        IEnumerable<VisualElement> children = btn.Children();
        var e = children.First();
        e.style.display = DisplayStyle.Flex;
        Texture2D iconDfNoraml = LoadTextureDF(texture);
        SetBackgroundDf(btn, iconDfNoraml);
    }
    protected void HideSkillbar(bool hide, VisualElement content , SkillListWindow _skillLearn)
    {
        var skillBar = GetSkillBar(content);
        _skillLearn.HideElement(hide, skillBar);
    }

    protected void SetBackgroundDf(UnityEngine.UIElements.Button btn, Texture2D iconDfNoraml)
    {
        btn.style.backgroundImage = new StyleBackground(iconDfNoraml);
    }

    protected Texture2D LoadTextureDF(string path)
    {
        return Resources.Load<Texture2D>(path);
    }

    protected VisualElement GetSkillBar(VisualElement content)
    {
        var childreb = content.Children();
        int i = 0;
        foreach (VisualElement item in childreb)
        {
            if (i > 0)
            {
                return item;
            }
            i++;
        }

        return null;
    }

    public void UpdateIconSkill(List<SkillInstance> list , List<VisualElement> rows)
    {

    }

    protected VisualElement CreateSlots(List<SkillInstance> skillList  , int panelCount , VisualTreeAsset templatePanel8x1 , VisualTreeAsset templateSlotsSkill, VisualElement boxPanel , VisualElement skillPanel)
    {
            for (int i = 0; i < panelCount; i++)
            {
                var templatePanel8x1_i = ToolTipsUtils.CloneOne(templatePanel8x1);
                boxPanel.Add(templatePanel8x1_i);
                AddSlots(skillList, templatePanel8x1_i, templateSlotsSkill);
            }

         return boxPanel;

    }


    private void AddSlots(List<SkillInstance> skillList , VisualElement templatePanel8x1_i , VisualTreeAsset templateSlotsSkill)
    {
        for(int i =0; i < skillList.Count; i++)
        {
            var rowNameVirtual = templatePanel8x1_i.Q(_rowNameInnerPanel);
            var skillInstance = skillList[i];

            if (rowNameVirtual != null)
            {
                var slotElement = ToolTipsUtils.CloneOne(templateSlotsSkill);
                var _skillSlot =  new SkillSlot(slotElement, i, SlotType.SkillWindow);
                _skillSlot.AssignSkill(skillInstance);

                rowNameVirtual.Add(slotElement);
            }
        }
    }

    protected int CalculatePanelCount(List<SkillInstance> list)
    {
        int skillsPerPanel = 8;
        int totalSkills = list.Count;
        int panelCount = (int)Math.Ceiling((double)totalSkills / skillsPerPanel);
        return panelCount;
    }
}
