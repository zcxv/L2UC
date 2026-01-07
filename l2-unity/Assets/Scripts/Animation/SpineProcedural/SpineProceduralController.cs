using System.Collections.Generic;
using UnityEngine;

public class SpineProceduralController : MonoBehaviour
{
    private Dictionary<Transform, BoneModification> _activeModifications = new Dictionary<Transform, BoneModification>();

    public static SpineProceduralController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetBoneMod(Transform bone, BoneModification mod)
    {
        if (bone == null) return;

        if (!_activeModifications.ContainsKey(bone))
        {
            _activeModifications[bone] = mod;
        }

    }

    public void RemoveBoneMod(Transform bone)
    {
        if (bone != null) _activeModifications.Remove(bone);
    }

    void LateUpdate()
    {
        foreach (var entry in _activeModifications)
        {
            Transform bone = entry.Key;
            BoneModification mod = entry.Value;

            if (bone != null)
            {
                // 1. Применяем смещение (Position) с учетом веса
                if (mod.PositionOffset != Vector3.zero)
                {
                    bone.localPosition += mod.PositionOffset * mod.Weight;
                }

                // 2. Применяем поворот (Rotation) с учетом веса
                if (mod.RotationOffset != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.Euler(mod.RotationOffset);
                    // Интерполяция от "без изменений" до "целевой наклон" по весу
                    Quaternion weightedRotation = Quaternion.Slerp(Quaternion.identity, targetRotation, mod.Weight);
                    Debug.Log("SpineProceduralController: -> Use Rotate ");
                    bone.localRotation *= weightedRotation;
                }
            }
        }
    }
}
