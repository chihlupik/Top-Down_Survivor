using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [Header("Ссылки на UI")]
    [SerializeField] private Image fillImage;       // Сюда перетащим HP_Bar_Fill
    [SerializeField] private TextMeshProUGUI hpText; // Сюда перетащим HP_Text

    [Header("Ссылка на игрока")]
    [SerializeField] private PlayerHealth playerHealth; // Сюда перетащим Player

    void Start()
    {
        // Если игрок не назначен в инспекторе, ищем его по тегу
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerHealth = player.GetComponent<PlayerHealth>();
        }

        // Подписываемся на событие изменения здоровья
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
            
            // Принудительно обновляем UI при старте, чтобы показать изначальное HP
            // (например, 100 из 100). Для этого нам нужен доступ к текущим данным.
            // Так как в PlayerHealth нет геттеров, мы просто вызовем обновление с дефолтными значениями
            // или добавим метод в PlayerHealth. Для простоты сделаем так:
        }
        else
        {
            Debug.LogError("HealthUI: Не удалось найти компонент PlayerHealth!");
        }
    }

    void OnDestroy()
    {
        // Отписываемся при уничтожении объекта, чтобы не было утечек памяти
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }

    // Этот метод вызывается каждый раз, когда игрок получает урон или лечится
    private void UpdateHealthUI(float currentHP, float maxHP)
    {
        // 1. Обновляем полоску (делим текущее на максимальное, получаем от 0 до 1)
        if (fillImage != null)
        {
            fillImage.fillAmount = currentHP / maxHP;
        }

        // 2. Обновляем текст. Mathf.RoundToInt округляет float до целого числа
        if (hpText != null)
        {
            hpText.text = $"{Mathf.RoundToInt(currentHP)} / {Mathf.RoundToInt(maxHP)}";
        }
    }
}