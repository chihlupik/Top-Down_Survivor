using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float contactDamage = 8f;
    [SerializeField] private float damageCooldown = 0.8f;

    private Transform player;
    private float lastDamageTime;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        
        // Движение к игроку
        transform.position += dir * moveSpeed * Time.deltaTime;

        // === ЛОГИКА ЗЕРКАЛИРОВАНИЯ ЧЕРЕЗ SCALE ===
        if (dir.x < -0.01f) 
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (dir.x > 0.01f) 
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Если игрок рядом и кулдаун прошел
        if (collision.gameObject.CompareTag("Player") && Time.time - lastDamageTime > damageCooldown)
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                // === РАНДОМНЫЙ УРОН (-3 до +3) ===
                float randomVariance = Random.Range(-3f, 3f);
                float finalDamage = contactDamage + randomVariance;
                
                // Защита, чтобы урон не стал отрицательным
                if (finalDamage < 1f) finalDamage = 1f; 
                // =================================

                ph.TakeDamage(finalDamage);
                lastDamageTime = Time.time;
                
                // (Опционально) Тут можно добавить звук удара
                AudioManager.Instance?.PlaySFX("PlayerHit"); 
            }
        }
    }
}