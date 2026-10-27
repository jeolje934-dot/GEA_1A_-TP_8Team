
using UnityEngine;
using TMPro;

public class CombatStats : MonoBehaviour
{
    public static CombatStats Instance;

    [Header("기록 UI")]
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private TMP_Text roundText;

    private int totalKills = 0;
    private float survivalTime = 0f;

    public int TotalKills => totalKills;
    public float SurvivalTime => survivalTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        // 일시정지 중에는 생존 시간이 증가하지 않음
        if (Time.timeScale > 0f)
        {
            survivalTime += Time.deltaTime;
        }

        UpdateUI();
    }

    public void RegisterKill()
    {
        totalKills++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (killsText != null)
        {
            killsText.text = "처치한 적: " + totalKills;
        }

        if (survivalTimeText != null)
        {
            int totalSeconds = Mathf.FloorToInt(survivalTime);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            survivalTimeText.text =
                $"생존 시간: {minutes:00}:{seconds:00}";
        }

        if (roundText != null)
        {
            int round = 1;

            if (EnemySpawner.Instance != null)
            {
                round = EnemySpawner.Instance.currentRound;
            }

            roundText.text = "현재 라운드: " + round;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
