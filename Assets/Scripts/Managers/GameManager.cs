using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // или TMPro если используешь TextMeshPro

public enum GameState { Start, Playing, Pause, Win, Lose }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // события
    public event Action<int> OnScoreChanged;
    public event Action<int, int> OnPelletCountChanged;
    public event Action<GameState> OnStateChanged;

    // состояние игры
    public GameState State { get; private set; }
    public int Score { get; private set; }
    public int totalPellets { get; private set; }
    public int collectedPellets { get; private set; }

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject pausePanel;  // 🆕 добавил
    public GameObject gameplayUI;

    [Header("UI Texts")]
    public TextMeshProUGUI scoreText;        // текущий счёт
    public TextMeshProUGUI pelletsText;      // оставшиеся пеллеты
    public TextMeshProUGUI finalScoreText;   // итоговый счёт (для Win/Lose панелей)
    public TextMeshProUGUI highScoreText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ⚠️ ВАЖНО: если у тебя панели "пропадают",
        // не используй DontDestroyOnLoad, пока не будет отдельной сцены для UI
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // начальные значения
        Score = 0;
        collectedPellets = 0;

        // считаем пеллеты
        totalPellets = FindObjectsOfType<Pellet>().Length;

        // показываем старт
        State = GameState.Start;
        Time.timeScale = 0f;

        startPanel?.SetActive(true);
        gameOverPanel?.SetActive(false);
        winPanel?.SetActive(false);
        pausePanel?.SetActive(false);
        gameplayUI?.SetActive(false);

        int hs = SaveSystem.LoadHighScore();
        highScoreText.text = "Рекорд: " + hs;

        UpdateUI();

        // уведомляем
        OnScoreChanged?.Invoke(Score);
        OnPelletCountChanged?.Invoke(collectedPellets, totalPellets);
        OnStateChanged?.Invoke(State);
    }

    public void StartGame()
    {
        State = GameState.Playing;
        Time.timeScale = 1f;

        startPanel?.SetActive(false);
        gameOverPanel?.SetActive(false);
        winPanel?.SetActive(false);
        pausePanel?.SetActive(false); // 🆕 чтобы не мешала
        gameplayUI?.SetActive(true);

        OnStateChanged?.Invoke(State);
    }

    public void PauseGame()
    {
        if (State == GameState.Playing)
        {
            State = GameState.Pause;
            Time.timeScale = 0f;
            pausePanel?.SetActive(true); // 🆕 показываем меню паузы
            gameplayUI?.SetActive(false);
        }
        else if (State == GameState.Pause)
        {
            State = GameState.Playing;
            Time.timeScale = 1f;
            pausePanel?.SetActive(false); // 🆕 убираем меню паузы
            gameplayUI?.SetActive(true);
        }
        OnStateChanged?.Invoke(State);
    }

    public void ResumeGame()
    {
        if (State == GameState.Pause)
        {
            State = GameState.Playing;
            Time.timeScale = 1f;
            pausePanel?.SetActive(false);
            gameplayUI?.SetActive(true);
            OnStateChanged?.Invoke(State);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CollectPellet(int value)
    {
        Score += value;
        collectedPellets++;

        OnScoreChanged?.Invoke(Score);
        OnPelletCountChanged?.Invoke(collectedPellets, totalPellets);

        if (collectedPellets >= totalPellets)
        {
            SaveSystem.SaveHighScore(Score);
            State = GameState.Win;
            Time.timeScale = 0;
            winPanel?.SetActive(true);
            OnStateChanged?.Invoke(State);
        }
    }

    public void PlayerDied()
    {
        if (State == GameState.Lose) return;

        State = GameState.Lose;
        Time.timeScale = 0f;
        gameOverPanel?.SetActive(true);
        gameplayUI?.SetActive(false);
        if (finalScoreText != null)
            finalScoreText.text = "Итоговый счёт: " + Score;
        OnStateChanged?.Invoke(State);

        SaveSystem.SaveHighScore(Score);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Счёт: " + Score;

        if (pelletsText != null)
            pelletsText.text = "Осталось: " + (totalPellets - collectedPellets);

        OnScoreChanged?.Invoke(Score);
        OnPelletCountChanged?.Invoke(collectedPellets, totalPellets);
    }
}