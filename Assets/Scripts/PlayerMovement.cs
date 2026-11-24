using UnityEngine;

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
        // Ищем картинку на самом объекте или на его дочерних объектах
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
    }

    void FixedUpdate()
    {
        Vector2 input = Vector2.zero;
        
        if (joystick != null)
            input = joystick.Value;

        // Движение
        rb.linearVelocity = Vector2.ClampMagnitude(input, 1f) * moveSpeed;

        // === ЛОГИКА ЗЕРКАЛИРОВАНИЯ ===
        if (spriteRenderer != null)
        {
            if (input.x < -0.01f) // Если движемся влево
            {
                spriteRenderer.flipX = true;
            }
            else if (input.x > 0.01f) // Если движемся вправо
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    public void IncreaseSpeed(float amount) => moveSpeed += amount;
}