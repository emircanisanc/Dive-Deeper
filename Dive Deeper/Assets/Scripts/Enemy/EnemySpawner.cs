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
        //liveEnemyCount = FindObjectsOfType<EnemyBaseAbstract>().Length;
        liveEnemyCount = 0;
    }

    void Start() {
        StartCoroutine(TimerCoroutine());    
    }

    IEnumerator TimerCoroutine()
    {
        InGameUI.Instance.SetTimerText(totalTime);
        while (totalTime > 0)
        {
            yield return new WaitForSeconds(1f);
            totalTime--;
            InGameUI.Instance.SetTimerText(totalTime);

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
        
        List<Transform> sortedSpawnPoints = enemySpawnPoints.OrderBy(spawnPoint => Vector3.Distance(spawnPoint.position, playerPos)).ToList();
        Vector3 spawnPosition = sortedSpawnPoints[Random.Range(1, sortedSpawnPoints.Count)].position;

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
