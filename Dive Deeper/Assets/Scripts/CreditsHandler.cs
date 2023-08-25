using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditsHandler : MonoBehaviour
{
    public RectTransform creditsText;
    public float targetY;
    public float duration;
    public GameObject activateAtEnd;

    void Start()
    {
        creditsText.DOAnchorPosY(targetY, duration).SetEase(Ease.Linear).OnComplete(() => activateAtEnd.SetActive(true));
    }
}
