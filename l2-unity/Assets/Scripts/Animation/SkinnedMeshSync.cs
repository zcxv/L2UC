using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshSync : MonoBehaviour {
    
    private SkinnedMeshRenderer rootRenderer;
    private readonly SkinnedMeshRenderer[] boneRenderer = new SkinnedMeshRenderer[8];

    private readonly Queue<GameObject> waitingObjects = new();
    private readonly List<Renderer> temporarilyHiddenRenderers = new();
    private bool isProcessing = false;

    private void Awake() {
        rootRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Start() {
        SyncMesh();
    }

    public void SyncMesh() {
        Array.Clear(boneRenderer, 0, boneRenderer.Length);
        for (int i = 0, childIndex = 0; i < transform.parent.childCount; i++) {
            Transform child = transform.parent.GetChild(i);
            if (child == transform) {
                continue;
            }
            
            boneRenderer[childIndex++] = child.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        foreach (var renderer in boneRenderer) {
            if (renderer != null) {
                renderer.bones = rootRenderer.bones;
            }
            //renderer.rootBone = _rootSkinnedRenderer.rootBone;
        }
    }

    public void AddObjectToQueue(GameObject newObject) {
        if (!gameObject.activeInHierarchy) {
            Debug.LogWarning("Cannot add object: GameObject is inactive");
            return;
        }

        HideRenderers(newObject);
        waitingObjects.Enqueue(newObject);

        // Если процесс не идет, начинаем его
        if (!isProcessing) {
            StartCoroutine(ProcessWaitingObjects());
        }
    }

    private IEnumerator ProcessWaitingObjects() {
        isProcessing = true;

        while (waitingObjects.Count > 0) {
            if (!gameObject.activeInHierarchy) {
                Debug.LogWarning("Cannot process objects: GameObject is inactive");
                break;
            }

            var currentObject = waitingObjects.Dequeue();
            yield return StartCoroutine(AddNewObjectWithWait(currentObject));

            // Небольшая задержка между добавлениями
            yield return new WaitForSeconds(0.1f);
        }

        isProcessing = false;
    }

    private IEnumerator AddNewObjectWithWait(GameObject newObject) {
        int currentCount = transform.parent.childCount;

        if (currentCount < 10) {
            newObject.transform.SetParent(transform.parent, false);

            bool success = false;
            yield return StartCoroutine(WaitForChildCountChangeCoroutine(8, result => success = result));

            if (success) {
                SyncMesh();
                ShowRenderers(newObject);
            } else {
                Debug.LogWarning("Не удалось дождаться изменения количества дочерних объектов");
                ShowRenderers(newObject);
            }
        } else {
            Debug.LogWarning("Достигнут лимит дочерних объектов (8). Невозможно добавить новый объект.");
            yield return false;
        }
    }

    private IEnumerator WaitForChildCountChangeCoroutine(int targetCount, System.Action<bool> callback) {
        if (!gameObject.activeInHierarchy) {
            callback?.Invoke(false);
            yield break;
        }

        float startTime = Time.time;
        float timeout = 0.3f;

        while (transform.parent.childCount > targetCount && Time.time - startTime < timeout) {
            if (!gameObject.activeInHierarchy) {
                callback?.Invoke(false);
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }

        callback?.Invoke(transform.parent.childCount == targetCount);
    }

    private void HideRenderers(GameObject obj) {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers) {
            renderer.enabled = false;
            temporarilyHiddenRenderers.Add(renderer);
        }
    }

    private void ShowRenderers(GameObject obj) {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers) {
            if (temporarilyHiddenRenderers.Remove(renderer)) {
                renderer.enabled = true;
            }
        }
    }

    public void PrintChildrenNames() {
        if (transform.parent.childCount == 0) {
            Debug.Log("No children found");
            return;
        }

        Debug.Log($"Found {transform.parent.childCount} children:");
        for (int i = 0; i < transform.parent.childCount; i++) {
            Transform child = transform.parent.GetChild(i);
            Debug.Log($"[{i}] {child.name}");
        }
    }

}
