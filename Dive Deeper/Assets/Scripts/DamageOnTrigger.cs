using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    public float damage = 50;
    void OnTriggerEnter(Collider other)
    {
        //  Debug.Log("aaa");
        if (other.TryGetComponent<PlayerHealth>(out var damageable))
        {


            damageable.ApplyDamage(damage);
        }
    }
}
