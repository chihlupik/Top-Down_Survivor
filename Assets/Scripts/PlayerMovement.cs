using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private OnScreenStick joystick;
    private Rigidbody2D rb;
    private Vector2 input;

    void Awake() => rb = GetComponent<Rigidbody2D>();

void Update()
{
    if (joystick != null && joystick.control is InputControl<Vector2> stickControl)
    {
        input = stickControl.ReadValue();
    }
    else
    {
        input = Vector2.zero;
    }
}

    void FixedUpdate()
    {
        rb.linearVelocity = input.normalized * moveSpeed;
    }

    public void SetSpeed(float newSpeed) => moveSpeed = newSpeed;
}