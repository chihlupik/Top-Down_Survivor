using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Префабы врагов")]
    [SerializeField] private GameObject normalEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;

    [Header("Точки спавна")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Баланс волн")]
    [SerializeField] private float baseEnemyCount = 5f;
    [SerializeField] private float enemyCountMultiplier = 1.3f;
    [SerializeField] private float baseSpawnInterval = 1.2f;
    [SerializeField] private float intervalMultiplier = 0.92f;
    [SerializeField] private float baseEnemyHP = 30f;
    [SerializeField] private float enemyHPGrowth = 1.15f;

    private int currentWave = 0;
    private float currentEnemyHP;

    public int CurrentWave => currentWave;

    void Start()
    {
        currentEnemyHP = baseEnemyHP;
        StartCoroutine(WaveLoop());
    }

IEnumerator WaveLoop()
{
    yield return new WaitForSeconds(1.5f);

    while (true)
    {
        currentWave++;
        int count = Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(enemyCountMultiplier, currentWave - 1));
        float interval = baseSpawnInterval * Mathf.Pow(intervalMultiplier, currentWave - 1);
        interval = Mathf.Max(interval, 0.15f);

        GameManager.Instance?.UpdateWaveUI(currentWave);

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(interval);
        }

        // Ждем 2 секунды, чтобы последние враги умерли
        yield return new WaitForSeconds(2f); 
        
        // === ВЫЗЫВАЕМ МЕНЮ УЛУЧШЕНИЙ ===
        // Находим UpgradeManager на сцене и показываем панель
        FindFirstObjectByType<UpgradeManager>()?.ShowUpgradePanel();
        
        currentEnemyHP *= enemyHPGrowth;
        
        // Ждем, пока игрок не выберет улучшение (игра встанет на паузу из-за Time.timeScale = 0)
        // Как только игрок нажмет кнопку, Time.timeScale станет 1, и корутина пойдет дальше
        yield return new WaitUntil(() => Time.timeScale > 0f);
    }
}

    void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Каждый 4-й враг — быстрый (начиная с 3-й волны)
        bool spawnFast = currentWave >= 3 && Random.value < 0.3f;
        GameObject prefab = spawnFast ? fastEnemyPrefab : normalEnemyPrefab;

        if (prefab == null) return;

        GameObject enemy = Instantiate(prefab, sp.position, Quaternion.identity);
        enemy.tag = "Enemy";

        EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
        if (eh != null) eh.SetMaxHP(currentEnemyHP);
    }
}