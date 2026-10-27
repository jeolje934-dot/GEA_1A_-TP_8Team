
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour
{
    [Header("보스 체력")]
    public int maxHealth = 1000;
    private int currentHealth;
    private bool isDead = false;

    [Header("라운드별 체력 증가")]
    [SerializeField] private float hpIncreasePerRound = 0.2f;
    [SerializeField] private float maxHpMultiplier = 3f;

    [Header("보스 체력 UI")]
    public GameObject bossHealthPanel;
    public Slider healthSlider;
    public TMP_Text bossNameText;

    void Awake()
    {
        int round = 1;

        if (EnemySpawner.Instance != null)
            round = EnemySpawner.Instance.currentRound;

        float multiplier = Mathf.Min(
            1f + (round - 1) * hpIncreasePerRound,
            maxHpMultiplier
        );

        maxHealth = Mathf.Max(
            1,
            Mathf.RoundToInt(maxHealth * multiplier)
        );

        currentHealth = maxHealth;

        Debug.Log(
            $"보스 체력 적용: 라운드 {round}, " +
            $"최대 체력 {maxHealth}"
        );
    }

    void Start()
    {
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

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (bossNameText != null)
            bossNameText.text = "악어 보스";

        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        Debug.Log("보스 HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddKillReward();

        Destroy(gameObject);
    }
}
