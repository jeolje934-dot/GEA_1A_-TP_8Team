
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("일반 적 설정")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public int maxAlive = 10;
    public int enemiesPerRound = 10;

    [Header("보스 설정")]
    public GameObject bossPrefab;
    public int bossRoundInterval = 3;

    [Header("라운드 정보")]
    public int currentRound = 1;

    private float timer;
    private int enemiesSpawned;
    private bool bossSpawned;

    void Update()
    {
        // 보스 라운드인지 확인
        if (currentRound % bossRoundInterval == 0)
        {
            UpdateBossRound();
        }
        else
        {
            UpdateNormalRound();
        }
    }

    // 일반 라운드
    void UpdateNormalRound()
    {
        int alive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // 정해진 수만큼 적을 소환하고 모두 처치하면 다음 라운드
        if (enemiesSpawned >= enemiesPerRound)
        {
            if (alive == 0)
            {
                NextRound();
            }

            return;
        }

        // 살아 있는 적이 너무 많으면 소환하지 않음
        if (alive >= maxAlive) return;

        timer += Time.deltaTime;

        if (timer < spawnInterval) return;

        timer = 0f;
        SpawnEnemy();
    }

    // 보스 라운드
    void UpdateBossRound()
    {
        // 보스는 한 번만 소환
        if (!bossSpawned)
        {
            if (bossPrefab == null)
            {
                Debug.LogError("Boss Prefab이 연결되지 않았습니다!");
                return;
            }

            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogError("Spawn Points가 설정되지 않았습니다!");
                return;
            }

            int s = Random.Range(0, spawnPoints.Length);

            Instantiate(
                bossPrefab,
                spawnPoints[s].position,
                Quaternion.identity
            );

            bossSpawned = true;
            Debug.Log(currentRound + "라운드 보스 등장!");
            return;
        }

        // 보스를 포함한 모든 적이 사라지면 다음 라운드
        int alive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (alive == 0)
        {
            Debug.Log("보스 처치! 다음 라운드로 이동합니다.");
            NextRound();
        }
    }

    // 일반 적 소환
    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("Enemy Prefabs가 설정되지 않았습니다!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn Points가 설정되지 않았습니다!");
            return;
        }

        int e = Random.Range(0, enemyPrefabs.Length);
        int s = Random.Range(0, spawnPoints.Length);

        Instantiate(
            enemyPrefabs[e],
            spawnPoints[s].position,
            Quaternion.identity
        );

        enemiesSpawned++;
    }

    // 다음 라운드로 이동
    void NextRound()
    {
        currentRound++;
        enemiesSpawned = 0;
        bossSpawned = false;
        timer = 0f;

        Debug.Log(currentRound + "라운드 시작!");
    }
}
