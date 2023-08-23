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

}
