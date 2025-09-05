using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillSlot : L2DraggableSlot
{
    private SkillInstance _skillInstance { get;  set; }
    private bool _empty = false;
    public event Action<int> EventLeftClick;
    public SkillSlot(VisualElement slotElement, int position, SlotType slotType) : base(position, slotElement, slotType, true, false)
    {
        _slotElement = slotElement;
        _position = position;
    }

    protected override void HandleRightClick()
    {
    }

    protected override void HandleMiddleClick()
    {
    }

    protected override void HandleLeftClick()
    {
        Debug.Log("Handle Left Click");
        EventLeftClick?.Invoke(_position);
        //SetSelected();
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

    public void UpdateData(SkillInstance skillInstance)
    {
        _id = skillInstance.SkillID;
        _level = skillInstance.Level;
        _skillInstance = skillInstance;
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


    public void AssignDestroy()
    {
        _empty = true;
        _id = 0;
        _name = "Unknown";
        _description = "Unknown item.";
        _skillInstance = null;

        _slotElement.parent?.Remove(_slotElement);

    }
    public void AssignEmpty()
    {
        _empty = true;
        _id = 0;
        _name = "Unknown";
        _description = "Unknown item.";
        _skillInstance = null;

        // Сброс иконки
        if (_slotElement != null)
        {
            _slotElement.style.display = DisplayStyle.None;
            ResetIcon();
            // Можно также сбросить текст подсказки, если требуется
            //_tooltipManipulator?.SetText("");
        }

        // Отключаем drag
        if (_slotDragManipulator != null)
            _slotDragManipulator.enabled = false;
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

        if (SlotBg != null)
        {
            _slotBg.style.backgroundImage = null;
            _slotDragManipulator.enabled = false;

        }
        else
        {

            _slotElement.style.backgroundImage = null;
        }
    }

    public void OnClickLeftEvent(int position)
    {
        Debug.Log("Click Event Skill Slot");
    }


}
