
using UnityEngine;
using UnityEngine.UI;

public class EXPBarUI : MonoBehaviour
{
    [Header("경험치 바")]
    public Slider expSlider;

    // 경험치 바 갱신
    public void UpdateEXPBar(int currentExp, int requiredExp)
    {
        if (expSlider == null) return;

        expSlider.minValue = 0;
        expSlider.maxValue = requiredExp;
        expSlider.value = currentExp;
    }



    
}
