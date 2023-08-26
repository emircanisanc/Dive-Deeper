using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageImage : Singleton<DamageImage>
{
    public Image damageImage;
    public float clearSpeed = 1f;
    float NetAlpha { get { return alpha / 250f; } }
    private float alpha = 0f; // Initialize alpha to 0

    public void ShowImage(float damage)
    {
        alpha += damage * 3f;
    }

    void Update()
    {
        alpha -= Time.deltaTime * clearSpeed;
        alpha = Mathf.Clamp(alpha, 0f, 255f);

        // Create a new color with the updated alpha value
        Color imageColor = damageImage.color;
        imageColor.a = NetAlpha;

        // Assign the updated color back to the damageImage
        damageImage.color = imageColor;
    }
}
