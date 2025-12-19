using Org.BouncyCastle.Security;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static ChangeWaitTypePacket;

public class HitManager : MonoBehaviour
{
    public static HitManager Instance { get; private set; }
    private const string ETC_NAME = "etc_";



    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keep between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void HandleHit(GameObject source, Transform target , Vector3 hitPointCollider, Vector3 hitDirection)
    {
        
        string sourceNameLower = source.name.ToLower();
        string etcNameLower = ETC_NAME.ToLower();

        if (sourceNameLower.IndexOf(etcNameLower) > -1)
        {
            HandleArrowHit(source, target, hitPointCollider, hitDirection);
        }
        else
        {
            Debug.LogWarning("HitManager> HandleHit Errors no detected hit type");
        }
    }


    private void HandleArrowHit(GameObject arrow, Transform target, Vector3 hitPointCollider ,Vector3 hitDirection)
    {
        if (!target.CompareTag("Npc")) return;


        MonsterEntity entity = target.GetComponent<MonsterEntity>();

        if (entity == null) return;

        entity.AttachArrowToNearestBone(arrow, hitPointCollider , target, hitDirection);

        // Disable physics
        Collider arrowCollider = arrow.GetComponent<Collider>();
        if (arrowCollider != null)
        {
            arrowCollider.enabled = false;
        }
    }



}



public enum HitType
{
    Projectile,
    Melee
}
