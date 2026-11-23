using UnityEngine;

public class AutoUpgradeManager : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;

    // Пороги очков, после которых выдаются улучшения
    private int nextDamageMilestone = 100;
    private int nextHPMilestone = 300;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            playerAttack = p.GetComponent<PlayerAttack>();
            playerHealth = p.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (GameManager.Instance == null || playerAttack == null) return;

        int currentScore = GameManager.Instance.GetScore();

        // Проверка на улучшение урона (каждые 100 очков)
        if (currentScore >= nextDamageMilestone)
        {
            playerAttack.IncreaseDamage(2f); // Даем +2 к урону
            nextDamageMilestone += 100;      // Следующий порог будет 200, 300 и т.д.
            Debug.Log("<color=purple>[Авто-Апгрейд]</color> Урон увеличен на 2!");
        }

        // Проверка на улучшение максимального HP (на 300 очках)
        if (currentScore >= nextHPMilestone)
        {
            if (playerHealth != null)
            {
                playerHealth.IncreaseMaxHP(20f);
                playerHealth.Heal(20f); // Сразу лечим на эту сумму
            }
            nextHPMilestone += 300; // Следующий порог 600
            Debug.Log("<color=purple>[Авто-Апгрейд]</color> Максимальное HP увеличено на 20!");
        }
    }
}