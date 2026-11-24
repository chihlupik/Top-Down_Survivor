using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Префабы врагов")]
    [SerializeField] private GameObject normalEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;
    [SerializeField] private GameObject shooterEnemyPrefab;

    [Header("Настройки спавна вокруг игрока")]
    [Tooltip("Расстояние от игрока, на котором появляются враги (должно быть чуть больше экрана)")]
    [SerializeField] private float spawnRadius = 12f; 
    
    [Tooltip("Слой, на котором лежат стены и границы карты")]
    [SerializeField] private LayerMask wallLayer;     

    [Header("Баланс волн")]
    [SerializeField] private float baseEnemyCount = 5f;
    [SerializeField] private float enemyCountMultiplier = 1.3f;
    [SerializeField] private float baseSpawnInterval = 1.2f;
    [SerializeField] private float intervalMultiplier = 0.92f;
    [SerializeField] private float baseEnemyHP = 30f;
    [SerializeField] private float enemyHPGrowth = 1.15f;

    [Header("Шансы спавна (в % от 0 до 100)")]
    [SerializeField] private float chanceForFast = 20f;
    [SerializeField] private float chanceForShooter = 15f;

    private int currentWave = 0;
    private float currentEnemyHP;
    private Transform playerTransform;

    public int CurrentWave => currentWave;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerTransform = p.transform;

        currentEnemyHP = baseEnemyHP;
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            currentWave++;
            
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification($"ВОЛНА {currentWave} НАЧАЛАСЬ!", Color.cyan);

            int count = Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(enemyCountMultiplier, currentWave - 1));
            float interval = baseSpawnInterval * Mathf.Pow(intervalMultiplier, currentWave - 1);
            interval = Mathf.Max(interval, 0.15f);

            for (int i = 0; i < count; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(interval);
            }

            // Ждем, пока игрок не убьет ВСЕХ врагов
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            
            // Открываем панель улучшений
            UpgradeManager um = FindFirstObjectByType<UpgradeManager>();
            if (um != null)
            {
                um.ShowUpgradePanel();
                yield return new WaitUntil(() => !um.IsPanelOpen);
            }

            // Задержка перед следующей волной
            yield return new WaitForSeconds(2f);
            currentEnemyHP *= enemyHPGrowth;
        }
    }

    void SpawnEnemy()
    {
        if (playerTransform == null) return;

        Vector2 spawnPos = GetValidSpawnPosition();
        
        // Логика выбора врага (шансы)
        float randomValue = Random.Range(0f, 100f);
        GameObject prefabToSpawn = normalEnemyPrefab;

        if (randomValue < chanceForFast && fastEnemyPrefab != null)
            prefabToSpawn = fastEnemyPrefab;
        else if (randomValue < chanceForFast + chanceForShooter && shooterEnemyPrefab != null)
            prefabToSpawn = shooterEnemyPrefab;

        if (prefabToSpawn == null) return;

        // Спавним
        GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        enemy.tag = "Enemy";

        EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
        if (eh != null) eh.SetMaxHP(currentEnemyHP);
    }

    // Ищем точку по кругу вокруг игрока, где нет стен
    Vector2 GetValidSpawnPosition()
    {
        int maxAttempts = 30; 

        // Попытка 1: Идеальный спавн за экраном (на расстоянии spawnRadius)
        for (int i = 0; i < maxAttempts; i++)
        {
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            Vector2 candidatePos = (Vector2)playerTransform.position + (direction * spawnRadius);

            // Проверяем, нет ли в этой точке коллайдера стены
            Collider2D hit = Physics2D.OverlapCircle(candidatePos, 1f, wallLayer);
            if (hit == null) return candidatePos;
        }

        // Попытка 2: Если игрок в углу и за экраном везде стены, 
        // спавним врага на половинном расстоянии, но внутри карты.
        Debug.Log("<color=orange>[WaveManager]</color> Игрок у края карты. Спавним врага ближе.");
        float closerRadius = spawnRadius / 2f;

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            Vector2 candidatePos = (Vector2)playerTransform.position + (direction * closerRadius);

            Collider2D hit = Physics2D.OverlapCircle(candidatePos, 1f, wallLayer);
            if (hit == null) return candidatePos;
        }

        // Аварийный случай: спавним прямо на игроке (если всё заблокировано)
        return playerTransform.position; 
    }
}