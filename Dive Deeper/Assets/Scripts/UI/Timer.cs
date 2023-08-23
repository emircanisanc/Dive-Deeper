using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField] private Slider speedSlider;
    private float maxTime = 30f;
    private bool isTime;

    private void Update()
    {
        if (isTime)
        {
            SpeedsSlider();
        }

    }

    private void SpeedsSlider()
    {
        speedSlider.value -= Time.deltaTime / maxTime;

    }
    public void ActiveSlider()
    {
        isTime = true;
        speedSlider.gameObject.SetActive(true);
        speedSlider.value = 1;
    }
    public void DeActiveSlider()
    {
        if (speedSlider.value <= 0)
        {
            isTime = false;
            speedSlider.gameObject.SetActive(false);
            speedSlider.value = 1;
        }

    }

}

