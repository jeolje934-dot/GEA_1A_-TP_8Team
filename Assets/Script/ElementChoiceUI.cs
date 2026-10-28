
using UnityEngine;

public class ElementChoiceUI : MonoBehaviour
{
    [Header("선택창")]
    [SerializeField] private GameObject elementPanel;

    [Header("플레이어 속성")]
    [SerializeField] private ElementalAttack elementalAttack;

    public void Show()
    {
        if (elementPanel == null || elementalAttack == null)
        {
            Debug.LogError(
                "ElementChoiceUI의 Panel 또는 ElementalAttack이 연결되지 않았습니다."
            );
            return;
        }

        elementPanel.SetActive(true);

        PlayerController.inputLocked = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void ChooseFire()
    {
        Choose(ProjectileElement.Fire);
    }

    public void ChooseIce()
    {
        Choose(ProjectileElement.Ice);
    }

    public void ChooseLightning()
    {
        Choose(ProjectileElement.Lightning);
    }

    private void Choose(ProjectileElement element)
    {
        if (elementalAttack == null)
        {
            Debug.LogError("ElementalAttack이 연결되지 않았습니다.");
            return;
        }

        elementalAttack.SelectElement(element);

        elementPanel.SetActive(false);

        PlayerController.inputLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.MarkElementChosen();
            ScoreManager.Instance.FinishLevelUp();
        }
    }
}
