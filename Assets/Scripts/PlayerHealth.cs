using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 100f;
    private float currentHP;
    
    public event Action<float, float> OnHealthChanged;
    public event Action OnDied;

    void Awake()
    {
        currentHP = maxHP;
    }
    
    void Start()
    {
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(float damage)
    {
        if (currentHP <= 0f) return;

        currentHP -= damage;

        // ВЫЗОВ ЦИФРЫ УРОНА
        DamagePopup.Create(transform.position, damage, true);

        // ЗВУК ПОЛУЧЕНИЯ УРОНА ИГРОКОМ
        if (currentHP > 0f) 
        {
            AudioManager.Instance?.PlaySFX("PlayerHit");
        }

        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0f)
        {
            currentHP = 0f;
            OnDied?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void Heal(float amount)
    {
        if (currentHP <= 0f) return; 
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void IncreaseMaxHP(float amount)
    {
        maxHP += amount;
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public bool IsFullHealth()
    {
        return currentHP >= maxHP;
    }
}