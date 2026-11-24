using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private float fadeSpeed = 2f;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null)
            Debug.LogError("На префабе DamagePopup нет компонента TextMeshPro (3D)!");
    }

    public static void Create(Vector3 position, float damageAmount, bool isPlayerDamaged)
    {
        // Берем префаб из менеджера
        GameObject prefab = DamagePopupSpawner.PopupPrefab;
        
        if (prefab == null) 
        {
            Debug.LogError("ОШИБКА: Префаб не назначен в DamagePopupSpawner на сцене!");
            return; 
        }

        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f), 0f);
        Vector3 spawnPosition = new Vector3(position.x + randomOffset.x, position.y + randomOffset.y, 0f);
        
        GameObject popupGO = Instantiate(prefab, spawnPosition, Quaternion.identity);
        DamagePopup popup = popupGO.GetComponent<DamagePopup>();
        
        if (popup != null) 
            popup.Setup(damageAmount, isPlayerDamaged);
    }

    private void Setup(float damageAmount, bool isPlayerDamaged)
    {
        if (textMesh == null) return;

        textMesh.SetText(Mathf.RoundToInt(damageAmount).ToString());
        
        textMesh.color = isPlayerDamaged ? Color.red : Color.yellow;
        
        textColor = textMesh.color;
        disappearTimer = 0.8f; 
        moveVector = new Vector3(0, 2f, 0);
    }

    private void Update()
    {
        if (textMesh == null) return;

        transform.position += moveVector * Time.deltaTime;
        moveVector -= new Vector3(0, 3f, 0) * Time.deltaTime; 

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;
            
            if (textColor.a <= 0) Destroy(gameObject);
        }
    }
}