using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : CollectableBase
{
    protected override void Collect(Collider other)
    {
        GameManager.Instance.WinGame();
        base.Collect(other);
    }
}
