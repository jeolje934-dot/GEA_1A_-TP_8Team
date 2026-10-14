
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public GameObject levelUpPanel;
    public PlayerStats playerStats;

    public void Show()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ChooseUpgrade(int type)
    {
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats가 연결되지 않았습니다!");
            return;
        }

        if (type == 0)
        {
            playerStats.UpgradeAttackPower();
        }
        else if (type == 1)
        {
            playerStats.UpgradeAttackSpeed();
        }
        else if (type == 2)
        {
            playerStats.UpgradeMaxHealth();
        }

        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;

        ScoreManager.Instance.FinishLevelUp();
    }
}
