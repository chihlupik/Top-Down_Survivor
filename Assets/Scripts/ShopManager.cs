using UnityEngine;
using UnityEngine.UI;
using TMPro; // Добавлено для работы с текстом

public class ShopManager : MonoBehaviour
{
    [Header("Кнопки покупки")]
    [SerializeField] private Button buyHealButton;
    [SerializeField] private Button buySpeedButton;

    [Header("Тексты на кнопках (для отображения КД)")]
    [SerializeField] private TextMeshProUGUI healButtonText;
    [SerializeField] private TextMeshProUGUI speedButtonText;

    [Header("Настройки Лечения (Heal)")]
    [SerializeField] private int healCost = 50;
    [SerializeField] private float healAmount = 30f;
    [SerializeField] private float healCooldown = 5f;

    [Header("Настройки Ускорения (Speed)")]
    [SerializeField] private int speedCost = 100;
    [SerializeField] private float speedDuration = 5f;
    [SerializeField] private float speedMultiplier = 1.5f; 
    [SerializeField] private float speedCooldown = 10f;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;

    private float healTimer = 0f;
    private float speedTimer = 0f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            playerHealth = p.GetComponent<PlayerHealth>();
            playerMovement = p.GetComponent<PlayerMovement>();
        }

        if (buyHealButton != null) buyHealButton.onClick.AddListener(BuyHeal);
        if (buySpeedButton != null) buySpeedButton.onClick.AddListener(BuySpeed);
    }

    void Update()
    {
        // Тикаем таймеры
        if (healTimer > 0) healTimer -= Time.deltaTime;
        if (speedTimer > 0) speedTimer -= Time.deltaTime;

        // ОБНОВЛЯЕМ ТЕКСТ НА КНОПКАХ, ЧТОБЫ ИГРОК ВИДЕЛ КД
        UpdateButtonTexts();
    }

    void UpdateButtonTexts()
    {
        // Текст кнопки лечения
        if (healButtonText != null)
        {
            if (healTimer > 0)
                healButtonText.text = $"KD {Mathf.CeilToInt(healTimer)}"; // Показываем таймер
            else
                healButtonText.text = $"HEAL {healCost}"; // Обычный текст
        }

        // Текст кнопки ускорения
        if (speedButtonText != null)
        {
            if (speedTimer > 0)
                speedButtonText.text = $"KD {Mathf.CeilToInt(speedTimer)}";
            else
                speedButtonText.text = $"SPEED {speedCost}";
        }
    }

    void BuyHeal()
    {
        if (GameManager.Instance == null || playerHealth == null) return;

        // Если идет кулдаун — показываем уведомление и выходим
        if (healTimer > 0)
        {
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification($"ХИЛКА БУДЕТ ЧЕРЕЗ {Mathf.CeilToInt(healTimer)} СЕК!", Color.red);
            return;
        }

        if (playerHealth.IsFullHealth())
        {
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification("ЗДОРОВЬЕ УЖЕ МАКСИМАЛЬНОЕ!", Color.red);
            return;
        }

        if (GameManager.Instance.SpendCoins(healCost))
        {
            playerHealth.Heal(healAmount);
            healTimer = healCooldown; 
            
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification($"КУПЛЕНО: +{healAmount} HP!", Color.green);
        }
        else
        {
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification("НЕ ХВАТАЕТ МОНЕТ!", Color.red);
        }
    }

    void BuySpeed()
    {
        if (GameManager.Instance == null || playerMovement == null) return;

        if (speedTimer > 0)
        {
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification($"УСКОРЕНИЕ БУДЕТ ЧЕРЕЗ {Mathf.CeilToInt(speedTimer)} СЕК!", Color.red);
            return;
        }

        if (GameManager.Instance.SpendCoins(speedCost))
        {
            playerMovement.ApplyTemporarySpeedBoost(speedDuration, speedMultiplier);
            speedTimer = speedCooldown;
            
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification("КУПЛЕНО: УСКОРЕНИЕ!", Color.cyan);
        }
        else
        {
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification("НЕ ХВАТАЕТ МОНЕТ!", Color.red);
        }
    }
}