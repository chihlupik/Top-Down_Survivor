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
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        OnHealthChanged?.Invoke(currentHP, maxHP);
        if (currentHP <= 0f)
        {
            currentHP = 0f;
                    Debug.Log("💀 [Игрок] HP = 0. Вызываю OnDied!");
            OnDied?.Invoke();
        }
            currentHP -= damage;
    
    // === ДЕБАГ ===
    Debug.Log($"<color=red>[Игрок]</color> Получил {damage} урона. Осталось HP: {currentHP} / {maxHP}");
    // =============

    OnHealthChanged?.Invoke(currentHP, maxHP);
    
    if (currentHP <= 0f)
    {
        currentHP = 0f;
        OnDied?.Invoke();
    }
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        OnHealthChanged?.Invoke(currentHP, maxHP);
            currentHP = Mathf.Min(currentHP + amount, maxHP);
    
    // === ДЕБАГ ===
    Debug.Log($"<color=green>[Игрок]</color> Вылечился на {amount}. Текущее HP: {currentHP} / {maxHP}");
    // =============
    
    OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void IncreaseMaxHP(float amount)
    {
        maxHP += amount;
        currentHP += amount;
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }
}