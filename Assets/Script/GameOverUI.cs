
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

        // 게임오버 UI 표시
        gameOverPanel.SetActive(true);

        // 플레이어 이동 및 카메라 회전 중지
        PlayerController.inputLocked = true;

        // 마우스 커서 표시
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 게임 정지
        Time.timeScale = 0f;
    }



    public void RetryGame()
    {
        Time.timeScale = 1f;
        PlayerController.inputLocked = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        PlayerController.inputLocked = false;

        SceneManager.LoadScene("TitleScene");
    }

}
