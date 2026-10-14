
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("공격 능력치")]
    public int attackPower = 10;
    public float attackCooldown = 0.5f;

    [Header("레벨업 강화 수치")]
    [Tooltip("레벨업 시 증가하는 공격력")]
    public int attackPowerIncrease = 5;

    [Tooltip("레벨업 시 공격 대기 시간 감소 비율 (0.2 = 20%)")]
    [Range(0f, 0.9f)]
    public float attackSpeedIncrease = 0.2f;

    [Tooltip("레벨업 시 증가하는 최대 체력")]
    public int maxHealthIncrease = 20;

    [Header("체력 능력치")]
    public int maxHealth = 100;
    public int currentHealth = 100;

    // 공격력 강화
    public void UpgradeAttackPower()
    {
        attackPower += attackPowerIncrease;

        Debug.Log("공격력 증가! 현재 공격력: " + attackPower);
    }

    // 공격속도 강화
    public void UpgradeAttackSpeed()
    {
        attackCooldown *= (1f - attackSpeedIncrease);
        attackCooldown = Mathf.Max(0.1f, attackCooldown);

        Debug.Log("공격 속도 증가! 현재 공격 대기 시간: "
            + attackCooldown);
    }

    // 최대 체력 강화
    public void UpgradeMaxHealth()
    {
        maxHealth += maxHealthIncrease;
        currentHealth += maxHealthIncrease;

        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log("최대 체력 증가! 현재 체력: "
            + currentHealth + " / " + maxHealth);
    }
}
