using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : Singleton<EnemySpawner>
{
    public GameObject[] enemyPrefabs;
    public List<Transform> enemySpawnPoints;
    public int totalTime;
    public int maxEnemyAlive;
    int liveEnemyCount;

    protected override void Awake()
    {
        base.Awake();
        liveEnemyCount = FindObjectsOfType<EnemyBaseAbstract>().Length;
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        while (totalTime > 0)
        {
            yield return new WaitForSeconds(1f);
            totalTime--;

            // Spawn enemies if the maximum number of enemies is not reached
            if (liveEnemyCount < maxEnemyAlive)
            {
                SpawnEnemy();
            }
        }

        // Check if there are no live enemies left
        if (liveEnemyCount <= 0)
        {
            EndTimer();
        }
    }

    private void EndTimer()
    {
        GameManager.Instance.StartPhaseTwo();
    }

    private void SpawnEnemy()
    {
        Vector3 playerPos = PlayerHealth.Instance.transform.position;
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Vector3 spawnPosition = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)].position; ;
        while (Vector3.Distance(spawnPosition, playerPos) < 10f)
        {
            spawnPosition = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)].position;
        }
        EnemyBaseAbstract enemy = Instantiate(randomPrefab, spawnPosition, Quaternion.identity).GetComponent<EnemyBaseAbstract>();
        enemy.SetPlayer();
        liveEnemyCount++;
    }

    public void OnEnemyDied()
    {
        liveEnemyCount--;
        // Check if the timer has ended and there are no live enemies left
        if (totalTime <= 0 && liveEnemyCount <= 0)
        {
            EndTimer();
        }
    }
}
