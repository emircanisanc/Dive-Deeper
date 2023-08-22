using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : MeleeEnemy
{
    [SerializeField] GameObject explosionPrefab;
    
    protected override void Die()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        base.Die();
    }
}
