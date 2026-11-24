using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;               // <-- ДОБАВЛЕНА ЭТА СТРОКА
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Collections.Generic;

public class FloatingJoystick : MonoBehaviour
{
    [Header("UI Ссылки")]
    [SerializeField] private RectTransform background; // Картинка фона джойстика
    [SerializeField] private RectTransform handle;     // Картинка ручки
    [SerializeField] private Canvas canvas;            // Ваш Canvas

    [Header("Настройки")]
    [SerializeField] private float radius = 100f; // Радиус движения ручки

    public Vector2 Value { get; private set; } // Выходное значение для игрока

    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();
    private Finger activeFinger;
    private Vector2 startPos;

    void Awake()
    {
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        
        // Изначально прячем визуал джойстика
        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerMove += OnFingerMove;
        Touch.onFingerUp += OnFingerUp;
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerMove -= OnFingerMove;
        Touch.onFingerUp -= OnFingerUp;
    }

    private void OnFingerDown(Finger finger)
    {
        // Если уже есть активный палец, игнорируем остальные
        if (activeFinger != null) return;

        // 1. Проверяем, не нажал ли игрок на UI-кнопку
        if (IsPointerOverUI(finger.screenPosition)) 
            return; 

        // 2. Если это не UI, активируем джойстик
        activeFinger = finger;
        startPos = finger.screenPosition;

        // Позиционируем джойстик в точке касания
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, 
            startPos, 
            canvas.worldCamera, 
            out localPoint
        );
        
        background.anchoredPosition = localPoint;
        handle.anchoredPosition = Vector2.zero;

        background.gameObject.SetActive(true);
        handle.gameObject.SetActive(true);
    }

    private void OnFingerMove(Finger finger)
    {
        if (finger != activeFinger) return;

        // Вычисляем разницу между текущей позицией и точкой старта
        Vector2 delta = finger.screenPosition - startPos;
        
        // Ограничиваем движение ручки радиусом
        Vector2 clampedDelta = Vector2.ClampMagnitude(delta, radius);
        
        // Двигаем ручку
        handle.anchoredPosition = clampedDelta;
        
        // Вычисляем нормализованное значение (-1..1)
        Value = clampedDelta / radius;
    }

    private void OnFingerUp(Finger finger)
    {
        if (finger != activeFinger) return;

        // Сбрасываем всё
        activeFinger = null;
        Value = Vector2.zero;
        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
    }

    // Проверка, находится ли точка над UI-элементом
    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (graphicRaycaster == null) return false;

        pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = screenPosition;
        raycastResults.Clear();
        
        graphicRaycaster.Raycast(pointerEventData, raycastResults);
        
        // Если в результатах есть хотя бы один объект — значит, палец на UI
        return raycastResults.Count > 0;
    }
}