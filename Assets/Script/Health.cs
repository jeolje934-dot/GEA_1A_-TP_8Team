using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public int maxHp = 3;
    public int currentHp;
    public Slider hpSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        UpdateBar();
    }

    public void TakeDamage(int damage)
    {
        if (currentHp <= 0) return;

        currentHp -= damage;
        Debug.Log(name + "HP: " + currentHp);
        UpdateBar();

        if (currentHp <= 0) Die();
    }
    public void IncreaseMaxHp(int amount)
    {
        maxHp += amount;
        currentHp += amount;

        currentHp = Mathf.Min(currentHp, maxHp);

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = currentHp;
        }

        Debug.Log("체력 강화 적용! 현재 체력: "
            + currentHp + " / " + maxHp);
    }

    void UpdateBar()
    {
        if (hpSlider != null)
            hpSlider.value = (float)currentHp / maxHp;
    }

    void Die()
    {
        if (CompareTag("Player"))
        {
            Debug.Log("게임 오버");

            if (GameOverUI.Instance != null)
            {
                GameOverUI.Instance.ShowGameOver();
            }
            else
            {
                Debug.LogWarning("GameOverUI가 연결되지 않았습니다!");
                Time.timeScale = 0f;
            }
        }
        else
        {
            ScoreManager.Instance.AddKillReward();
            Destroy(gameObject);
        }
    }
}