using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI scoreText;   
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI coinText;    // НОВОЕ: Текст для монет

    private float survivalTime;
    private bool gameOver;
    private const string HIGHSCORE_KEY = "HighScore";

    // НОВОЕ: Переменные для очков и монет
    private int currentScore = 0;
    private int currentCoins = 0;
    private float scoreTimer = 0f; // Таймер для медленного набора очков

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            PlayerHealth ph = p.GetComponent<PlayerHealth>();
            if (ph != null) ph.OnDied += TriggerGameOver;
        }
        UpdateUI();
    }

    void Update()
    {
        if (!gameOver)
        {
            survivalTime += Time.deltaTime;
            
            // Медленный набор очков: 1 очко в секунду
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1f)
            {
                AddScore(1); // Начисляем 1 очко
                scoreTimer = 0f;
            }
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateUI();
    }

    // Метод для покупки расходников
    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            UpdateUI();
            return true;
        }
        return false; // Не хватает монет
    }

    public int GetCoins() => currentCoins;
    public int GetScore() => currentScore;

    public void UpdateWaveUI(int wave)
    {
        if (waveText != null) waveText.text = $"Wave {wave}";
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {currentScore}";
        if (coinText != null) coinText.text = $"Coins: {currentCoins}";
    }

    void TriggerGameOver()
    {
        if (gameOver) return;
        gameOver = true;

        int highScore = PlayerPrefs.GetInt(HIGHSCORE_KEY, 0);

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt(HIGHSCORE_KEY, currentScore);
            PlayerPrefs.Save();
            if (resultText != null) resultText.text = $"NEW HIGH SCORE!\n{currentScore}\nCoins: {currentCoins}";
        }
        else
        {
            if (resultText != null) resultText.text = $"Score: {currentScore}\nBest: {highScore}\nCoins: {currentCoins}";
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        
        Time.timeScale = 0f; 
        AudioManager.Instance?.PlaySFX("GameOver");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}