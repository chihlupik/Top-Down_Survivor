using UnityEngine;

public class AutoUpgradeManager : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    [Header("Настройки")]
    [SerializeField] private int scoreStep = 100; // Каждые 100 очков даем баф
    private int nextMilestone = 100;

    // Флаги для особых улучшений
    private bool gotDoubleShot = false;
    private bool gotTripleShot = false;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            playerAttack = p.GetComponent<PlayerAttack>();
            playerMovement = p.GetComponent<PlayerMovement>();
            playerHealth = p.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (GameManager.Instance == null || playerAttack == null) return;

        int currentScore = GameManager.Instance.GetScore();

        // 1. Проверяем особые рубежи (500 и 1000)
        if (currentScore >= 500 && !gotDoubleShot)
        {
            gotDoubleShot = true;
            playerAttack.SetProjectileCount(2); // Двойной выстрел
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification("АВТО: ДВОЙНОЙ ВЫСТРЕЛ!", Color.magenta);
        }
        else if (currentScore >= 1000 && !gotTripleShot)
        {
            gotTripleShot = true;
            playerAttack.SetProjectileCount(3); // Тройной выстрел
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification("АВТО: ТРОЙНОЙ ВЫСТРЕЛ!", Color.magenta);
        }

        // 2. Обычные случайные улучшения (кроме 500 и 1000)
        if (currentScore >= nextMilestone)
        {
            // Если текущий рубеж - это не 500 и не 1000, даем случайный баф
            if (nextMilestone != 500 && nextMilestone != 1000)
            {
                ApplyRandomBuff();
            }
            nextMilestone += scoreStep; 
        }
    }

    void ApplyRandomBuff()
    {
        int rand = Random.Range(0, 4); 
        string msg = "";

        switch (rand)
        {
            case 0: 
                playerAttack.IncreaseDamage(3f); 
                msg = "АВТО: +3 УРОН!"; 
                break;
            case 1: 
                if (playerMovement != null) playerMovement.IncreaseSpeed(0.5f); 
                msg = "АВТО: +0.5 СКОРОСТЬ!"; 
                break;
            case 2: 
                if (playerHealth != null) playerHealth.IncreaseMaxHP(15f); 
                msg = "АВТО: +15 МАКС. HP!"; 
                break;
            case 3: 
                playerAttack.IncreaseFireRate(0.1f); 
                msg = "АВТО: +СКОРОСТРЕЛЬНОСТЬ!"; 
                break;
        }

        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowNotification(msg, Color.magenta);
    }
}