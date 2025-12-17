using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] public ProjectileData defaultSettings;

    // Константы для расчета скорости
    private const float SPEED_RANGE_1 = 8f;    // Скорость до 4 метров
    private const float SPEED_RANGE_2_MAX = 11f; // Максимальная скорость во втором диапазоне
    private const float SPEED_RANGE_3_MAX = 12f; // Максимальная скорость в третьем диапазоне

    private const float DISTANCE_SPLIT_1 = 4f;  // Первая точка разделения дистанции
    private const float DISTANCE_SPLIT_2 = 8f;  // Вторая точка разделения дистанции
    private const float DISTANCE_SPLIT_3 = 12f; // Третья точка разделения дистанции

    private Dictionary<int, ProjectileData> activeProjectiles = new Dictionary<int, ProjectileData>();
    private int nextId = 0;

    #region Singleton
    public static ProjectileManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion
    private const float HIT_OFFSET = 0.3f; //  раннее попадание так же нужно измениь значение в CalcBaseParam->CalculateAttackAndFlightTimes
    private float CalculateFlightTime(float distance)
    {
        // Определяем скорость в зависимости от дистанции
        float speed;
        if (distance <= DISTANCE_SPLIT_1)
        {
            speed = SPEED_RANGE_1;
        }
        else if (distance <= DISTANCE_SPLIT_2)
        {
            // Пропорция от 8 до 11 м/с между 4 и 8 метрами
            speed = SPEED_RANGE_1 + (distance - DISTANCE_SPLIT_1) *
                   ((SPEED_RANGE_2_MAX - SPEED_RANGE_1) / (DISTANCE_SPLIT_2 - DISTANCE_SPLIT_1));
        }
        else if (distance <= DISTANCE_SPLIT_3)
        {
            // Пропорция от 11 до 12 м/с между 8 и 12 метрами
            speed = SPEED_RANGE_2_MAX + (distance - DISTANCE_SPLIT_2) *
                   ((SPEED_RANGE_3_MAX - SPEED_RANGE_2_MAX) / (DISTANCE_SPLIT_3 - DISTANCE_SPLIT_2));
        }
        else
        {
            speed = SPEED_RANGE_3_MAX;
        }

        float flightTime = (distance / speed);

        if (distance >= 4) return Mathf.Max(flightTime - HIT_OFFSET, 0.1f); 

        return Mathf.Max(flightTime, 0.1f); 
    }



    public int LaunchProjectile(Vector3 startPos, Transform target, ProjectileData settings = null)
    {
        ProjectileData currentSettings = settings ?? new ProjectileData(defaultSettings);
        GameObject projectileObj = Instantiate(currentSettings.prefab, startPos, Quaternion.identity);

        int projectileId = nextId++;

        // Рассчитываем дистанцию и время полета
        float distance = Vector3.Distance(startPos, target.position);
        float flightTime = CalculateFlightTime(distance);

        // Рассчитываем необходимую скорость
        float requiredSpeed = distance / flightTime;

        ProjectileData projectileData = new ProjectileData
        {
            id = projectileId,
            prefab = currentSettings.prefab,
            transform = projectileObj.transform,
            startPosition = startPos,
            targetTransform = target,
            speed = requiredSpeed,
            damage = currentSettings.damage,
            lifetime = currentSettings.lifetime,
            speedCurve = currentSettings.speedCurve,
            startTime = Time.time,
            distance = distance,
            useGravity = currentSettings.useGravity,
            gravityScale = currentSettings.gravityScale,
            isActive = true
        };

        // Направляем снаряд к цели
        projectileObj.transform.LookAt(target);

        activeProjectiles[projectileId] = projectileData;
        return projectileId;
    }

    public int LaunchProjectile(GameObject readyProjectile, Vector3 startPos, Transform target, ProjectileData settings = null)
    {
        if (readyProjectile == null)
        {
            Debug.LogError("Ready projectile cannot be null!");
            return -1;
        }

        ClearParentObject(readyProjectile);

        // Рассчитываем точку попадания
        Vector3 adjustedTarget = VectorUtils.GetCollision(startPos, target);

        // Рассчитываем дистанцию и время полета
        float distance = Vector3.Distance(startPos, adjustedTarget);
        float flightTime = CalculateFlightTime(distance);

        // Рассчитываем необходимую скорость
        float requiredSpeed = distance / flightTime;

        int projectileId = nextId++;
        ProjectileData projectileData = new ProjectileData
        {
            id = projectileId,
            prefab = readyProjectile,
            transform = readyProjectile.transform,
            startPosition = startPos,
            targetTransform = target,
            targetPosition = adjustedTarget,
            speed = requiredSpeed,
            damage = settings?.damage ?? defaultSettings.damage,
            lifetime = settings?.lifetime ?? defaultSettings.lifetime,
            speedCurve = settings?.speedCurve ?? defaultSettings.speedCurve,
            startTime = Time.time,
            distance = distance,
            useGravity = settings?.useGravity ?? defaultSettings.useGravity,
            gravityScale = settings?.gravityScale ?? defaultSettings.gravityScale,
            isActive = true
        };

        if(distance <= 4f)
        {
            projectileData.speedCurve = AnimationCurve.Linear(0, 1, 1, 1);
        }

        // Устанавливаем начальную позицию и вращение
        readyProjectile.transform.position = startPos;
        Vector3 direction = adjustedTarget - startPos;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation *= Quaternion.Euler(0, 90, 0);
        readyProjectile.transform.rotation = rotation;

        activeProjectiles[projectileId] = projectileData;
        return projectileId;
    }

    private void ClearParentObject(GameObject readyProjectile)
    {
        if (readyProjectile.transform.parent != null)
            readyProjectile.transform.SetParent(null);
    }

    public void StopProjectile(int projectileId)
    {
        if (activeProjectiles.TryGetValue(projectileId, out ProjectileData projectile))
        {
            projectile.isActive = false;
            if (projectile.prefab != null)
            {
                //Destroy(projectile.prefab);
            }
            activeProjectiles.Remove(projectileId);
        }
    }

    private void Update()
    {
        List<int> projectilesToRemove = new List<int>();

        foreach (var pair in activeProjectiles)
        {
            int projectileId = pair.Key;
            ProjectileData projectile = pair.Value;

            if (!projectile.isActive || !UpdateProjectile(projectile))
            {
                projectilesToRemove.Add(projectileId);
            }
        }

        foreach (int projectileId in projectilesToRemove)
        {
            if (activeProjectiles.TryGetValue(projectileId, out ProjectileData projectile))
            {
                if (projectile.prefab != null)
                {
                    //Destroy(projectile.prefab);
                }
                activeProjectiles.Remove(projectileId);
            }
        }
    }

    private bool UpdateProjectile(ProjectileData projectile)
    {
        if (!projectile.isActive) return false;

        // Обновляем позицию цели и точку попадания
        if (projectile.targetTransform != null)
        {
            projectile.targetPosition = VectorUtils.GetCollision(projectile.transform.position, projectile.targetTransform);
        }

        // Добавляем offset к времени полета для более медленного полета
        float flightTime = CalculateFlightTime(projectile.distance);

        // Рассчитываем прогресс полета на основе увеличенного времени
        float fractionOfJourney = (Time.time - projectile.startTime) / flightTime;

        // Проверяем попадание
        if (fractionOfJourney >= 1f)
        {
            OnHit(projectile);
            return false;
        }

        // Получаем текущую скорость из кривой
        float currentSpeed = projectile.speedCurve.Evaluate(fractionOfJourney);
        Debug.Log("requiredSpeed " + currentSpeed);
        // Двигаем снаряд с учетом более медленного полета
        projectile.transform.position = Vector3.Lerp(
            projectile.startPosition,
            projectile.targetPosition,
            fractionOfJourney
        );

        // Обновляем вращение стрелы
        if (projectile.targetTransform != null)
        {
            Vector3 direction = projectile.targetPosition - projectile.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(0, 90, 0);
            projectile.transform.rotation = rotation;
        }

        // Проверяем время жизни
        //if (Time.time - projectile.startTime > projectile.lifetime)
        //{
        //    OnLifetimeExpired(projectile);
        //    return false;
        //}

        return true;
    }


    private void OnHit(ProjectileData projectile)
    {
        // Наносим урон цели
        if (projectile.targetTransform != null)
        {
            Entity targetEntity = projectile.targetTransform.GetComponent<Entity>();
            if (targetEntity != null)
            {
                //targetEntity.TakeDamage(projectile.damage);
            }
        }

        // Создаем эффект попадания
        // TODO: Добавить эффекты

        // Уничтожаем снаряд
        if (projectile.prefab != null)
        {
            //Destroy(projectile.prefab);
        }
    }

    private void OnLifetimeExpired(ProjectileData projectile)
    {
        // Создаем эффект исчезновения
        // TODO: Добавить эффекты

        // Уничтожаем снаряд
        if (projectile.prefab != null)
        {
            //Destroy(projectile.prefab);
        }
    }
}
