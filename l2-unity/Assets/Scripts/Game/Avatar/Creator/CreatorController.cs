using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CreatorService))]
public class CreatorController : MonoBehaviourSingleton<CreatorController> {

    private const float ROTATION_SPEED = 69f;
    private const int UNSELETED = -1;

    public bool IsSelected => avatarIndex != UNSELETED;
    
    private int avatarIndex = -1;
    private Vector3 rotateDirection = Vector3.zero;

    public void Init() {
        CreatorService.Instance.Spawn();
    }

    public void Select(int race, int classType, int sex) {
        if (IsSelected) {
            ResetSelection();
        }
        
        avatarIndex = CreatorService.Instance.GetIndex((CreatorRace) race, (CreatorClassType) classType, sex);
    }

    public void ResetSelection() {
        CreatorService.Instance.ResetAvatar(avatarIndex);
        avatarIndex = UNSELETED;
    }
    
    public void Rotate(Vector3 direction) {
        rotateDirection = direction;
    }

    public void SetAppearance(int face, int hairColor, int hairStyle) {
        CreatorService.Instance.SetAppearance(avatarIndex, face, hairColor, hairStyle);
    }

    private void Update() {
        if (IsSelected && rotateDirection != Vector3.zero) {
            CreatorService.Instance.Rotate(avatarIndex, rotateDirection * (Time.deltaTime * ROTATION_SPEED));
        }
    }

}