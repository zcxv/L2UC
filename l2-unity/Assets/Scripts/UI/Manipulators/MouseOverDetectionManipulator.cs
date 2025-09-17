using UnityEngine;
using UnityEngine.UIElements;

public class MouseOverDetectionManipulator : PointerManipulator
{
    private bool _enabled = true;
    private bool _overThisManipulator = false;
    private L2UI _ui;
    private VisualElement _subElement;

    public bool MouseOver => _overThisManipulator;

    public MouseOverDetectionManipulator(VisualElement target)
    {
        this.target = target;
        _ui = L2GameUI.Instance != null ? L2GameUI.Instance : L2LoginUI.Instance;
    }

    public void RefreshTargetElement(VisualElement newTarget)
    {
        if (target != null) UnregisterCallbacksFromTarget();
        target = newTarget;
        if (target != null) RegisterCallbacksOnTarget();
    }

    public void SetSubElement(VisualElement subElement) => _subElement = subElement;
    public void Enable() => _enabled = true;
    public void Disable()
    {
        _enabled = false;
        if (_overThisManipulator) UnBlock();
    }
    public void SetBlock(bool isBlock) { if (isBlock) Block(); else UnBlock(); }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerEnterEvent>(PointerEnterHandler);
        target.RegisterCallback<PointerOverEvent>(PointerOverHandler);
        target.RegisterCallback<PointerLeaveEvent>(PointerLeaveHandler);
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        if (_subElement != null)
        {
            _subElement.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            _subElement.RegisterCallback<PointerLeaveEvent>(PointerLeaveHandler);
            _subElement.RegisterCallback<PointerOverEvent>(PointerOverHandler);
        }
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerEnterEvent>(PointerEnterHandler);
        target.UnregisterCallback<PointerOverEvent>(PointerOverHandler);
        target.UnregisterCallback<PointerLeaveEvent>(PointerLeaveHandler);
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        if (_subElement != null)
        {
            _subElement.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            _subElement.UnregisterCallback<PointerLeaveEvent>(PointerLeaveHandler);
            _subElement.UnregisterCallback<PointerOverEvent>(PointerOverHandler);
        }
    }

    private bool IsVisibleAndContains(Vector2 pos, VisualElement el) =>
        el != null && el.resolvedStyle.display != DisplayStyle.None && el.worldBound.Contains(pos);

    private void PointerEnterHandler(PointerEnterEvent evt)
    {
        if (_enabled && (_subElement == null || !IsVisibleAndContains(evt.position, _subElement)))
            Block();
    }

    private void PointerOverHandler(PointerOverEvent evt)
    {
        if (_enabled && (_subElement == null || !IsVisibleAndContains(evt.position, _subElement)))
            Block();
        else if (_enabled && _subElement != null && IsVisibleAndContains(evt.position, _subElement))
            Block();
    }

    private void PointerLeaveHandler(PointerLeaveEvent evt)
    {
        if (!_enabled) return;
        if (!IsVisibleAndContains(evt.position, target) && !IsVisibleAndContains(evt.position, _subElement))
            UnBlock();
    }

    private void PointerDownHandler(PointerDownEvent evt)
    {
        if (_enabled && (IsVisibleAndContains(evt.position, _subElement) || IsVisibleAndContains(evt.position, target)))
            Block();
        else
            UnBlock();
    }

    private void Block()
    {
        _ui.MouseOverUI = true;
        _overThisManipulator = true;
    }

    private void UnBlock()
    {
        _ui.MouseOverUI = false;
        _overThisManipulator = false;
    }
}