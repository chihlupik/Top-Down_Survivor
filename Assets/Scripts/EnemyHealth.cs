using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 30f;
    
    [Header("Награда за убийство")]
    [SerializeField] private int scoreValue = 10; // Сколько очков дает
    [SerializeField] private int coinValue = 5;    // Сколько монет дает

    private float currentHP;

    void Awake() => currentHP = maxHP;

    public void TakeDamage(float damage)
    {
        if (currentHP <= 0f) return;
        
        currentHP -= damage;
        Debug.Log($"<color=orange>[{gameObject.name}]</color> Получил {damage} урона. Осталось HP: {currentHP} / {maxHP}");

        if (currentHP <= 0f) Die();
    }

    void Die()
    {
        Debug.Log($"<color=yellow>[{gameObject.name}] Умер! Дает {scoreValue} очков и {coinValue} монет.</color>");
        
        // Начисляем награду
        GameManager.Instance?.AddScore(scoreValue);
        GameManager.Instance?.AddCoins(coinValue);
        
        AudioManager.Instance?.PlaySFX("EnemyDeath");
        Destroy(gameObject);
    }

    public void SetMaxHP(float hp)
    {
        maxHP = hp;
        currentHP = hp;
    }
}