using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : Singleton<PlayerHealth>, IDamageable
{
    [SerializeField] Float health;
    [SerializeField] Float maxHealth;

    public Action OnPlayerDied;

    bool isDead;

    void Start()
    {
        health.Value = maxHealth.Value;    
    }

    public void ApplyDamage(float damage)
    {
        health.Value -= damage;

        if (health.Value <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        OnPlayerDied?.Invoke();

    }
}

