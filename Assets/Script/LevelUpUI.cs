
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public GameObject levelUpPanel;
    public PlayerStats playerStats;

    public void Show()
    {
        // 레벨업 UI 표시
        levelUpPanel.SetActive(true);

        // 플레이어 이동 및 카메라 회전 중지
        PlayerController.inputLocked = true;

        // 마우스 커서 표시
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 게임 일시 정지
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

        // 레벨업 UI 닫기
        levelUpPanel.SetActive(false);

        // 게임 재개
        Time.timeScale = 1f;

        // 플레이어 조작 재개
        PlayerController.inputLocked = false;

        // 마우스 커서 숨기기 및 다시 고정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ScoreManager.Instance.FinishLevelUp();
    }
}
