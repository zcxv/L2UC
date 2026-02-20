using System;
using UnityEngine;

public class SceneObjectLocator : MonoBehaviourSingleton<SceneObjectLocator> {

    [NonSerialized] public GameObject Avatars;
    [NonSerialized] public GameObject Player;

    protected override void Awake() {
        base.Awake();
        Avatars = new GameObject("Avatars");
        Player = new GameObject("Player");
    }
    
}