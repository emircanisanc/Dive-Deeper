using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Message : MonoBehaviour
{
    static Message instance;


    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI tmpMessage;


    string lastString;
    bool isMessageActive;
    Sequence sequence;

    void Awake()
    {
        instance = this;    
    }


    private void ShowMessage(string message)
    {
        if (isMessageActive && message != lastString)
        {
            sequence.Kill();
            tmpMessage.SetText(message);
            canvasGroup.alpha = 1f;
            sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(1, 1.5f));
            sequence.Append(canvasGroup.DOFade(0, 0.5f));
            sequence.OnComplete(() => isMessageActive = false);
        }
        else
        {
            tmpMessage.SetText(message);
            sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(1, 0.5f));
            sequence.Append(canvasGroup.DOFade(1, 1.5f));
            sequence.Append(canvasGroup.DOFade(0, 0.5f));
            sequence.OnComplete(() => isMessageActive = false);
        }
        isMessageActive = true;
        lastString = message;
    }


    public static void ShowMessageInstance(string message)
    {
        if (instance)
            instance.ShowMessage(message);
        else
            Debug.Log("There is no instance of Message Class");
    }
}
