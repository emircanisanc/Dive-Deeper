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
        AudioManager.Instance.OnSoundVolumeChanged += ChangeSoundVolume;
        audioSource.volume = AudioManager.Instance.SoundVolume;
    }

    private void ChangeSoundVolume(float value)
    {
        audioSource.volume = value;
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
            DamageImage.Instance.ShowImage(damage);
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

