using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningSceneManager : MonoBehaviour
{
    public List<Dialog> dialogs;
    public AudioSource audioSource;
    public float startDuration = 5f;
    public Animator eyesAnimator;
    public TextMeshProUGUI tmp;
    public GameObject textParent;
    public GameObject reverseEyes;
    public GameObject openingParent;
    public float endDuration = 3f;
    public float durationBeforeDialog = 3f;
    public AudioClip endingClip;
    public Image talkerImage;
    bool canSkip;
    Coroutine dialogCoroutine;

    void Start()
    {
        audioSource.volume = AudioManager.Instance.SoundVolume;
        dialogCoroutine = StartCoroutine(StartDialog());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSkip)
        {
            StopCoroutine(dialogCoroutine);
            StartCoroutine(Ending());
        }    
    }

    IEnumerator StartDialog()
    {
        yield return new WaitForSeconds(startDuration);
        openingParent.SetActive(false);
        eyesAnimator.enabled = true;
        yield return new WaitForSeconds(durationBeforeDialog);
        textParent.SetActive(true);
        canSkip = true;
        foreach (Dialog dialog in dialogs)
        {
            talkerImage.sprite = dialog.talkerImage;
            tmp.SetText(dialog.dialogText);
            if (dialog.dialogSound)
            {
                audioSource.clip = dialog.dialogSound;
                audioSource.Play();
            }
            yield return new WaitForSeconds(dialog.duration);
        }
        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        canSkip = false;
        textParent.SetActive(false);
        reverseEyes.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        AudioManager.Instance.PlayClipAtPoint(endingClip, transform.position);
        yield return new WaitForSeconds(endDuration);
        SceneManager.LoadScene("Map 1");
    }
}

[System.Serializable]
public class Dialog
{
    public Sprite talkerImage;
    public string dialogText;
    public AudioClip dialogSound;
    public float duration;
}