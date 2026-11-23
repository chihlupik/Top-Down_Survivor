using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button openShopButton;
    [SerializeField] private Button closeShopButton;
    [SerializeField] private Button buyHealButton;
    [SerializeField] private TextMeshProUGUI shopCoinsText; // Текст монет внутри магазина

    [Header("Настройки")]
    [SerializeField] private int healCost = 50;
    [SerializeField] private float healAmount = 30f;

    private PlayerHealth playerHealth;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerHealth = p.GetComponent<PlayerHealth>();

        // Привязка кнопок
        if (openShopButton != null) openShopButton.onClick.AddListener(OpenShop);
        if (closeShopButton != null) closeShopButton.onClick.AddListener(CloseShop);
        if (buyHealButton != null) buyHealButton.onClick.AddListener(BuyHeal);

        if (shopPanel != null) shopPanel.SetActive(false);
    }

    void OpenShop()
    {
        if (shopPanel == null) return;
        
        shopPanel.SetActive(true);
        
        // Прячем кнопку открытия, показываем кнопку закрытия
        if (openShopButton != null) openShopButton.gameObject.SetActive(false);
        if (closeShopButton != null) closeShopButton.gameObject.SetActive(true);

        UpdateCoinsUI();
        Time.timeScale = 0f; // Пауза
    }

    void CloseShop()
    {
        if (shopPanel == null) return;
        
        shopPanel.SetActive(false);
        
        // Прячем кнопку закрытия, показываем кнопку открытия
        if (closeShopButton != null) closeShopButton.gameObject.SetActive(false);
        if (openShopButton != null) openShopButton.gameObject.SetActive(true);

        Time.timeScale = 1f; // Снимаем с паузы
    }

    void BuyHeal()
    {
        if (GameManager.Instance == null || playerHealth == null) return;

        // Пытаемся списать монеты
        if (GameManager.Instance.SpendCoins(healCost))
        {
            playerHealth.Heal(healAmount);
            AudioManager.Instance?.PlaySFX("UpgradePick");
            UpdateCoinsUI();
            Debug.Log($"<color=green>Куплено лечение на {healAmount} HP!</color>");
        }
        else
        {
            Debug.Log("<color=red>Не хватает монет на лечение!</color>");
        }
    }

    void UpdateCoinsUI()
    {
        if (shopCoinsText != null && GameManager.Instance != null)
        {
            shopCoinsText.text = $"Ваши монеты: {GameManager.Instance.GetCoins()}";
        }
    }
}