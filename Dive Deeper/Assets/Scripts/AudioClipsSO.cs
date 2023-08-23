using UnityEngine;

[CreateAssetMenu(menuName = "Audio Clip List")]
public class AudioClipsSO : ScriptableObject
{
    public AudioClip[] audioClips;
    public AudioClip RandomAudioClip => audioClips[Random.Range(0, audioClips.Length)];
}
