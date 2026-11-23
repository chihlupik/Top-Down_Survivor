using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("UI Панели")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject mapSelectionPanel;

    [Header("Кнопки")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button map1Button;
    [SerializeField] private Button map2Button;
    [SerializeField] private Button backButton;

    void Start()
    {
        // Изначально показываем только главное меню
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (mapSelectionPanel != null) mapSelectionPanel.SetActive(false);

        // Привязка кнопок
        if (playButton != null) playButton.onClick.AddListener(OpenMapSelection);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        if (backButton != null) backButton.onClick.AddListener(BackToMainMenu);

        if (map1Button != null) map1Button.onClick.AddListener(() => SelectMap(1));
        if (map2Button != null) map2Button.onClick.AddListener(() => SelectMap(2));
    }

    void OpenMapSelection()
    {
        mainMenuPanel.SetActive(false);
        mapSelectionPanel.SetActive(true);
    }

    void BackToMainMenu()
    {
        mapSelectionPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

void SelectMap(int mapId)
{
    // Сохраняем выбор (на будущее, если понадобится)
    PlayerPrefs.SetInt("SelectedMap", mapId);
    PlayerPrefs.Save();

    // Загружаем нужную сцену в зависимости от нажатой кнопки
    if (mapId == 1)
    {
        SceneManager.LoadScene("level0"); // Загружаем первую карту
    }
    else if (mapId == 2)
    {
        SceneManager.LoadScene("level1"); // Загружаем вторую карту
    }
}

    void QuitGame()
    {
        Application.Quit();
    }
}