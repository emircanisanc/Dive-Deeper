using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEffect : MonoBehaviour
{
    [SerializeField] GameObject effectPf;

    static GunEffect instance;

    void Awake()
    {
        instance = this;
    }

    public static void CreateEffect(Vector3 hitPoint, Vector3 hitNormal)
    {
        var effect = Instantiate(instance.effectPf);
        effect.transform.position = hitPoint;
        effect.transform.eulerAngles = hitNormal;
        effect.SetActive(true);
    }
}
