using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button hpButton;
    [SerializeField] private Button fireRateButton;

    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

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

    public void ShowUpgradePanel()
    {
        if (upgradePanel == null) return;
        
        upgradePanel.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.Instance?.PlaySFX("UpgradeOpen");
    }

    void ApplyUpgrade(string type)
    {
        switch (type)
        {
            case "damage": 
                if (playerAttack != null) playerAttack.IncreaseDamage(5f); 
                break;
            case "speed": 
                if (playerMovement != null) playerMovement.SetSpeed(6f); 
                break;
            case "hp": 
                if (playerHealth != null) 
                {
                    playerHealth.IncreaseMaxHP(20f);
                    playerHealth.Heal(20f);
                }
                break;
            case "firerate": 
                if (playerAttack != null) playerAttack.IncreaseFireRate(0.3f); 
                break;
        }

        AudioManager.Instance?.PlaySFX("UpgradePick");
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}