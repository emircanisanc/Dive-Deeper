using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletKit : CollectableBase
{
    [SerializeField] BulletType bulletType;
    [SerializeField] int bulletAmount = 50;

    protected override void Collect(Collider other)
    {
        if (other.TryGetComponent<WeaponHandler>(out var weaponHandler))
        {
            weaponHandler.AddBullet(bulletType, bulletAmount);
        }
        base.Collect(other);
    }
}
