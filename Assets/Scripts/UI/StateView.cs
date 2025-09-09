using UnityEngine;
using UnityEngine.UI;

public class StateView : MonoBehaviour
{
    public Button startBtn;
    public Button pauseBtn;
    public Button restartBtn;

    void Start()
    {
        startBtn.onClick.AddListener(() => GameManager.Instance.StartGame());
        pauseBtn.onClick.AddListener(() => GameManager.Instance.PauseGame());
        restartBtn.onClick.AddListener(() => GameManager.Instance.RestartGame());
    }
}