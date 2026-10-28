
using System.Collections;
using UnityEngine;

public class ElementalStatusEffect : MonoBehaviour
{
    [Header("화상 설정")]
    [SerializeField] private int burnDamage = 1;
    [SerializeField] private float burnDuration = 3f;
    [SerializeField] private float burnInterval = 1f;

    [Header("빙결 설정")]
    [SerializeField] private float slowDuration = 2f;
    [SerializeField, Range(0f, 1f)]
    private float slowAmount = 0.4f;

    private Health health;
    private BossHealth bossHealth;
    private EnemyChaser enemyChaser;

    private Coroutine burnCoroutine;
    private Coroutine slowCoroutine;
    private float originalMoveSpeed;
    private bool speedCached;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        bossHealth = GetComponentInParent<BossHealth>();
        enemyChaser = GetComponentInParent<EnemyChaser>();

        if (enemyChaser != null)
        {
            originalMoveSpeed = enemyChaser.moveSpeed;
            speedCached = true;
        }
    }

    public void ApplyBurn()
    {
        if (burnCoroutine != null)
            StopCoroutine(burnCoroutine);

        burnCoroutine = StartCoroutine(BurnRoutine());
    }

    private IEnumerator BurnRoutine()
    {
        float elapsed = 0f;

        while (elapsed < burnDuration && IsAlive())
        {
            yield return new WaitForSeconds(burnInterval);

            if (!IsAlive())
                break;

            if (health != null)
                health.TakeDamage(burnDamage);
            else if (bossHealth != null)
                bossHealth.TakeDamage(burnDamage);

            elapsed += burnInterval;
        }

        burnCoroutine = null;
    }

    public void ApplySlow()
    {
        if (enemyChaser == null)
            return;

        if (!speedCached)
        {
            originalMoveSpeed = enemyChaser.moveSpeed;
            speedCached = true;
        }

        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        enemyChaser.moveSpeed = originalMoveSpeed * (1f - slowAmount);
        slowCoroutine = StartCoroutine(SlowRoutine());
    }

    private IEnumerator SlowRoutine()
    {
        yield return new WaitForSeconds(slowDuration);

        if (enemyChaser != null)
            enemyChaser.moveSpeed = originalMoveSpeed;

        slowCoroutine = null;
    }

    private bool IsAlive()
    {
        if (health != null)
            return health.enabled && health.gameObject.activeInHierarchy;

        if (bossHealth != null)
            return bossHealth.enabled && bossHealth.gameObject.activeInHierarchy;

        return false;
    }

    private void OnDisable()
    {
        if (burnCoroutine != null)
            StopCoroutine(burnCoroutine);

        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        if (enemyChaser != null && speedCached)
            enemyChaser.moveSpeed = originalMoveSpeed;
    }
}
