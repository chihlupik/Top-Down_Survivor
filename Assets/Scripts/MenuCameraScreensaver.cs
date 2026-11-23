using UnityEngine;

public class MenuCameraScreensaver : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float speed = 2f; // Скорость полета

    [Header("Границы движения камеры")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;

    private Vector3 targetPosition;

    void Start()
    {
        // При запуске сразу выбираем первую случайную точку
        PickNewTarget();
    }

    void Update()
    {
        // Двигаем камеру к цели с постоянной скоростью
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Если камера почти долетела до цели (расстояние меньше 0.5), выбираем новую
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        // Генерируем случайные координаты в пределах заданных границ
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        
        // Оставляем Z без изменений (обычно это -10 для 2D камеры)
        targetPosition = new Vector3(randomX, randomY, transform.position.z);
    }
}