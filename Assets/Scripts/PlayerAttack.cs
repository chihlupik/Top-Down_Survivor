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
    
    // Улучшения
    private int projectileCount = 1; 
    private int pierceCount = 0;     

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
        
        // Логика мультивыстрела (разброс веером)
        float totalSpread = 15f * (projectileCount - 1); 
        float startAngle = -totalSpread / 2f;

        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = startAngle + (i * 15f);
            Vector2 dir = Quaternion.Euler(0, 0, currentAngle) * baseDir;

            // === РАНДОМНЫЙ УРОН (-3 до +3) ===
            float randomVariance = Random.Range(-3f, 3f);
            float finalDamage = damage + randomVariance;
            
            // Защита от отрицательного урона
            if (finalDamage < 1f) finalDamage = 1f; 
            // ==================================

            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile p = proj.GetComponent<Projectile>();
            
            if (p != null)
            {
                // Передаем финальный (рандомный) урон в пулю
                p.Init(dir, finalDamage, pierceCount, Projectile.ProjectileOwner.Player);
            }
        }
    }

    // --- МЕТОДЫ УЛУЧШЕНИЙ ---
    public void IncreaseDamage(float amount) => damage += amount;
    public void IncreaseFireRate(float amount) => fireRate += amount;
    public void AddProjectile() => projectileCount++;
    public void AddPierce() => pierceCount++;
    public void SetProjectileCount(int count) => projectileCount = count; 
}