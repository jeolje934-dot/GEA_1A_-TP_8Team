
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [Header("패널")]
    public GameObject pausePanel;
    public GameObject statsPanel;

    [Header("타이틀 화면")]
    public string titleSceneName = "TitleScene";

    [Header("플레이어 능력치")]
    public PlayerStats playerStats;
    public Health playerHealth;

    [Header("능력치 텍스트")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;

    [Header("카메라 회전")]
    public MonoBehaviour cameraLookScript;

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        statsPanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pausePanel.SetActive(true);
        statsPanel.SetActive(false);

        // 마우스 조작으로 카메라가 돌아가지 않게 함
        if (cameraLookScript != null)
            cameraLookScript.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        statsPanel.SetActive(false);

        if (cameraLookScript != null)
            cameraLookScript.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowStats()
    {
        UpdateStatsUI();

        if (CombatStats.Instance != null)
        {
            CombatStats.Instance.UpdateUI();
        }

        pausePanel.SetActive(false);
        statsPanel.SetActive(true);
    }

    public void BackToPause()
    {
        statsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void UpdateStatsUI()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text =
                "최대 체력: " + playerHealth.maxHp;
        }

        if (playerStats != null)
        {
            if (damageText != null)
            {
                damageText.text =
                    "공격력: " + playerStats.attackPower;
            }

            if (attackSpeedText != null)
            {
                float cooldown = playerStats.attackCooldown;
                float speed = cooldown > 0f
                    ? 1f / cooldown
                    : 0f;

                attackSpeedText.text =
                    "공격 속도: " + speed.ToString("F1")
                    + "발/초";
            }
        }
    }

    public void GoToTitle()
    {
        // 씬 이동 전에 게임 정지 상태 해제
        Time.timeScale = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(titleSceneName);
    }
}
