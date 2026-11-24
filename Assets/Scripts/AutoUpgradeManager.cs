using UnityEngine;

public class AutoUpgradeManager : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    [Header("Настройки")]
    [SerializeField] private int scoreForAutoUpgrade = 100; // Каждые 100 очков даем баф
    private int nextMilestone = 100;

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

        // Проверяем, достигли ли мы следующего рубежа очков
        if (currentScore >= nextMilestone)
        {
            nextMilestone += scoreForAutoUpgrade; // Увеличиваем следующий рубеж
            ApplyRandomBuff(); // Вызываем случайный баф
        }
    }

    void ApplyRandomBuff()
    {
        int rand = Random.Range(0, 4); // Случайное число от 0 до 3
        string msg = "";

        // Выбираем, какой баф дать (точно такие же, как в обычной прокачке)
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