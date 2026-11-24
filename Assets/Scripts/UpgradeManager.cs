using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    [Header("UI Панели")]
    [SerializeField] private GameObject upgradePanel;
    
    [Header("Кнопки улучшений")]
    [SerializeField] private Button damageButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button hpButton;
    [SerializeField] private Button fireRateButton;
    
    [Header("Таймер")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timeToChoose = 5f;

    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    private bool isPanelOpen = false;
    private float currentTimer;

    public bool IsPanelOpen => isPanelOpen;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            playerAttack = p.GetComponent<PlayerAttack>();
            playerMovement = p.GetComponent<PlayerMovement>();
            playerHealth = p.GetComponent<PlayerHealth>();
        }

        if (damageButton != null) damageButton.onClick.AddListener(() => ApplyUpgrade("damage"));
        if (speedButton != null) speedButton.onClick.AddListener(() => ApplyUpgrade("speed"));
        if (hpButton != null) hpButton.onClick.AddListener(() => ApplyUpgrade("hp"));
        if (fireRateButton != null) fireRateButton.onClick.AddListener(() => ApplyUpgrade("firerate"));

        if (upgradePanel != null) upgradePanel.SetActive(false);
    }

    void Update()
    {
        if (isPanelOpen)
        {
            currentTimer -= Time.deltaTime;
            
            if (timerText != null)
                timerText.text = Mathf.CeilToInt(currentTimer).ToString();

            if (currentTimer <= 0f)
            {
                PickRandomUpgrade();
            }
        }
    }

    public void ShowUpgradePanel()
    {
        if (upgradePanel == null) return;
        
        upgradePanel.SetActive(true);
        isPanelOpen = true;
        currentTimer = timeToChoose;
        
        AudioManager.Instance?.PlaySFX("UpgradeOpen");
    }

    void ApplyUpgrade(string type)
    {
        if (!isPanelOpen) return;

        string notifMessage = "";

        // Значения сделаны более плавными
        switch (type)
        {
            case "damage": 
                if (playerAttack != null) playerAttack.IncreaseDamage(3f); 
                notifMessage = "УЛУЧШЕНИЕ: +3 УРОН";
                break;
            case "speed": 
                if (playerMovement != null) playerMovement.IncreaseSpeed(0.5f); // +0.5 к скорости
                notifMessage = "УЛУЧШЕНИЕ: +0.5 СКОРОСТЬ";
                break;
            case "hp": 
                if (playerHealth != null) playerHealth.IncreaseMaxHP(15f);
                notifMessage = "УЛУЧШЕНИЕ: +15 МАКС. HP";
                break;
            case "firerate": 
                if (playerAttack != null) playerAttack.IncreaseFireRate(0.1f); // +0.1 к скорострельности
                notifMessage = "УЛУЧШЕНИЕ: +СКОРОСТРЕЛЬНОСТЬ";
                break;
        }

        if (!string.IsNullOrEmpty(notifMessage) && NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowNotification(notifMessage, Color.yellow);
        }

        ClosePanel();
    }

    void PickRandomUpgrade()
    {
        string[] types = { "damage", "speed", "hp", "firerate" };
        string randomType = types[Random.Range(0, types.Length)];
        ApplyUpgrade(randomType);
    }

    void ClosePanel()
    {
        AudioManager.Instance?.PlaySFX("UpgradePick");
        upgradePanel.SetActive(false);
        isPanelOpen = false;
    }
}