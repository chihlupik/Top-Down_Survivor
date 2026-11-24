using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public enum ProjectileOwner { Player, Enemy }

    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 2f;
    
    private float damage;
    private int pierceCount;
    private Vector2 direction;
    private ProjectileOwner ownerType; // Кто выстрелил
    private List<GameObject> hitEnemies = new List<GameObject>();

    public void Init(Vector2 dir, float dmg, int pierce, ProjectileOwner owner)
    {
        direction = dir;
        damage = dmg;
        pierceCount = pierce;
        ownerType = owner;
        Destroy(gameObject, lifetime);
    }

    void Update() => transform.Translate(direction * speed * Time.deltaTime, Space.World);

    void OnTriggerEnter2D(Collider2D other)
    {
        // ЛОГИКА ДЛЯ ПУЛИ ИГРОКА
        if (ownerType == ProjectileOwner.Player && other.CompareTag("Enemy") && !hitEnemies.Contains(other.gameObject))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(damage);
                hitEnemies.Add(other.gameObject);
            }

            if (pierceCount <= 0) Destroy(gameObject);
            else pierceCount--;
        }
        
        // ЛОГИКА ДЛЯ ПУЛИ ВРАГА
        else if (ownerType == ProjectileOwner.Enemy && other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
            Destroy(gameObject); // Пуля врага исчезает при попадании
        }
    }
}