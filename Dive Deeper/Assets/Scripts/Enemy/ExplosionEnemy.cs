using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : MeleeEnemy
{
    [SerializeField] GameObject explosionPrefab;
    public float deltaYToSpawn = 1f;
    
    protected override void Die()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y += deltaYToSpawn;
        Instantiate(explosionPrefab, spawnPos, Quaternion.identity);
        base.Die();
    }
}
