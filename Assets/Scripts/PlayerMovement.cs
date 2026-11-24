using UnityEngine;
using System.Collections; // Добавлено для корутин

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private FloatingJoystick joystick; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector2 input = joystick != null ? joystick.Value : Vector2.zero;

        rb.linearVelocity = Vector2.ClampMagnitude(input, 1f) * moveSpeed;

        if (spriteRenderer != null)
        {
            if (input.x < -0.01f) spriteRenderer.flipX = true; 
            else if (input.x > 0.01f) spriteRenderer.flipX = false; 
        }
    }

    public void IncreaseSpeed(float amount) => moveSpeed += amount;

    // НОВЫЙ МЕТОД: Временное ускорение
    public void ApplyTemporarySpeedBoost(float duration, float multiplier)
    {
        StartCoroutine(SpeedBoostRoutine(duration, multiplier));
    }

    private IEnumerator SpeedBoostRoutine(float duration, float multiplier)
    {
        float originalSpeed = moveSpeed; // Запоминаем текущую скорость
        moveSpeed *= multiplier;         // Ускоряем
        
        yield return new WaitForSeconds(duration); // Ждем заданное время
        
        // Возвращаем скорость, но только если игрок не прокачал её за это время
        // (Проверка простая: если скорость не изменилась, возвращаем)
        if (Mathf.Approximately(moveSpeed, originalSpeed * multiplier))
        {
            moveSpeed = originalSpeed;
        }
    }
}