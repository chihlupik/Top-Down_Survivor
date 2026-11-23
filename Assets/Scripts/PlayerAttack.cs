using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float baseFireRate = 1.2f;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float attackRange = 8f;

    private float fireCooldown;
    private float damage;
    private float fireRate;
    
    // НОВОЕ: Улучшения
    private int projectileCount = 1; // Количество пуль за выстрел
    private int pierceCount = 0;     // Сколько врагов пробивает пуля

    void Start()
    {
        damage = baseDamage;
        fireRate = baseFireRate;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Transform target = FindClosestEnemy();
            if (target != null)
            {
                Fire(target);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minDist = attackRange;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = e.transform;
            }
        }
        return closest;
    }

    void Fire(Transform target)
    {
        if (projectilePrefab == null) return;

        Vector2 baseDir = (target.position - transform.position).normalized;
        
        // Логика мультивыстрела (веер)
        float totalSpread = 15f * (projectileCount - 1); // Угол разброса
        float startAngle = -totalSpread / 2f;

        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = startAngle + (i * 15f);
            Vector2 dir = Quaternion.Euler(0, 0, currentAngle) * baseDir;

            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile p = proj.GetComponent<Projectile>();
            if (p != null)
                p.Init(dir, damage, pierceCount);
        }
    }

    // --- МЕТОДЫ УЛУЧШЕНИЙ ---
    public void IncreaseDamage(float amount) => damage += amount;
    public void IncreaseFireRate(float amount) => fireRate += amount;
    public void AddProjectile() => projectileCount++;
    public void AddPierce() => pierceCount++;
}