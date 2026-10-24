
using UnityEngine;

public class BossAttackPatterns : MonoBehaviour
{
    [Header("플레이어")]
    public Transform player;

    [Header("기본 추적")]
    public float moveSpeed = 2f;
    public float stopDistance = 5f;

    [Header("패턴 공통 설정")]
    public float patternInterval = 4f;

    [Header("패턴 1 - 돌진")]
    public float chargeSpeed = 12f;
    public float chargeDuration = 0.6f;
    public float chargeCooldown = 5f;
    public int chargeDamage = 2;
    public float chargeHitDistance = 1.5f;

    [Header("패턴 2 - 물대포")]
    public GameObject waterProjectilePrefab;
    public Transform firePoint;
    public int projectileCount = 5;
    public float fanAngle = 60f;
    public float projectileSpeed = 10f;
    public int projectileDamage = 1;
    public float waterCooldown = 5f;

    private enum BossState
    {
        Chase,
        Charge,
        Recover,
        WaterAttack
    }

    private BossState state = BossState.Chase;

    private float nextPatternTime;
    private float nextChargeTime;
    private float nextWaterTime;
    private float stateTimer;

    private Vector3 chargeDirection;
    private bool hasHitPlayer;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }

        nextPatternTime = Time.time + patternInterval;
        nextChargeTime = Time.time + chargeCooldown;
        nextWaterTime = Time.time + waterCooldown;
    }

    void Update()
    {
        if (player == null) return;
        if (Time.timeScale == 0f) return;

        switch (state)
        {
            case BossState.Chase:
                ChasePlayer();
                CheckPattern();
                break;

            case BossState.Charge:
                DoCharge();
                break;

            case BossState.Recover:
                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0f)
                    state = BossState.Chase;
                break;

            case BossState.WaterAttack:
                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0f)
                    state = BossState.Chase;
                break;
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > 0.01f)
            transform.forward = direction.normalized;

        if (distance > stopDistance)
        {
            transform.position +=
                direction.normalized * moveSpeed * Time.deltaTime;
        }
    }

    void CheckPattern()
    {
        if (Time.time < nextPatternTime) return;

        bool canCharge = Time.time >= nextChargeTime;
        bool canShoot = Time.time >= nextWaterTime;

        if (!canCharge && !canShoot) return;

        // 두 패턴을 번갈아 사용
        if (canCharge && (!canShoot || Random.value < 0.5f))
        {
            StartCharge();
        }
        else
        {
            StartWaterAttack();
        }

        nextPatternTime = Time.time + patternInterval;
    }

    // 패턴 1: 플레이어를 향해 돌진
    void StartCharge()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        chargeDirection = direction.normalized;
        transform.forward = chargeDirection;

        hasHitPlayer = false;
        state = BossState.Charge;
        stateTimer = chargeDuration;
        nextChargeTime = Time.time + chargeCooldown;
    }

    void DoCharge()
    {
        transform.position +=
            chargeDirection * chargeSpeed * Time.deltaTime;

        if (!hasHitPlayer)
        {
            Vector3 playerPosition = player.position;
            Vector3 bossPosition = transform.position;

            playerPosition.y = 0f;
            bossPosition.y = 0f;

            if (Vector3.Distance(bossPosition, playerPosition)
                <= chargeHitDistance)
            {
                Health health = player.GetComponent<Health>();

                if (health != null)
                    health.TakeDamage(chargeDamage);

                hasHitPlayer = true;
            }
        }

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            state = BossState.Recover;
            stateTimer = 1f;
        }
    }

    // 패턴 2: 부채꼴 물대포 발사
    void StartWaterAttack()
    {
        if (waterProjectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning(
                "물탄환 Prefab 또는 Fire Point가 연결되지 않았습니다."
            );

            state = BossState.Chase;
            nextWaterTime = Time.time + waterCooldown;
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
            transform.forward = direction.normalized;

        state = BossState.WaterAttack;
        stateTimer = 0.8f;
        nextWaterTime = Time.time + waterCooldown;

        int count = Mathf.Max(1, projectileCount);

        for (int i = 0; i < count; i++)
        {
            float angle = count == 1
                ? 0f
                : -fanAngle / 2f
                  + fanAngle * i / (count - 1);

            Quaternion rotation =
                Quaternion.AngleAxis(angle, Vector3.up)
                * transform.rotation;

            GameObject projectile = Instantiate(
                waterProjectilePrefab,
                firePoint.position,
                rotation
            );

            BossWaterProjectile water =
                projectile.GetComponent<BossWaterProjectile>();

            if (water != null)
            {
                water.damage = projectileDamage;
                water.speed = projectileSpeed;
            }
        }
    }
}
