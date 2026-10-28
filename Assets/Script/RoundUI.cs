
using UnityEngine;
using TMPro;

public class RoundUI : MonoBehaviour
{
    [Header("라운드 UI")]
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text bossWarningText;

    private int lastRound = -1;

    private void Start()
    {
        if (bossWarningText != null)
        {
            bossWarningText.gameObject.SetActive(false);
        }

        UpdateRoundUI();
    }

    private void Update()
    {
        UpdateRoundUI();
    }

    private void UpdateRoundUI()
    {
        if (EnemySpawner.Instance == null)
        {
            return;
        }

        int round = EnemySpawner.Instance.currentRound;

        if (round == lastRound)
        {
            return;
        }

        lastRound = round;

        if (roundText != null)
        {
            roundText.text = "ROUND " + round;
        }

        int bossInterval = EnemySpawner.Instance.bossRoundInterval;

        bool isBossRound =
            bossInterval > 0 && round % bossInterval == 0;

        if (bossWarningText != null)
        {
            bossWarningText.gameObject.SetActive(isBossRound);

            if (isBossRound)
            {
                bossWarningText.text = "BOSS ROUND!";
            }
        }

        Debug.Log(
            isBossRound
                ? $"ROUND {round}: 보스 라운드 UI 표시"
                : $"ROUND {round}: 라운드 UI 갱신"
        );
    }
}
