using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MapPanner 
{
    private VisualElement _viewport;
    private VisualElement _map;

    private Vector2 lastPointer;
    private Vector2 offset; // текущее смещение по карте (от левого-верхнего угла)

    private bool dragging;

    private const float VIEW_W = 294f;
    private const float VIEW_H = 300f;
    private const float MAP_W = 1812f;
    private const float MAP_H = 2620f;

    public void SetElements(VisualElement viewport, VisualElement map)
    {
        _viewport = viewport;
        _viewport.pickingMode = PickingMode.Position;
        _map = map;

    }

    public void RegisterCallback()
    {
        // подписываемся на указательные события
        _viewport.RegisterCallback<PointerDownEvent>(OnPointerDown);
        _viewport.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        _viewport.RegisterCallback<PointerUpEvent>(OnPointerUp);
        _viewport.RegisterCallback<PointerCancelEvent>(OnPointerCancel);

    }

    void UnRegisterCallback()
    {
        if (_viewport != null)
        {
            _viewport.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            _viewport.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            _viewport.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            _viewport.UnregisterCallback<PointerCancelEvent>(OnPointerCancel);
        }
    }

    void OnPointerDown(PointerDownEvent evt)
    {
        // захватить указатель, чтобы получать перемещения даже если курсор уходит за пределы элемента
        Debug.Log("OnPointerDown");
        _viewport.CapturePointer(evt.pointerId);
        dragging = true;
        lastPointer = evt.localPosition;
        evt.StopPropagation();
    }

    void OnPointerMove(PointerMoveEvent evt)
    {
        Debug.Log("OnPointerMove ");
        if (!dragging) return;

        Vector2 local = evt.localPosition;
        Vector2 delta = local - lastPointer;

        // перемещение карты противоположно движению мыши (таскаем карту)
        offset -= delta;

        // clamp по границам карты
        float maxX = Mathf.Max(0f, MAP_W - VIEW_W);
        float maxY = Mathf.Max(0f, MAP_H - VIEW_H);
        offset.x = Mathf.Clamp(offset.x, 0f, maxX);
        offset.y = Mathf.Clamp(offset.y, 0f, maxY);

        // применяем смещение: сдвигаем карту влево/вверх на offset
        _map.style.left = -offset.x;
        _map.style.top = -offset.y;

        lastPointer = local;
        evt.StopPropagation();
    }

    void OnPointerUp(PointerUpEvent evt)
    {
        Debug.Log("OnPointerUp ");
        if (dragging)
        {
            _viewport.ReleasePointer(evt.pointerId);
            dragging = false;
            evt.StopPropagation();
        }
    }

    void OnPointerCancel(PointerCancelEvent evt)
    {
        if (dragging)
        {
            Debug.Log("OnPointerCancel ");
            _viewport.ReleasePointer(evt.pointerId);
            dragging = false;
            evt.StopPropagation();
        }
    }





}