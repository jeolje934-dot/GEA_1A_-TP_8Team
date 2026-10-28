
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

    // 보상 컴포넌트가 없는 적에게 적용되는 기본 경험치
    public int expPerKill = 1;

    [Header("레벨업 선택창")]
    public LevelUpUI levelUpUI;

    [Header("속성 선택창")]
    public ElementChoiceUI elementChoiceUI;

    private bool hasChosenElement = false;

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




    // 기존 코드와의 호환성을 위한 기본 처치 보상
    public void AddKillReward()
    {
        AddKillReward(expPerKill);
    }

    // 지정된 경험치만큼 처치 보상 지급
    public void AddKillReward(int expReward)
    {
        if (isLevelingUp)
            return;

        score += scorePerKill;
        currentExp += Mathf.Max(0, expReward);

        // 경험치가 남아 있다면 연속 레벨업도 처리
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            level++;

            // 플레이어 레벨업 시 경험치 소리 
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayLevelUp();
            }



            expToNextLevel =
                Mathf.CeilToInt(expToNextLevel * 1.5f);

            isLevelingUp = true;
        }


        UpdateUI();

        if (isLevelingUp)
        {
            // 레벨 10 이상이고 아직 속성을 선택하지 않았다면
            // 일반 레벨업 대신 속성 선택창을 표시
            if (level >= 10 && !hasChosenElement)
            {
                if (elementChoiceUI != null)
                {
                    elementChoiceUI.Show();
                }
                else
                {
                    Debug.LogWarning(
                        "ElementChoiceUI가 연결되지 않았습니다. 일반 레벨업 창을 표시합니다."
                    );

                    if (levelUpUI != null)
                        levelUpUI.Show();
                    else
                        isLevelingUp = false;
                }
            }
            else
            {
                if (levelUpUI != null)
                {
                    levelUpUI.Show();
                }
                else
                {
                    Debug.LogWarning("LevelUpUI가 연결되지 않았습니다!");
                    isLevelingUp = false;
                }
            }
        }
    }

    public void FinishLevelUp()
    {
        isLevelingUp = false;
        UpdateUI();
    }
    public void MarkElementChosen()
    {
        hasChosenElement = true;
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
