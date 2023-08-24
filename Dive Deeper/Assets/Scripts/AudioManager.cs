using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioClipsSO grenadeExplosions;
    public AudioClip GrenadeExplosion => grenadeExplosions.RandomAudioClip;
    [SerializeField] AudioClipsSO pickupSounds;
    public AudioClip PickupSound => pickupSounds.RandomAudioClip;
    [SerializeField] AudioClipsSO creatureSounds;
    public AudioClip CreatureSound => creatureSounds.RandomAudioClip;
    public AudioClip buttonClip;

    public AudioClip musicClip;

    float musicVolume;
    public float MusicVolume => musicVolume;
    float soundVolume;
    public float SoundVolume => soundVolume;


    protected override void Awake()
    {
        soundVolume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
    }

    public void PlayGrenadeExplosionAtPoint(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(GrenadeExplosion, position, soundVolume);
    }

    public void PlayPickupAtPoint(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(PickupSound, position, soundVolume);
    }

    public void PlayCreatureSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(CreatureSound, position, soundVolume);
    }

    public void PlayButtonSound()
    {
        AudioSource.PlayClipAtPoint(buttonClip, Camera.main.transform.position, soundVolume);
    }

    public void PlayClipAtPoint(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position, soundVolume);
    }

}
