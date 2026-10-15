
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 2f;
    public float detectRange = 15f;
    public float keepDistance = 8f;

    [Header("공격 설정")]
    public int damage = 5;
    public float attackCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    private Transform player;
    private float lastAttackTime = -999f;

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");

        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;

        if (distance > detectRange) return;
        if (distance < 0.01f) return;

        transform.forward = toPlayer.normalized;

        // 플레이어와 거리를 유지한다.
        if (distance > keepDistance)
        {
            transform.position +=
                transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (distance < keepDistance * 0.7f)
        {
            transform.position -=
                transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning(
                name + ": Projectile Prefab 또는 Fire Point가 없습니다.");
            return;
        }

        lastAttackTime = Time.time;

        Vector3 direction = player.position - firePoint.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        GameObject bullet = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        EnemyProjectile projectile =
            bullet.GetComponent<EnemyProjectile>();

        if (projectile != null)
            projectile.damage = damage;
    }
}
