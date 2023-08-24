using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public GameObject doorToClose;

    [Header("TALK BUBBLE")]
    public bool talkAtStart;
    public GameObject talkCanvas;
    public TextMeshProUGUI talkerText;
    public Image talkerImage;
    public List<Dialog> startDialogs;
    public float startDuration;
    public AudioSource audioSource;

    bool isGameEnd;
    bool isGamePaused;


    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        if (audioSource)
        {
            audioSource.volume = 0.5f;
            audioSource.playOnAwake = false;
        }
    }

    void Start()
    {
        PlayerHealth.Instance.OnPlayerDied += LoseGame;

        if (talkAtStart)
            StartCoroutine(StartDialog());
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
        talkCanvas.SetActive(false);
    }

    public void StartPhaseTwo()
    {
        doorToClose.SetActive(false);
    }

    public void WinGame()
    {
        if (isGameEnd)
            return;

        isGameEnd = true;
        InGameUI.Instance.ShowWinGameUI();
    }

    public void LoseGame()
    {
        if (isGameEnd)
            return;

        isGameEnd = true;
        InGameUI.Instance.ShowLoseGameUI();
    }

    public void PauseGame()
    {
        if (isGameEnd)
            return;

        if (isGamePaused)
            return;

        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        if (isGameEnd)
            return;

        if (!isGamePaused)
            return;

        isGamePaused = false;
        Time.timeScale = 1f;
    }
}
