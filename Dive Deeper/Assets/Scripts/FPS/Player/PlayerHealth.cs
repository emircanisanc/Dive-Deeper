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
        health.Value = Mathf.Max(health.Value - damage, 0);

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

    public void Heal(float healAmount)
    {
        health.Value = Mathf.Min(maxHealth.Value, healAmount + health.Value);
    }
}

