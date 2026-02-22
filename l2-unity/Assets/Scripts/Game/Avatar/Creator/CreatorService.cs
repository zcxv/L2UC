using System;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class CreatorService : MonoBehaviourSingleton<CreatorService> {

    private CreatorEntity[] models;
    
    private void Start() {
         models = GameObject.FindGameObjectsWithTag(Tags.TRIGGER)
            .Select(go => go.GetComponent<CreatorEntity>())
            .Where(go => go != null)
            .ToArray();
    }

    public void Spawn() {
        for (int i = 0; i < models.Length; i++) {
            models[i].Spawn();
        }
    }

    public int GetIndex(CreatorRace race, CreatorClassType classType, int sex) {
        for (int i = 0; i < models.Length; i++) {
            var model =  models[i];
            if (model.Race == race && model.ClassType == classType && model.Sex == sex) {
                return i;
            }
        }

        Debug.LogError(
            "Create Avatar not found for specified parameters: " +
            $"race:'{Enum.GetName(typeof(CreatorRace), race)}', " +
            $"classType:'{Enum.GetName(typeof(CreatorClassType), classType)}', " +
            $"sex:'{sex}'"
        );
        return -1;
    }

    public void ResetAvatar(int index) {
        models[index].ResetModel();
    }

    public void SetAppearance(int index, int face, int hairColor, int hairStyle) {
        var model = models[index];
        model.SetFace(face);
        model.SetHair(hairColor, hairStyle);
    }

    public void Rotate(int index, Vector3 rotation) {
        var model = models[index];
        model.Rotate(rotation);
    }
}