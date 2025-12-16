using UnityEngine;

public class ProjectileData
{
    public ProjectileData()
    {

        id = 0;
        prefab = null;
        transform = null;
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
        speed = 10f;
        damage = 10f;
        lifetime = 5f;
        speedCurve = AnimationCurve.Linear(0, 1, 1, 1);
        startTime = 0f;
        distance = 0f;
        useGravity = false;
        gravityScale = 1f;
        isActive = true;
    }

    public ProjectileData(GameObject prefabGo , Transform transform1 , Vector3 startPos , Vector3 endPos)
    {
        id = 0;
        prefab = prefabGo;
        transform = transform1; 
        startPosition = startPos;
        targetPosition = endPos;
        speed = 10f;
        damage = 10f;
        lifetime = 5f;
        speedCurve = AnimationCurve.Linear(0, 1, 1, 1);
        startTime = 0f;
        distance = 0f;
        useGravity = false;
        gravityScale = 1f;
        isActive = true;
    }

    public ProjectileData(ProjectileData other)
    {
        if (other != null)
        {
            id = other.id;
            prefab = other.prefab;
            transform = other.transform;
            startPosition = other.startPosition;
            targetPosition = other.targetPosition;
            speed = other.speed;
            damage = other.damage;
            lifetime = other.lifetime;
            speedCurve = other.speedCurve;
            startTime = other.startTime;
            distance = other.distance;
            useGravity = other.useGravity;
            gravityScale = other.gravityScale;
            isActive = other.isActive;
        }

    }

    public int id;
    public GameObject prefab;
    public Transform transform;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public float speed;
    public float damage;
    public float lifetime;
    public AnimationCurve speedCurve;
    public float startTime;
    public float distance;
    public bool useGravity;
    public float gravityScale;
    public bool isActive;
}
