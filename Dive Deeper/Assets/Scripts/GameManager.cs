using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GameManager : Singleton<GameManager>
{
    public GameObject doorToMove;
    public float doorTargetY;

    [Header("TALK BUBBLE")]
    public bool talkAtStart;
    public GameObject talkCanvas;
    public TextMeshProUGUI talkerText;
    public Image talkerImage;
    public List<Dialog> startDialogs;
    public float startDuration;
    public AudioSource audioSource;
    public bool canPauseGame { get; set; } = true;

    protected bool phaseOne = true;
    public Action OnGameEnd;

    bool isGameEnd;
    bool isGamePaused;


    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }

    void Start()
    {
        PlayerHealth.Instance.OnPlayerDied += LoseGame;

        if (talkAtStart)
        {
            audioSource.volume = AudioManager.Instance.SoundVolume;
            AudioManager.Instance.OnSoundVolumeChanged += ChangeSoundVolume;
            StartCoroutine(StartDialog());
        }
        else
        {
            audioSource.volume = AudioManager.Instance.MusicVolume;
            audioSource.clip = AudioManager.Instance.musicClip;
            audioSource.loop = true;
            AudioManager.Instance.OnMusicVolumeChanged += ChangeSoundVolume;
            audioSource.Play();
        }

        Invoke(nameof(SetObjectiveAtStart), 1f);
    }

    private void SetObjectiveAtStart()
    {
        ImportantMessager.Instance.ShowMessage("SURVIVE AND FIND THE MIND JUICE");
    }

    private void ChangeSoundVolume(float value)
    {
        audioSource.volume = value;
    }

    IEnumerator StartDialog()
    {
        yield return new WaitForSeconds(startDuration);
        talkCanvas.SetActive(true);
        foreach (Dialog dialog in startDialogs)
        {
            talkerImage.sprite = dialog.talkerImage;
            talkerText.SetText(dialog.dialogText);
            if (audioSource)
            {
                audioSource.clip = dialog.dialogSound;
                audioSource.Play();
            }
            yield return new WaitForSeconds(dialog.duration);
        }
        AudioManager.Instance.OnSoundVolumeChanged -= ChangeSoundVolume;
        AudioManager.Instance.OnMusicVolumeChanged += ChangeSoundVolume;
        talkCanvas.SetActive(false);
        audioSource.volume = AudioManager.Instance.MusicVolume;
        audioSource.clip = AudioManager.Instance.musicClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public virtual void StartPhaseTwo()
    {
        if (!phaseOne)
            return;

        phaseOne = false;
        OpenDoor();
    }

    public void OpenDoor()
    {
        doorToMove.transform.DOMoveY(doorTargetY, 2f);
        ImportantMessager.Instance.ShowMessage("THE DOOR IS UNLOCKED COLLECT THE JUICE");
    }

    public void WinGame()
    {
        if (isGameEnd)
            return;

        isGameEnd = true;
        OnGameEnd?.Invoke();
        InGameUI.Instance.ShowWinGameUI();
    }

    public void LoseGame()
    {
        if (isGameEnd)
            return;

        isGameEnd = true;
        OnGameEnd?.Invoke();
        InGameUI.Instance.ShowLoseGameUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isGamePaused)
                UnPauseGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isGameEnd)
            return;

        if (isGamePaused)
            return;

        if (!canPauseGame)
            return;

        InGameUI.Instance.ShowPausePanel();
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        if (isGameEnd)
            return;

        if (!isGamePaused)
            return;

        if (!canPauseGame)
            return;

        InGameUI.Instance.ClosePausePanel();
        isGamePaused = false;
        Time.timeScale = 1f;
    }
    
    public void SetCanPauseGame()
    {
        Invoke(nameof(SetPauseActive), 1.2f);
    }

    private void SetPauseActive()
    {
        canPauseGame = true;
    }
}
