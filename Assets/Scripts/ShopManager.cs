using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private Button buyHealButton;

    [Header("Настройки")]
    [SerializeField] private int healCost = 50;
    [SerializeField] private float healAmount = 30f;

    private PlayerHealth playerHealth;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerHealth = p.GetComponent<PlayerHealth>();

        if (buyHealButton != null) buyHealButton.onClick.AddListener(BuyHeal);
    }

    void BuyHeal()
    {
        if (GameManager.Instance == null || playerHealth == null) return;

        // ПРОВЕРКА НА МАКСИМАЛЬНОЕ ЗДОРОВЬЕ
        if (playerHealth.IsFullHealth())
        {
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification("ЗДОРОВЬЕ УЖЕ МАКСИМАЛЬНОЕ!", Color.red);
            return;
        }

        if (GameManager.Instance.SpendCoins(healCost))
        {
            playerHealth.Heal(healAmount);
            AudioManager.Instance?.PlaySFX("UpgradePick");
            
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification($"КУПЛЕНО: +{healAmount} HP!", Color.green);
        }
        else
        {
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification("НЕ ХВАТАЕТ МОНЕТ!", Color.red);
        }
    }
}