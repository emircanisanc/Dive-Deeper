using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider soundSlider;
    public Slider musicSlider;

    float musicVolume = 0.5f;
    float soundVolume = 0.5f;
    
    void Start()
    {
        soundVolume = AudioManager.Instance.SoundVolume;
        musicVolume = AudioManager.Instance.MusicVolume;
        soundSlider.value = soundVolume;
        musicSlider.value = musicVolume;
        audioSource.volume = musicVolume;
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayButtonSound();
        PlayerPrefs.SetFloat("soundVolume", soundVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.Save();
        SceneManager.LoadScene("OpeningScene");
    }

    public void OnSoundValueChanged(float value)
    {
        soundVolume = value;
    }

    public void OnMusicValueChanged(float value)
    {
        musicVolume = value;
        audioSource.volume = musicVolume;
    }
}
