using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;


public class SwordCollisionService : MonoBehaviour
{
    public static SwordCollisionService Instance { get; private set; }

    public LayerMask _entityMask;
    private List<TrackedSword> activeSwords = new List<TrackedSword>();
    private Dictionary<Transform, Vector3> lastPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, HashSet<int>> hitRegistry = new Dictionary<Transform, HashSet<int>>();

    public event Action<RaycastHit, Transform, Transform> OnHitDetected;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        _entityMask = LayerMask.GetMask("EntityClick");
    }


    public void RegisterSword(Transform swordBase, Transform swordTip , float extraRange)
    {
        if (swordBase == null || swordTip == null) return;


        if (activeSwords.Exists(s => s.basePt == swordBase)) return;

        Debug.Log("SwordCollisionManager: RegisterSword");
        activeSwords.Add(new TrackedSword(swordBase, swordTip , extraRange));
        lastPositions[swordBase] = swordBase.position;
        lastPositions[swordTip] = swordTip.position;
        ResetHitRegistry(swordBase);
    }


    public void ResetHitRegistry(Transform swordBase)
    {
        if (!hitRegistry.ContainsKey(swordBase)) hitRegistry[swordBase] = new HashSet<int>();
        hitRegistry[swordBase].Clear();
    }

    public void UnregisterSword(Transform swordBase)
    {
        TrackedSword sword = activeSwords.Find(s => s.basePt == swordBase);
        Debug.Log("SwordCollisionManager: Удаление RegisterSword");
        if (sword != null)
        {
            activeSwords.Remove(sword);
            lastPositions.Remove(sword.basePt);
            lastPositions.Remove(sword.tipPt);
        }
    }

    void FixedUpdate()
    {
        for (int i = activeSwords.Count - 1; i >= 0; i--)
        {
            var sword = activeSwords[i];

            if (sword.basePt == null || sword.tipPt == null) continue;

            // Берем чистые позиции БЕЗ добавок
            Vector3 currentBase = sword.basePt.position;
            Vector3 currentTip = sword.tipPt.position;

            Vector3 dirBlade = (currentTip - currentBase).normalized;

            float offset = 1.2f; 
            Vector3 currentExtendedTip = currentTip + (dirBlade * offset);

            if (lastPositions.ContainsKey(sword.tipPt))
            {
                Vector3 lastExtendedTip = lastPositions[sword.tipPt];

                // Проверяем траекторию именно этой вынесенной вперед точки
                CheckVolumePath(lastExtendedTip, currentExtendedTip, sword , 0.6f);
            }

            // Сохраняем реальные позиции
            lastPositions[sword.basePt] = currentBase;
            lastPositions[sword.tipPt] = currentTip;

            // А extraRange используйте ТОЛЬКО внутри CheckBlade для "укола" вперед
            CheckBladeVolume(currentBase, currentTip, sword , 0.6f); // Здесь передаем бонус длины
        }
    }


    private void CheckVolumePath(Vector3 start, Vector3 end, TrackedSword sword, float radius)
    {
        Vector3 dir = end - start;
        float dist = dir.magnitude;

        if (dist > 0.001f)
        {
        
            if (Physics.SphereCast(start, radius, dir.normalized, out RaycastHit hit, dist, _entityMask))
            {
                Debug.Log("Volume Hit! 1");
                OnHitDetected?.Invoke(hit, sword.basePt.parent, sword.tipPt);
                DebugLineDraw.ShowDrawLineDebugNpc(sword.basePt.GetInstanceID(), start, hit.point, Color.magenta);
            }
            else
            {
                DebugLineDraw.ShowDrawLineDebugNpc(sword.basePt.GetInstanceID(), start, end, Color.yellow);
            }
        }
    }

    private void CheckBladeVolume(Vector3 start, Vector3 end, TrackedSword sword, float radius)
    {
        Vector3 dir = end - start;
        // Стреляем сферой от рукояти к кончику
        if (Physics.SphereCast(start, radius, dir.normalized, out RaycastHit hit, dir.magnitude, _entityMask))
        {
            //Debug.Log("Volume Hit! 2");
            Debug.Log("Volume Hit! 1");
            OnHitDetected?.Invoke(hit, sword.basePt.parent, sword.tipPt);
            DebugLineDraw.ShowDrawLineDebugNpc(sword.basePt.GetInstanceID(), start, hit.point, Color.red);
        }
    }

    
    private void CheckPointPath(Transform point)
    {
        Vector3 currentPos = point.position;
        Vector3 lastPos = lastPositions[point];
        Vector3 dir = currentPos - lastPos;
        float dist = dir.magnitude;

        if (dist > 0.001f)
        {
            if (Physics.Raycast(lastPos, dir, out RaycastHit hit, dist, _entityMask))
            {
                Debug.Log("SwordCollisionManager 1: Hit detected");
                OnHitDetected?.Invoke(hit, point.parent, point);
            }
        }
        lastPositions[point] = currentPos;
    }

    private void CheckBlade(Transform b, Transform t)
    {
        Vector3 start = b.position;  // Handle position
        Vector3 end = t.position;    // Tip position

        // Calculate direction from handle to tip
        Vector3 direction = end - start;
        Vector3 directionNormalized = direction.normalized;
        float baseLength = direction.magnitude;

        // Добавляем бонусную дистанцию (range) к проверке
        float extraRange = 0.9f;

        // The ray should go from handle through tip and beyond
        // So we extend from the tip in the same direction
        Vector3 extendedEnd = end + directionNormalized * extraRange;
        float totalCheckDistance = baseLength + extraRange;
        // Draw the complete raycast path from handle through tip and beyond
        //DebugLineDraw.ShowDrawLineDebugNpc(b.GetInstanceID(), start, extendedEnd, Color.red);

        // Now cast the ray from handle through tip and beyond
        if (Physics.Raycast(start, directionNormalized, out RaycastHit hit, totalCheckDistance, _entityMask))
        {
            // Log detailed information about the hit
            //Debug.Log($"SwordCollisionManager 2: Hit detected at point: {hit.point}");
            //Debug.Log($"Hit distance: {hit.distance}");
            //Debug.Log($"Hit normal: {hit.normal}");
            //Debug.Log($"Hit collider: {hit.collider.name}");
            //Debug.Log($"Hit transform: {hit.transform.name}");

            // Draw the hit point
           // DebugLineDraw.ShowDrawLineDebugNpc(
            //    b.GetInstanceID(),
            //    hit.point,
            //    hit.point + Vector3.up * 0.2f,
           //     Color.green
           // );

            // Draw the actual raycast path
            DebugLineDraw.ShowDrawLineDebugNpc(
                b.GetInstanceID(),
                start,
                hit.point,
                Color.red
            );
        }
        else
        {
            //Debug.Log("SwordCollisionManager 2: No hit detected");
            //DebugLineDraw.ShowDrawLineDebugNpc(b.GetInstanceID(), start, extendedEnd, Color.blue);
        }
    }


}
