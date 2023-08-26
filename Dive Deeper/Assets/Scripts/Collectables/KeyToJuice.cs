using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyToJuice : CollectableBase
{
    protected override void Collect(Collider other)
    {
        GameManager.Instance.OpenDoor();
        AudioManager.Instance.PlayPickupAtPoint(transform.position);
        AfterCollect();
    }
}
