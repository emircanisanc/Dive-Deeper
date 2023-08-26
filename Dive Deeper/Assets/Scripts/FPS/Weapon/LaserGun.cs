using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : GunBase
{
    [SerializeField] protected AudioClipsSO fireGrenadeClips;
    
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

    protected override void Start()
    {
        base.Start();
        InGameUI.Instance.SetGreandeUI(true);
        InGameUI.Instance.SetGrenadeCount(grenadeAmount);
    }

    public override void HandleSecondFire(Transform cam)
    {
        if (grenadeAmount > 0 && !isReloading)
        {
            if (Time.time >= nextGrenadeTime && canFireGrenade)
            {
                animator.SetTrigger("Grenade");
                canFireGrenade = false;
                nextGrenadeTime = Time.time + 0.3f;
                grenadeAmount--;
                InGameUI.Instance.SetGrenadeCount(grenadeAmount);
                audioSource.PlayOneShot(fireGrenadeClips.RandomAudioClip);
                Instantiate(grenadePrefab, grenadePoint.position, cam.rotation);
            }
        }
    }

    public override void StartReload()
    {
        base.StartReload();
        animator.SetTrigger("Reload");
    }




    protected override void OnEnable()
    {
        base.OnEnable();
        if (InGameUI.Instance)
        {
            InGameUI.Instance.SetGreandeUI(true);
            InGameUI.Instance.SetGrenadeCount(grenadeAmount);
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
        InGameUI.Instance.SetGrenadeCount(grenadeAmount);
    }
}
