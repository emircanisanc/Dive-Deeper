using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBoss : SkeletonBoss
{
    public string bossName = "DEMON GIRL";
    public GameObject spawnOnDeath;
    public float deltaSpawnY = 1f;

    protected override void Start()
    {
        base.Start();
        InGameUI.Instance.OpenBossHealthBar(bossName);
    }

    protected override void Die()
    {
        if (spawnOnDeath)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y += deltaSpawnY;
            Instantiate(spawnOnDeath, spawnPos, Quaternion.identity);
        }
        base.Die();
    }

    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);
        InGameUI.Instance.SetPercentOfHealthBar(currentHealth / maxHealth);
    }
}
