using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    //[SerializeField] private Transform projectileContainer;
    [SerializeField] public ProjectileData defaultSettings;

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

    public int LaunchProjectile(Vector3 startPos, Vector3 target, ProjectileData settings = null)
    {
        // Используем настройки по умолчанию, если не указаны другие
        ProjectileData currentSettings = settings ?? new ProjectileData(defaultSettings);

        // Создаем объект снаряда
        GameObject projectileObj = Instantiate(currentSettings.prefab, startPos, Quaternion.identity);

        int projectileId = nextId++;

        ProjectileData projectileData = new ProjectileData
        {
            id = projectileId,
            prefab = currentSettings.prefab,
            transform = projectileObj.transform,
            startPosition = startPos,
            targetPosition = target,
            speed = currentSettings.speed,
            damage = currentSettings.damage,
            lifetime = currentSettings.lifetime,
            speedCurve = currentSettings.speedCurve,
            startTime = Time.time,
            distance = Vector3.Distance(startPos, target),
            useGravity = currentSettings.useGravity,
            gravityScale = currentSettings.gravityScale,
            isActive = true
        };

        // Направляем снаряд к цели
        projectileObj.transform.LookAt(target);

        // Сохраняем данные
        activeProjectiles[projectileId] = projectileData;

        return projectileId;
    }

    public int LaunchProjectile(GameObject readyProjectile, Vector3 startPos , Vector3 target, ProjectileData settings = null)
    {
        // Используем настройки по умолчанию, если не указаны другие
        ProjectileData currentSettings = settings ?? new ProjectileData();

        // Проверяем, что стрела не null
        if (readyProjectile == null)
        {
            Debug.LogError("Ready projectile cannot be null!");
            return -1;
        }


        // Создаем данные снаряда
        int projectileId = nextId++;
        ProjectileData projectileData = new ProjectileData
        {
            id = projectileId,
            prefab = readyProjectile,  // Сохраняем ссылку на готовый объект
            transform = readyProjectile.transform,
            startPosition = startPos,
            targetPosition = target,
            speed = currentSettings.speed,
            damage = currentSettings.damage,
            lifetime = currentSettings.lifetime,
            speedCurve = currentSettings.speedCurve,
            startTime = Time.time,
            distance = Vector3.Distance(startPos, target),
            useGravity = currentSettings.useGravity,
            gravityScale = currentSettings.gravityScale,
            isActive = true
        };

        // Направляем снаряд к цели
        readyProjectile.transform.LookAt(target);

        // Сохраняем данные
        activeProjectiles[projectileId] = projectileData;

        return projectileId;
    }


    public void StopProjectile(int projectileId)
    {
        if (activeProjectiles.TryGetValue(projectileId, out ProjectileData projectile))
        {
            projectile.isActive = false;
            if (projectile.prefab != null)
            {
                Destroy(projectile.prefab);
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

        // Удаляем снаряды, которые завершили свой полет
        foreach (int projectileId in projectilesToRemove)
        {
            if (activeProjectiles.TryGetValue(projectileId, out ProjectileData projectile))
            {
                if (projectile.prefab != null)
                {
                    Destroy(projectile.prefab);
                }
                activeProjectiles.Remove(projectileId);
            }
        }
    }

    private bool UpdateProjectile(ProjectileData projectile)
    {
        if (!projectile.isActive) return false;

        float journeyLength = projectile.distance;
        float distCovered = (Time.time - projectile.startTime) * projectile.speed;
        float fractionOfJourney = distCovered / journeyLength;

        // Получаем текущую скорость из кривой
        float currentSpeed = projectile.speedCurve.Evaluate(fractionOfJourney);

        // Двигаем снаряд
        projectile.transform.position = Vector3.Lerp(
            projectile.startPosition,
            projectile.targetPosition,
            fractionOfJourney
        );

        // Проверяем попадание
        if (fractionOfJourney >= 1f)
        {
            OnHit(projectile);
            return false;
        }

        // Проверяем время жизни
        if (Time.time - projectile.startTime > projectile.lifetime)
        {
            OnLifetimeExpired(projectile);
            return false;
        }

        return true;
    }

    private void OnHit(ProjectileData projectile)
    {
        // Наносим урон цели
        // TODO: Реализовать систему урона

        // Создаем эффект попадания
        // TODO: Добавить эффекты

        // Уничтожаем снаряд
        if (projectile.prefab != null)
        {
            Destroy(projectile.prefab);
        }
    }

    private void OnLifetimeExpired(ProjectileData projectile)
    {
        // Создаем эффект исчезновения
        // TODO: Добавить эффекты

        // Уничтожаем снаряд
        if (projectile.prefab != null)
        {
            Destroy(projectile.prefab);
        }
    }
}
