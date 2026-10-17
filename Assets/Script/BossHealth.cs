
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour
{
    [Header("보스 체력")]
    public int maxHealth = 1000;
    private int currentHealth;
    private bool isDead = false;

    [Header("보스 체력 UI")]
    public GameObject bossHealthPanel;
    public Slider healthSlider;
    public TMP_Text bossNameText;

    void Start()
    {
        // 씬에 있는 보스 UI 관리자 찾기
        BossUIController ui =
            FindFirstObjectByType<BossUIController>();

        if (ui != null)
        {
            bossHealthPanel = ui.bossHealthPanel;
            healthSlider = ui.healthSlider;
            bossNameText = ui.bossNameText;
        }
        else
        {
            Debug.LogError(
                "BossUIController를 찾을 수 없습니다. Canvas를 확인하세요."
            );
        }

        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (bossNameText != null)
        {
            bossNameText.text = "악어 보스";
        }

        if (bossHealthPanel != null)
        {
            bossHealthPanel.SetActive(true);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(
            currentHealth, 0, maxHealth
        );

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log("보스 HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (bossHealthPanel != null)
        {
            bossHealthPanel.SetActive(false);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddKillReward();
        }

        Destroy(gameObject);
    }
}
