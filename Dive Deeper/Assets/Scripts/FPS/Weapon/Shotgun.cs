using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunBase
{
    public int ammoPerShot = 3;
    public Transform[] firepoints;

    protected override void Start()
    {
        base.Start();
        InGameUI.Instance.SetGreandeUI(false);
    }

    public override void HandleSecondFire(Transform cam)
    {

    }

    public override void HandleReleaseSecondFire()
    {

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (InGameUI.Instance)
            InGameUI.Instance.SetGreandeUI(false);
    }

    public override bool HandleFire(Transform cam)
    {
        if (!canFire)
        {
            return false;
        }
        if (!isShooting)
        {
            isShooting = true;
        }
        if (Time.timeSinceLevelLoad >= nextAttackTime)
        {
            /* PassiveMuzzle();
            ActiveMuzzle();
 */
            Vector3 startPos = cam.position;
            Vector3 dir = cam.forward + UnityEngine.Random.insideUnitSphere * sprayAmount;
            OnFire?.Invoke();
            audioSource.PlayOneShot(fireClips.RandomAudioClip);
            ReduceAmmo();
            if (!muzzle.activeSelf)
            {
                StartCoroutine(HandleMuzzle());
            }
            OnCurrentAmmoReduced?.Invoke(currentAmmo);
            for (int i = 0; i < ammoPerShot; i++)
            {
                var newDir = dir + new Vector3(RandomDirMultiplier, RandomDirMultiplier, RandomDirMultiplier) * UnityEngine.Random.Range(-0.1f, 0.1f);
                SendLine(newDir, i); // DEBUG LINE
                HandleHit(Physics.RaycastAll(startPos, newDir, range, targetLayer));
            }
            nextAttackTime = Time.timeSinceLevelLoad + fireRate;
            return true;
        }
        return false;
    }

    public float RandomDirMultiplier => Random.Range(0.3f, 0.4f) * RandomSign;
    public float RandomSign => Random.Range(0, 2) == 0 ? 1f : -1f;

    protected void SendLine(Vector3 dir, int firePoint)
    {
        float min = UnityEngine.Random.Range(0.1f, 0.2f);
        //Vector3 start = firePoint.position + dir * min;
        Transform cam = Camera.main.transform;
        Vector3 end = firepoints[firePoint].position + firepoints[firePoint].forward * 10;
        var hits = Physics.RaycastAll(cam.position, firepoints[firePoint].forward, range, targetLayer);
        if (hits.Length > 0)
            end = hits[0].point;

        Vector3 start = firepoints[firePoint].position + (end - firepoints[firePoint].position).normalized * 0.5f;

        // StartCoroutine(DisableTrailAfterDelay(end)); // disable the trail renderer after a delay
        TrailManager.Instance.CreateShotgunTrail(start, end);
    }

    /* protected override void SendLine(Vector3 dir)
    {
        float min = UnityEngine.Random.Range(0.1f, 0.2f);
        //Vector3 start = firePoint.position + dir * min;
        Transform cam = Camera.main.transform;
        Vector3 end = cam.position + dir * 10;
        var hits = Physics.RaycastAll(cam.position, dir, range, targetLayer);
        if (hits.Length > 0)
            end = hits[0].point;

        Vector3 start = firePoint.position + (end - firePoint.position).normalized * 1f;

        // StartCoroutine(DisableTrailAfterDelay(end)); // disable the trail renderer after a delay
        TrailManager.Instance.CreateShotgunTrail(start, end);
    } */
}
