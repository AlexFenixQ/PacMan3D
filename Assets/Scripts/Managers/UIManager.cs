using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pelletsText;
    public TextMeshProUGUI finalScoreText; // дл€ экрана Win/Lose
    public TextMeshProUGUI powerTimerText;

    void Start()
    {
        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnPelletCountChanged += UpdatePellets;
        GameManager.Instance.OnStateChanged += UpdateFinalScore;
    }

    void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "—чЄт: " + score;
    }

    void UpdatePellets(int collected, int total)
    {
        if (pelletsText != null)
            pelletsText.text = $"ќсталось: {total - collected}/{total}";
    }

    void UpdateFinalScore(GameState state)
    {
        if ((state == GameState.Win || state == GameState.Lose) && finalScoreText != null)
        {
            finalScoreText.text = "»тоговый счЄт: " + GameManager.Instance.Score;
        }
    }

    void OnEnable()
    {
        PowerUpManager.Instance.OnPowerTimeChanged += OnPowerTick;
        PowerUpManager.Instance.OnPowerEnd += OnPowerEnd;
    }
    void OnDisable()
    {
        PowerUpManager.Instance.OnPowerTimeChanged -= OnPowerTick;
        PowerUpManager.Instance.OnPowerEnd -= OnPowerEnd;
    }
    void OnPowerTick(float t) { powerTimerText.text = $"Power: {Mathf.CeilToInt(t)}"; }
    void OnPowerEnd() { powerTimerText.text = ""; }
}