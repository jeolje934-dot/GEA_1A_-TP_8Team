
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("점수 UI")]
    public TMP_Text scoreText;

    [Header("레벨 UI")]
    public TMP_Text levelText;
    public Slider expSlider;
    public TMP_Text expText;

    [Header("점수 설정")]
    public int score = 0;
    public int scorePerKill = 100;

    [Header("경험치 설정")]
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;
    public int expPerKill = 25;

    [Header("레벨업 선택창")]
    public LevelUpUI levelUpUI;

    private bool isLevelingUp = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    // 적 처치 보상
    public void AddKillReward()
    {
        // 레벨업 선택 중에는 보상을 중복 처리하지 않음
        if (isLevelingUp)
            return;

        score += scorePerKill;
        currentExp += expPerKill;

        UpdateUI();

        // 레벨업 조건 확인
        if (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            level++;

            expToNextLevel =
                Mathf.CeilToInt(expToNextLevel * 1.5f);

            isLevelingUp = true;

            UpdateUI();

            if (levelUpUI != null)
            {
                levelUpUI.Show();
            }
            else
            {
                Debug.LogWarning(
                    "LevelUpUI가 연결되지 않았습니다!"
                );

                isLevelingUp = false;
            }
        }
    }

    public void FinishLevelUp()
    {
        isLevelingUp = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (levelText != null)
            levelText.text = "Lv. " + level;

        if (expSlider != null)
        {
            expSlider.minValue = 0;
            expSlider.maxValue = expToNextLevel;
            expSlider.value = currentExp;
        }

        if (expText != null)
        {
            expText.text =
                currentExp + " / " + expToNextLevel;
        }
    }
}
