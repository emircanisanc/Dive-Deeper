using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : GunBase
{
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] Transform grenadePoint;

    [SerializeField] int maxGrenadeAmount = 5;
    int grenadeAmount;
    public int MaxGrenadeAmount => maxGrenadeAmount;
    public int GrenadeAmount => grenadeAmount;
    float nextGrenadeTime;
    bool canFireGrenade = true;

    protected override void InitWeapon()
    {
        base.InitWeapon();
        grenadeAmount = maxGrenadeAmount;
    }

    public override void HandleSecondFire(Transform cam)
    {
        if (grenadeAmount > 0)
        {
            if (Time.time >= nextGrenadeTime)
            {
                canFireGrenade = false;
                nextGrenadeTime = Time.time + 0.3f;
                grenadeAmount--;
                Instantiate(grenadePrefab, grenadePoint.position, cam.rotation);
            }
        }
    }

    public override void HandleReleaseSecondFire()
    {
        canFireGrenade = true;
    }

    public override void AddBullet(int bulletAmount)
    {
        base.AddBullet(bulletAmount);
        grenadeAmount = maxGrenadeAmount;
    }
}
