using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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
    public Transform profTransform;
    public Animator profAnimator;
    Sequence profMovement;

    public List<ProfMovement> profMovements;

    void Start()
    {
        audioSource.volume = AudioManager.Instance.SoundVolume;
        dialogCoroutine = StartCoroutine(StartDialog());
        
        profMovement = DOTween.Sequence();
        for (int i =0; i < profMovements.Count; i++)
        {
            ProfMovement movement = profMovements[i];
            ProfMovement nextMovement = null;
            if (profMovements.Count > i + 1)
            {
                if (profMovements[i + 1].profAnim != ProfAnim.noneOf)
                    nextMovement = profMovements[i + 1];
            }
                

            if (i == 0)
                profAnimator.SetTrigger(movement.profAnim.ToString());

            if (movement.append)
            {
                if (movement.movementType == ProfMovementEnum.move)
                {
                    if (nextMovement == null)
                        profMovement.Append(profTransform.DOMove(movement.targetTransform.position, movement.duration).SetEase(Ease.Linear));
                    else
                        profMovement.Append(profTransform.DOMove(movement.targetTransform.position, movement.duration).SetEase(Ease.Linear).OnComplete(() => profAnimator.SetTrigger(nextMovement.profAnim.ToString())));
                }
                else if (movement.movementType == ProfMovementEnum.turn)
                {
                    if (nextMovement == null)
                        profMovement.Append(profTransform.DORotate(movement.targetTransform.eulerAngles, movement.duration));
                    else
                        profMovement.Append(profTransform.DORotate(movement.targetTransform.eulerAngles, movement.duration).OnComplete(() => profAnimator.SetTrigger(nextMovement.profAnim.ToString())));
                }
                else
                {
                    if (nextMovement == null)
                        profMovement.Append(profTransform.DOScale(profTransform.localScale, movement.duration));
                    else
                        profMovement.Append(profTransform.DOScale(profTransform.localScale, movement.duration).OnComplete(() => profAnimator.SetTrigger(nextMovement.profAnim.ToString())));
                }
            }
            else
            {
                if (movement.movementType == ProfMovementEnum.move)
                {
                    if (nextMovement == null)
                        profMovement.Join(profTransform.DOMove(movement.targetTransform.position, movement.duration));
                    else
                        profMovement.Join(profTransform.DOMove(movement.targetTransform.position, movement.duration).OnComplete(() => profAnimator.SetTrigger(nextMovement.profAnim.ToString())));
                }
                else if (movement.movementType == ProfMovementEnum.turn)
                {
                    if (nextMovement == null)
                        profMovement.Join(profTransform.DORotate(movement.targetTransform.eulerAngles, movement.duration));
                    else
                        profMovement.Join(profTransform.DORotate(movement.targetTransform.eulerAngles, movement.duration).OnComplete(() => profAnimator.SetTrigger(nextMovement.profAnim.ToString())));
                }
                else
                {
                    if (nextMovement == null)
                        profMovement.Join(profTransform.DOScale(profTransform.localScale, movement.duration));
                    else
                        profMovement.Join(profTransform.DOScale(profTransform.localScale, movement.duration).OnComplete(() => profAnimator.SetTrigger(nextMovement.profAnim.ToString())));
                }
            }
            
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSkip)
        {
            StopCoroutine(dialogCoroutine);
            profMovement.Complete();
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

[System.Serializable]
public class ProfMovement
{
    public Transform targetTransform;
    public ProfMovementEnum movementType;
    public ProfAnim profAnim;
    public float duration;
    public bool append;
}

public enum ProfMovementEnum
{
    move,
    stop,
    turn
}

public enum ProfAnim
{
    idle,
    idleLooking,
    walk,
    noneOf
}