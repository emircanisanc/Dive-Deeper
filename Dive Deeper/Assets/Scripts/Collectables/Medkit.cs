using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : CollectableBase
{
    [SerializeField] float healAmount = 20;

    protected override void Collect(Collider other)
    {
        PlayerHealth.Instance.Heal(healAmount);
        base.Collect(other);
    }
}
