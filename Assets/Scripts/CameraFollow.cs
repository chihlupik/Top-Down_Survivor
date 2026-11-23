using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Цель (Игрок)")]
    [SerializeField] private Transform target;

    [Header("Настройки следования")]
    [Tooltip("Скорость следования. Чем выше, тем резче камера догоняет игрока.")]
    [SerializeField] private float followSpeed = 5f;
    
    [Tooltip("Смещение камеры. Z должно быть -10 для 2D.")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);

    void Start()
    {
        // Если цель не назначена в инспекторе, ищем игрока по тегу
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning("CameraFollow: Игрок с тегом 'Player' не найден!");
            }
        }
    }

    // Используем LateUpdate, чтобы камера двигалась ПОСЛЕ того, как игрок переместился
    void LateUpdate()
    {
        if (target == null) return;

        // Вычисляем идеальную позицию камеры (позиция игрока + смещение)
        Vector3 desiredPosition = target.position + offset;

        // Плавно перемещаем камеру к этой позиции.
        // Благодаря Lerp, скорость движения камеры автоматически зависит от расстояния:
        // Игрок далеко -> Lerp делает большой шаг (камера летит быстро).
        // Игрок близко -> Lerp делает маленький шаг (камера плавно подстраивается).
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}