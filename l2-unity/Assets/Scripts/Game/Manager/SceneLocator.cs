using System;
using UnityEngine;

public class SceneLocator : MonoBehaviourSingleton<SceneLocator> {

    [NonSerialized] public GameObject Avatars;
    [NonSerialized] public GameObject Player;

    // FIXME m0nster: не забыть положить на объект Game в Menu сцене
    protected override void Awake() {
        base.Awake();
        Avatars = new GameObject("Avatars");
        Player = new GameObject("Player");
    }
    
}