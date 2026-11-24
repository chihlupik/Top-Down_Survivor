using UnityEngine;

public class EnemyShootingAI : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float stoppingDistance = 5f;

    [Header("Стрельба")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float damage = 10f;
    
    private Transform player;
    private float fireCooldown = 0f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        Vector3 dir = (player.position - transform.position).normalized;

        // === ЛОГИКА ЗЕРКАЛИРОВАНИЯ ЧЕРЕЗ SCALE ===
        if (dir.x < -0.01f) 
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (dir.x > 0.01f) 
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Движение, если игрок далеко
        if (distance > stoppingDistance)
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
        // Стрельба, если близко
        else
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = fireRate;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        
        // === РАНДОМНЫЙ УРОН (-3 до +3) ===
        float randomVariance = Random.Range(-3f, 3f);
        float finalDamage = damage + randomVariance;
        if (finalDamage < 1f) finalDamage = 1f;
        // ===================================

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
            p.Init(dir, finalDamage, 0, Projectile.ProjectileOwner.Enemy); 
    }
}