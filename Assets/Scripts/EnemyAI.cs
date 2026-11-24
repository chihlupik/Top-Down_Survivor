using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float contactDamage = 8f;
    [SerializeField] private float damageCooldown = 0.8f;

    private Transform player;
    private float lastDamageTime;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        
        // Ищем картинку врага (на нем самом или на дочернем объекте)
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        
        // Движение к игроку
        transform.position += dir * moveSpeed * Time.deltaTime;

        // === ЛОГИКА ЗЕРКАЛИРОВАНИЯ ===
        if (spriteRenderer != null)
        {
            if (dir.x < -0.01f) spriteRenderer.flipX = true;  // Если игрок слева - смотрит влево
            else if (dir.x > 0.01f) spriteRenderer.flipX = false; // Если игрок справа - смотрит вправо
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time - lastDamageTime > damageCooldown)
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(contactDamage);
            lastDamageTime = Time.time;
        }
    }
}