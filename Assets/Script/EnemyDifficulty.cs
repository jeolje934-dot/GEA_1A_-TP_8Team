
using UnityEngine;

public class EnemyDifficulty : MonoBehaviour
{
    [Header("라운드별 난이도 설정")]
    [SerializeField] private float hpIncreasePerRound = 0.2f;
    [SerializeField] private float damageIncreasePerRound = 0.2f;

    [Header("최대 배율")]
    [SerializeField] private float maxHpMultiplier = 3f;
    [SerializeField] private float maxDamageMultiplier = 3f;

    private void Awake()
    {
        // 현재 라운드 확인
        int round = 1;

        if (EnemySpawner.Instance != null)
        {
            round = EnemySpawner.Instance.currentRound;
        }

        // 라운드가 높을수록 배율 증가
        float hpMultiplier = Mathf.Min(
            1f + (round - 1) * hpIncreasePerRound,
            maxHpMultiplier
        );

        float damageMultiplier = Mathf.Min(
            1f + (round - 1) * damageIncreasePerRound,
            maxDamageMultiplier
        );

        // 체력 증가
        Health health = GetComponent<Health>();

        if (health != null)
        {
            health.maxHp = Mathf.Max(
                1,
                Mathf.RoundToInt(health.maxHp * hpMultiplier)
            );
        }

        // 근접 공격력 증가
        EnemyChaser chaser = GetComponent<EnemyChaser>();

        if (chaser != null)
        {
            chaser.damage = Mathf.Max(
                1,
                Mathf.RoundToInt(chaser.damage * damageMultiplier)
            );
        }

        // 원거리 공격력 증가
        EnemyRanged ranged = GetComponent<EnemyRanged>();

        if (ranged != null)
        {
            ranged.damage = Mathf.Max(
                1,
                Mathf.RoundToInt(ranged.damage * damageMultiplier)
            );
        }

        Debug.Log(
            $"{name} 난이도 적용 | 라운드: {round} | " +
            $"체력 배율: {hpMultiplier:F1} | " +
            $"공격력 배율: {damageMultiplier:F1}"
        );
    }
}
