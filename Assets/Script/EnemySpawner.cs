
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    private void Awake()
    {
        Instance = this;
    }
    [Header("일반 적 설정")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public int maxAlive = 10;
    public int enemiesPerRound = 10;

    [Header("라운드별 난이도")]
    public int addEnemiesPerRound = 2;
    public float spawnIntervalDecrease = 0.2f;
    public float minSpawnInterval = 0.8f;

    [Header("보스 설정")]
    public GameObject bossPrefab;
    public int bossRoundInterval = 3;

    [Header("라운드 정보")]
    public int currentRound = 1;

    private float timer;
    private int enemiesSpawned;
    private bool bossSpawned;

    // 현재 라운드에서 생성할 적의 총수
    public int CurrentEnemiesPerRound
    {
        get
        {
            return enemiesPerRound
                + (currentRound - 1) * addEnemiesPerRound;
        }
    }

    // 라운드가 높을수록 적 생성 간격 감소
    public float CurrentSpawnInterval
    {
        get
        {
            return Mathf.Max(
                minSpawnInterval,
                spawnInterval
                    - (currentRound - 1) * spawnIntervalDecrease
            );
        }
    }

    void Start()
    {
        if (currentRound < 1)
            currentRound = 1;

        Debug.Log(
            $"ROUND {currentRound} 시작! " +
            $"일반 적 목표: {CurrentEnemiesPerRound}마리"
        );
    }

    void Update()
    {
        if (bossRoundInterval > 0 &&
            currentRound % bossRoundInterval == 0)
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
        int alive =
            GameObject.FindGameObjectsWithTag("Enemy").Length;

        // 이번 라운드의 적을 모두 생성한 경우
        if (enemiesSpawned >= CurrentEnemiesPerRound)
        {
            if (alive == 0)
                NextRound();

            return;
        }

        // 동시 생존 적 수 제한
        if (alive >= maxAlive)
            return;

        timer += Time.deltaTime;

        if (timer < CurrentSpawnInterval)
            return;

        timer = 0f;
        SpawnEnemy();
    }

    // 보스 라운드
    void UpdateBossRound()
    {
        if (!bossSpawned)
        {
            if (bossPrefab == null)
            {
                Debug.LogError(
                    "Boss Prefab이 연결되지 않았습니다!"
                );
                return;
            }

            if (spawnPoints == null ||
                spawnPoints.Length == 0)
            {
                Debug.LogError(
                    "Spawn Points가 설정되지 않았습니다!"
                );
                return;
            }

            int s = Random.Range(0, spawnPoints.Length);

            Instantiate(
                bossPrefab,
                spawnPoints[s].position,
                Quaternion.identity
            );

            bossSpawned = true;

            Debug.Log(
                $"ROUND {currentRound}: 보스 등장!"
            );

            return;
        }

        int alive =
            GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (alive == 0)
        {
            Debug.Log("보스 처치! 다음 라운드로 이동합니다.");
            NextRound();
        }
    }

    // 일반 적 생성
    void SpawnEnemy()
    {
        if (enemyPrefabs == null ||
            enemyPrefabs.Length == 0)
        {
            Debug.LogError(
                "Enemy Prefabs가 설정되지 않았습니다!"
            );
            return;
        }

        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogError(
                "Spawn Points가 설정되지 않았습니다!"
            );
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

        Debug.Log(
            $"ROUND {currentRound}: " +
            $"{enemiesSpawned}/{CurrentEnemiesPerRound}마리 생성"
        );
    }

    // 다음 라운드
    void NextRound()
    {
        currentRound++;

        enemiesSpawned = 0;
        bossSpawned = false;
        timer = 0f;

        Debug.Log(
            $"ROUND {currentRound} 시작! " +
            $"일반 적 목표: {CurrentEnemiesPerRound}마리, " +
            $"생성 간격: {CurrentSpawnInterval:F1}초"
        );
    }
}
