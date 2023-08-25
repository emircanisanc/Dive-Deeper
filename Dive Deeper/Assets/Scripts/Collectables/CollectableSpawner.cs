using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] CollectableBase[] collectablePrefabs;
    [SerializeField] float spawnDuration = 10f;
    GameObject lastSpawned;
    Transform lastSpawnLocation;
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
        if (lastSpawnLocation == null)
            lastSpawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
        else
        {
            var newLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
            while (newLocation == lastSpawnLocation)
            {
                newLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
            }
            lastSpawnLocation = newLocation;
        }
        Vector3 spawnPoint = lastSpawnLocation.position;
        lastSpawned = Instantiate(collectablePrefabs[Random.Range(0, collectablePrefabs.Length)].gameObject);
        lastSpawned.transform.position = spawnPoint;
    }
}
