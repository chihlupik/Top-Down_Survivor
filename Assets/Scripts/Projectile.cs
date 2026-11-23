using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 2f;
    
    private float damage;
    private int pierceCount;
    private Vector2 direction;
    private List<GameObject> hitEnemies = new List<GameObject>(); // Чтобы не бить одного врага дважды

    public void Init(Vector2 dir, float dmg, int pierce)
    {
        direction = dir;
        damage = dmg;
        pierceCount = pierce;
        Destroy(gameObject, lifetime);
    }

    void Update() => transform.Translate(direction * speed * Time.deltaTime, Space.World);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !hitEnemies.Contains(other.gameObject))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                Debug.Log($"<color=cyan>[Пуля]</color> Наносит урон: {damage}");
                eh.TakeDamage(damage);
                hitEnemies.Add(other.gameObject);
            }

            // Если пуля не пробивает - уничтожаем
            if (pierceCount <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                pierceCount--; // Тратим заряд пробивания
            }
        }
    }
}