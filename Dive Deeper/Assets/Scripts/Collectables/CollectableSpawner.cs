using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] CollectableBase[] collectablePrefabs;
    [SerializeField] float spawnDuration = 10f;
    GameObject lastSpawned;
    float nextSpawnTime;

    void Start()
    {
        SpawnRandomCollectable();
    }

    void Update()
    {
        if (lastSpawned == null)
        {
            if (Time.time >= nextSpawnTime)
            {
                nextSpawnTime = Time.time + spawnDuration;
                SpawnRandomCollectable();
            }
        }
    }

    private void SpawnRandomCollectable()
    {
        Vector3 spawnPoint = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
        lastSpawned = Instantiate(collectablePrefabs[Random.Range(0, collectablePrefabs.Length)].gameObject);
        lastSpawned.transform.position = spawnPoint;
    }
}
