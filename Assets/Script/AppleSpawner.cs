
using UnityEngine;
using System.Collections.Generic;

public class AppleSpawner : MonoBehaviour
{
    public static AppleSpawner Instance;

    [Header("사과 설정")]
    public GameObject applePrefab;
    public int applesPerRound = 2;

    [Header("사과 생성 위치")]
    public Transform[] appleSpawnPoints;

    private List<GameObject> spawnedApples =
        new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    // 라운드가 시작될 때 호출
    public void SpawnApplesForRound()
    {
        // 이전 라운드의 사과 정리
        ClearApples();

        if (applePrefab == null)
        {
            Debug.LogError("Apple Prefab이 연결되지 않았습니다!");
            return;
        }

        if (appleSpawnPoints == null ||
            appleSpawnPoints.Length == 0)
        {
            Debug.LogError("Apple Spawn Points를 설정해 주세요!");
            return;
        }

        // 사과 2개를 랜덤 위치에 생성
        for (int i = 0; i < applesPerRound; i++)
        {
            Transform point = appleSpawnPoints[
                Random.Range(0, appleSpawnPoints.Length)
            ];

            GameObject apple = Instantiate(
                applePrefab,
                point.position,
                Quaternion.identity
            );

            spawnedApples.Add(apple);
        }

        Debug.Log($"사과 {applesPerRound}개 생성 완료!");
    }

    // 이전 라운드 사과 제거
    private void ClearApples()
    {
        foreach (GameObject apple in spawnedApples)
        {
            if (apple != null)
            {
                Destroy(apple);
            }
        }

        spawnedApples.Clear();
    }
}
