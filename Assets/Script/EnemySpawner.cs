using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public int maxAlive = 10;

    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0f;

        int alive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (alive >= maxAlive) return;

        int e = Random.Range(0, enemyPrefabs.Length);
        int s = Random.Range(0, spawnPoints.Length);
        Vector3 pos = spawnPoints[s].position;
        Instantiate(enemyPrefabs[e], pos, Quaternion.identity);
    }
}
