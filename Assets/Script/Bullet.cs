
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("공격 설정")]
    public int damage = 1;
    public float lifeTime = 2f;
    public string targetTag = "Enemy";
    public string ownerTag = "Player";

    [Header("번개 설정")]
    public float lightningRadius = 4f;
    public int lightningDamage = 1;
    public int maxChainTargets = 2;

    private PlayerStats playerStats;
    private ElementalAttack elementalAttack;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(ownerTag);

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            elementalAttack = player.GetComponent<ElementalAttack>();

            if (playerStats != null)
                damage = playerStats.attackPower;
        }
        else
        {
            Debug.LogWarning("Player 태그 오브젝트를 찾을 수 없습니다.");
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.CompareTag(ownerTag))
            return;

        if (!other.CompareTag(targetTag))
            return;

        Transform enemyRoot = GetEnemyRoot(other);

        if (enemyRoot == null)
            return;

        DealDamage(enemyRoot, damage);

        ProjectileElement element = elementalAttack != null
            ? elementalAttack.SelectedElement
            : ProjectileElement.None;

        switch (element)
        {
            case ProjectileElement.Fire:
                {
                    ElementalStatusEffect effect =
                        GetStatusEffect(enemyRoot);

                    if (effect != null)
                        effect.ApplyBurn();

                    break;
                }

            case ProjectileElement.Ice:
                {
                    ElementalStatusEffect effect =
                        GetStatusEffect(enemyRoot);

                    if (effect != null)
                        effect.ApplySlow();

                    break;
                }

            case ProjectileElement.Lightning:
                ChainLightning(enemyRoot);
                break;
        }

        Destroy(gameObject);
    }

    private Transform GetEnemyRoot(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();
        if (health != null)
            return health.transform;

        BossHealth boss = other.GetComponentInParent<BossHealth>();
        if (boss != null)
            return boss.transform;

        return other.transform;
    }

    private ElementalStatusEffect GetStatusEffect(Transform enemy)
    {
        ElementalStatusEffect effect =
            enemy.GetComponent<ElementalStatusEffect>();

        if (effect == null)
            effect = enemy.gameObject.AddComponent<ElementalStatusEffect>();

        return effect;
    }

    private void DealDamage(Transform enemy, int amount)
    {
        BossHealth boss = enemy.GetComponentInParent<BossHealth>();

        if (boss != null)
        {
            boss.TakeDamage(amount);
            return;
        }

        Health hp = enemy.GetComponentInParent<Health>();

        if (hp != null)
            hp.TakeDamage(amount);
    }

    private void ChainLightning(Transform firstEnemy)
    {
        Collider[] nearby = Physics.OverlapSphere(
            firstEnemy.position,
            lightningRadius
        );

        HashSet<Transform> hitEnemies = new HashSet<Transform>();
        hitEnemies.Add(firstEnemy);

        int chainCount = 0;

        foreach (Collider col in nearby)
        {
            if (chainCount >= maxChainTargets)
                break;

            if (col == null || col.isTrigger)
                continue;

            if (!col.CompareTag(targetTag))
                continue;

            Transform enemy = GetEnemyRoot(col);

            if (enemy == null || hitEnemies.Contains(enemy))
                continue;

            hitEnemies.Add(enemy);
            DealDamage(enemy, lightningDamage);
            chainCount++;
        }
    }
}
