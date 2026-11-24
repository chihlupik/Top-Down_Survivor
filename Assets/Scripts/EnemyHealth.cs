using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 30f;
    
    [Header("Награда за убийство")]
    [SerializeField] private int scoreValue = 10; 
    [SerializeField] private int coinValue = 5;   

    private float currentHP;

    void Awake() => currentHP = maxHP;

    public void TakeDamage(float damage)
    {
        if (currentHP <= 0f) return;
        
        currentHP -= damage;

        // ВЫЗОВ ЦИФРЫ УРОНА
        DamagePopup.Create(transform.position, damage, false);

        // ЗВУК ПОЛУЧЕНИЯ УРОНА ВРАГОМ
        if (currentHP > 0f) 
        {
            AudioManager.Instance?.PlaySFX("EnemyHit");
        }

        if (currentHP <= 0f) Die();
    }

    void Die()
    {
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