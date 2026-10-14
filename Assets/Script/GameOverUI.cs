
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [Header("게임 오버 UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (finalScoreText != null &&
            ScoreManager.Instance != null)
        {
            finalScoreText.text =
                "최종 점수: " + ScoreManager.Instance.score;
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }
}
