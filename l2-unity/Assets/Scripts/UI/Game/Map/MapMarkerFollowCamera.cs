using UnityEngine;
using UnityEngine.UIElements;

public class MapMarkerFollowCamera : MonoBehaviour
{
    public Camera targetCamera;
    private VisualElement _marker;

    // храним последний применённый угол (в диапазоне -180..180)
    float lastAngle = float.NaN;

    public void SetElement(VisualElement marker)
    {
        _marker = marker;

        // Устанавливаем точку вращения в центр элемента (если нужно).
        // В зависимости от версии Unity API эта строчка может выглядеть иначе.
        //marker.style.transformOrigin = new StyleTransformOrigin(
        //    new TransformOrigin(new Length(50, LengthUnit.Percent), new Length(50, LengthUnit.Percent))
        //);
    }

    public void UpdateTurn(Camera targetCamera)
    {
        if (_marker == null || targetCamera == null) return;

        // Выбираем нужную ось: для top-down — yaw (Y)
        float cameraYaw = targetCamera.transform.eulerAngles.y;

        // Если маркер поворачивается в противоположную сторону — просто уберите знак минус.
        // Используйте cameraYaw или -cameraYaw в зависимости от желаемого результата.
        float desiredAngle = cameraYaw; // <- исправление: убрали минус

        // Нормализуем в диапазон -180..180, чтобы плавно сравнивать и избегать прыжков 359->0
        desiredAngle = NormalizeAngle(desiredAngle);

        // Обновляем только если угол изменился (экономим присвоения стиля)
        if (Mathf.Approximately(desiredAngle, lastAngle)) return;
        lastAngle = desiredAngle;

        _marker.style.rotate = new Rotate(new Angle(desiredAngle, AngleUnit.Degree));
    }

    static float NormalizeAngle(float a)
    {
        a = (a + 180f) % 360f;
        if (a < 0) a += 360f;
        return a - 180f;
    }
}