using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    public Text scoreText;    // привязать в инспекторе
    public Text pelletText;   // привязать в инспекторе

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScore;
            GameManager.Instance.OnPelletCountChanged += UpdatePellets;
        }
    }

    void Start()
    {
        // обновим UI сразу (на случай, если события уже были вызваны)
        if (GameManager.Instance != null)
        {
            UpdateScore(GameManager.Instance.Score);
            UpdatePellets(GameManager.Instance.collectedPellets, GameManager.Instance.totalPellets);
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnScoreChanged -= UpdateScore;
        GameManager.Instance.OnPelletCountChanged -= UpdatePellets;
    }

    void UpdateScore(int s) => scoreText.text = "Score: " + s;
    void UpdatePellets(int current, int total) => pelletText.text = $"Pellets: {current}/{total}";
}
