using System;
using UnityEngine;
public class FpsLimiter : MonoBehaviour {
    
    [SerializeField] private int focusedFrameRate = 144;
    [SerializeField] private int unfocusedFrameRate = 30;

#if !UNITY_EDITOR
    private void Start() {
        QualitySettings.vSyncCount = 0;

        // Set the initial frame rate
        SetFrameRate(focusedFrameRate);
    }

    private void Update() {
        // Check if the application window is focused
        bool isFocused = Application.isFocused;

        // Set the frame rate based on focus state
        int targetFrameRate = isFocused ? focusedFrameRate : unfocusedFrameRate;
        SetFrameRate(targetFrameRate);
    }
#endif
    
    private void SetFrameRate(int frameRate) {
        // Set the target frame rate
        Application.targetFrameRate = frameRate;
    }

#if UNITY_EDITOR
    private void Reset() {
        focusedFrameRate = 144;
        unfocusedFrameRate = 30;
    }
#endif

    
}
