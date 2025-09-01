using UnityEngine;
using UnityEngine.UIElements;

public class SkillSlot : L2DraggableSlot
{
    private SkillInstance _skillInstance { get;  set; }
    private bool _empty = false;
    public SkillSlot(VisualElement slotElement, int position, SlotType slotType) : base(position, slotElement, slotType, true, false)
    {
        _slotElement = slotElement;
        _position = position;
    }

    protected override void HandleLeftClick()
    {
        Debug.Log("CLICK EVENT SKILL SLOT");
        //PlayerActions.Instance.UseAction((ActionType)ActionId);
    }

    protected override void HandleRightClick()
    {
    }

    protected override void HandleMiddleClick()
    {
    }

    

    public void AssignSkill(int skillId, int level)
    {
        _id = skillId;
        _level = level;

        var skill = SkillgrpTable.Instance.GetSkill(skillId, level);
        Assign(skill);
    }

    public void AssignSkill(SkillInstance skillInstance)
    {
        _id = skillInstance.SkillID;
        _level = skillInstance.Level;
        _skillInstance = skillInstance;

        var skill = SkillgrpTable.Instance.GetSkill(_id, _level);
        Assign(skill);
    }

    private void Assign(Skillgrp skill)
    {
        if (skill == null)
        {
            //Debug.LogWarning($"AssignSkill: skill not found (id={skillId}, level={level})");
            return;
        }

        _empty = false;

        if (_slotDragManipulator != null)
            _slotDragManipulator.enabled = true;


        _slotElement?.RemoveFromClassList("empty");

        AddIcon(skill);

        // Обновляем подсказку (если манипулятор есть)
        //_tooltipManipulator?.SetText("Demo Text ToolTips");
    }

    public void AssignEmpty()
    {
        _empty = true;
        _id = 0;
        _name = "Unkown";
        _description = "Unkown item.";
        _skillInstance = null;

        if (_slotElement != null)
        {
            ResetIcon();
        }
    }

    private void AddIcon(Skillgrp skill)
    {
        var icon = IconManager.Instance.LoadTextureByName(skill.Icon);
        if (icon != null)
        {
            _slotBg.style.backgroundImage = icon;
        }
        else
        {
            Debug.LogWarning($"AssignSkill: icon '{skill.Icon}' not found for skill id={skill.Id}");
        }
    }

    public void ResetIcon()
    {
        StyleBackground background = new StyleBackground(IconManager.Instance.GetInvetoryDefaultBackground());
        if (SlotBg != null)
        {
            _slotBg.style.backgroundImage = background;
            _slotDragManipulator.enabled = false;

        }
        else
        {

            _slotElement.style.backgroundImage = null;
        }
    }


}
