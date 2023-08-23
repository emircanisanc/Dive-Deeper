using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        StartCoroutine(StartDialog());
    }

    IEnumerator StartDialog()
    {
        yield return new WaitForSeconds(startDuration);
        openingParent.SetActive(false);
        eyesAnimator.enabled = true;
        yield return new WaitForSeconds(durationBeforeDialog);
        textParent.SetActive(true);
        foreach (Dialog dialog in dialogs)
        {
            tmp.SetText(dialog.dialogText);
            if (dialog.dialogSound)
            {
                audioSource.clip = dialog.dialogSound;
                audioSource.Play();
            }
            yield return new WaitForSeconds(dialog.duration);
        }
        reverseEyes.SetActive(true);
        yield return new WaitForSeconds(endDuration);
        SceneManager.LoadScene("Level 1");
    }
}

[System.Serializable]
public class Dialog
{
    public string dialogText;
    public AudioClip dialogSound;
    public float duration;
}