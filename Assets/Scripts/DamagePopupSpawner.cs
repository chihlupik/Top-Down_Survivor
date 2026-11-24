using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    public static GameObject PopupPrefab;

    [SerializeField] private GameObject damagePopupPrefab;

    void Awake()
    {
        PopupPrefab = damagePopupPrefab;
    }
}