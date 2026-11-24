using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    [Header("UI Ссылки")]
    [SerializeField] private Transform container;       // Перетащим сюда NotificationContainer
    [SerializeField] private GameObject textPrefab;     // Перетащим сюда префаб NotificationItem

    [Header("Настройки")]
    [SerializeField] private float displayTime = 2f;    // Сколько секунд текст висит на экране
    [SerializeField] private float fadeDuration = 1f;   // Сколько секунд длится исчезновение

    void Awake()
    {
        // Синглтон, чтобы к нему можно было обращаться из любого скрипта
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // Вызов уведомления. Цвет по умолчанию — белый.
    public void ShowNotification(string message, Color? color = null)
    {
        if (container == null || textPrefab == null) return;

        // Создаем текст в контейнере
        GameObject go = Instantiate(textPrefab, container);
        TextMeshProUGUI textComp = go.GetComponent<TextMeshProUGUI>();
        textComp.text = message;
        
        if (color.HasValue) textComp.color = color.Value;

        // Запускаем корутину для плавного исчезновения
        StartCoroutine(FadeAndDestroy(textComp));
    }

    private IEnumerator FadeAndDestroy(TextMeshProUGUI textComp)
    {
        // Ждем, пока текст просто висит
        yield return new WaitForSeconds(displayTime);

        // Плавно уменьшаем прозрачность (Alpha)
        float t = 0;
        Color originalColor = textComp.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            textComp.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Уничтожаем текст (контейнер сам сдвинет остальные вверх благодаря Vertical Layout Group)
        Destroy(textComp.gameObject);
    }
}