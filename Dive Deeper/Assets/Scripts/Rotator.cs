using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed = 5f;

    void LateUpdate()
    {
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }
}
