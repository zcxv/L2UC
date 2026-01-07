using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;


public class SwordCollisionService : MonoBehaviour
{
    public static SwordCollisionService Instance { get; private set; }

    public LayerMask _entityMask;
    private List<TrackedSword> activeSwords = new List<TrackedSword>();
    private Dictionary<Transform, Vector3> lastPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, HashSet<int>> hitRegistry = new Dictionary<Transform, HashSet<int>>();

    //public event Action<RaycastHit, Transform, Transform> OnHitCollider;

    public event Action<Transform, Transform, Vector3, Vector3> OnHitCollider;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        _entityMask = LayerMask.GetMask("EntityClick");
    }


    public void RegisterSword(Transform swordBase, Transform swordTip , Transform target , float extraRange)
    {
        if (swordBase == null || swordTip == null) return;


        if (activeSwords.Exists(s => s.basePt == swordBase))
        {
            ResetHitRegistry(swordBase);
            return;
        }


        //Debug.Log("SwordCollisionManager: RegisterSword");
        activeSwords.Add(new TrackedSword(swordBase, swordTip , target ,  extraRange));
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
        //Debug.Log("SwordCollisionManager: Удаление RegisterSword");
        if (sword != null)
        {
            activeSwords.Remove(sword);
            lastPositions.Remove(sword.basePt);
            lastPositions.Remove(sword.tipPt);
            hitRegistry.Remove(swordBase);
        }
    }

    void LateUpdate()
    {
        if (activeSwords.Count == 0) return;

        for (int i = activeSwords.Count - 1; i >= 0; i--)
        {
            var sword = activeSwords[i];
            if (sword.basePt == null || sword.tipPt == null) continue;

            Vector3 currentBase = sword.basePt.position;
            Vector3 currentTip = sword.tipPt.position;

  
            if (lastPositions.ContainsKey(sword.basePt) && lastPositions.ContainsKey(sword.tipPt))
            {
                Vector3 prevBase = lastPositions[sword.basePt];
                Vector3 prevTip = lastPositions[sword.tipPt];

                // ГЛАВНЫЙ МЕТОД: Проверка "полотна" взмаха
                CheckSwordSwingPanel(prevBase, prevTip, currentBase, currentTip, sword);
            }

      
            lastPositions[sword.basePt] = currentBase;
            lastPositions[sword.tipPt] = currentTip;
        }
    }


    private void CheckSwordSwingPanel(Vector3 prevBase, Vector3 prevTip, Vector3 currBase, Vector3 currTip, TrackedSword sword)
    {
        int rayCount = 6;
        float swordThickness = 0.5f; // Радиус сферы (толщина лезвия)
        float forwardReach = 0.4f;   // На сколько ПЕРЕД мечом мы ищем цель (те самые 20-30 см)

        for (int i = 0; i <= rayCount; i++)
        {
            float t = (float)i / rayCount;

            Vector3 start = Vector3.Lerp(prevBase, prevTip, t);
            Vector3 end = Vector3.Lerp(currBase, currTip, t);

            // Направление движения этой точки меча
            Vector3 movementDir = (end - start).normalized;
            // Расстояние, которое прошла точка + запас вперед
            float dist = Vector3.Distance(start, end) + forwardReach;

            if (dist > 0.01f)
            {
               
                if (Physics.SphereCast(start, swordThickness, movementDir, out RaycastHit hit, dist, _entityMask))
                {
                    if (RegisterHit(sword.basePt, hit.collider.GetInstanceID()))
                    {
                        OnHit(hit, sword);
                        //OnHitCollider?.Invoke(hit, sword.basePt.parent, sword.tipPt);
                        Debug.Log("SwordCollisionService: HIT Monster!");
                        //DebugLineDraw.ShowDrawLineDebugNpc(sword.basePt.GetInstanceID(), start, hit.point, Color.red);
                    }
                }
                else
                {
                    // Рисуем желтую линию — это то, где мы искали
                   // DebugLineDraw.ShowDrawLineDebugNpc(sword.basePt.GetInstanceID(), start, start + movementDir * dist, Color.yellow);
                }
            }
        }
    }

    // Вспомогательный метод для реестра попаданий (чтобы один взмах не бил 100 раз по одной цели)
    private bool RegisterHit(Transform swordBase, int targetID)
    {
        if (!hitRegistry.ContainsKey(swordBase)) hitRegistry[swordBase] = new HashSet<int>();

        if (hitRegistry[swordBase].Contains(targetID)) return false;

        hitRegistry[swordBase].Add(targetID);
        return true;
    }




  
    private void OnHit(RaycastHit hit, TrackedSword sword)
    {
        GameObject gameObject = hit.transform.parent.gameObject;
        Entity entity = gameObject.GetComponent<Entity>();
        GameObject targetGameObject = sword.target.gameObject;
        Entity targetEntity = targetGameObject.GetComponent<Entity>();

        if (targetEntity == null || entity == null) return;

        if (entity.IdentityInterlude.Id == targetEntity.IdentityInterlude.Id)
        {
            Vector3 startPos = sword.basePt.position;

            var hitDirection = VectorUtils.CalcHitDirection(hit.point, startPos);

            OnHitCollider?.Invoke(sword.basePt.parent, sword.target, hit.point, hitDirection);
        }
    }


}
