using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : Singleton<PlayerHealth>, IDamageable
{
    [SerializeField] Float health;
    [SerializeField] Float maxHealth;

    public AudioClipsSO damageClips;
    public AudioClipsSO dieClips;

    public Action OnPlayerDied;
    AudioSource audioSource;
    bool isDead;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        health.Value = maxHealth.Value;
    }

    public void ApplyDamage(float damage)
    {
        health.Value = Mathf.Max(health.Value - damage, 0);

        if (health.Value <= 0)
        {
            if (!isDead)
            {
                audioSource.clip = dieClips.RandomAudioClip;
                audioSource.Play();
                Die();
            }
        }
        else
        {
            audioSource.clip = damageClips.RandomAudioClip;
            audioSource.Play();
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

