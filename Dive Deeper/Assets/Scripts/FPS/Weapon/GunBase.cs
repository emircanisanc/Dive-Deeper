using System.Collections;
using UnityEngine;
using System.Linq;

public class GunBase : WeaponBaseAbstract, IBackfireable
{
    [Header("Visuals")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform firePoint;

    [Header("Reload Settings")]
    [SerializeField] private float maxReloadTime = 2;
    protected float nextAttackTime;
    
    [Header("Fire Settings")]
    [SerializeField] protected float fireRate = 0.1f;
    [SerializeField] protected float range = 30f;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilMultiplier = 1;
    [SerializeField] private float recoilAmountX = 150;
    [SerializeField] private float recoilAmountY = 150;
    [SerializeField] private float maxRecoilTime = 0.7f;
    public float RecoilMultiplier => recoilMultiplier;
    public float RecoilAmountX => recoilAmountX;
    public float RecoilAmountY => recoilAmountY;
    public float MaxRecoilTime => maxRecoilTime;

    [Header("Spray Settings")]
    [SerializeField] private float sprayMultiplier = 1;
    protected float sprayAmount;
    public float SprayAmount { get => sprayAmount; set { sprayAmount = value * sprayMultiplier; } }



    public override bool HandleFire(Transform cam)
    {
        if (!canFire)
        {
            return false;
        }
        OnFire?.Invoke();
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
            SendLine(dir); // DEBUG LINE
            FireBullet(startPos, dir);
            nextAttackTime = Time.timeSinceLevelLoad + fireRate;
            return true;
        }
        return false;
    }

    protected void FireBullet(Vector3 start, Vector3 dir)
    {
        HandleHit(Physics.RaycastAll(start, dir, range, targetLayer));
        ReduceAmmo();
        OnCurrentAmmoReduced?.Invoke(currentAmmo);
    }
    protected void HandleHit(RaycastHit[] hits)
    {
        bool canBreak = false;
        // Sort hits by distance in ascending order (nearest to farthest)
        hits = hits.OrderBy(hit => hit.distance).ToArray();
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.ApplyDamage(damage);
                canBreak = true;
            }
            if (hit.collider.TryGetComponent<IHitable>(out var hitable))
            {
                hitable.Hit(hit);
                canBreak = true;
            }
            if (canBreak)
                break;
        }
    }

    protected override void ReduceAmmo()
    {
        currentAmmo--;
        if (currentAmmo == 0)
            StartReload();
    }

    public override void HandleReleaseFire()
    {
        StopFire();
    }

    protected void StopFire()
    {
        OnRelease?.Invoke();
        isShooting = false;
        /* PassiveMuzzle(); */
    }

    protected override void OnClipFinished()
    {
        StopFire();
        StartReload();
    }

    public override void StartReload()
    {
        isReloading = true;
        StopFire();
        canFire = false;
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        Debug.Log("reloading");
        yield return new WaitForSeconds(maxReloadTime);
        Debug.Log("reload ended");
        isReloading = false;
        canFire = true;
        totalAmmo += currentAmmo;
        currentAmmo = 0;
        int bulletToAdd = Mathf.Min(clipSize, totalAmmo);
        currentAmmo = bulletToAdd;
        totalAmmo -= bulletToAdd;
        OnTotalAmmoReduced?.Invoke(totalAmmo);
        OnCurrentAmmoReduced?.Invoke(currentAmmo);
    }

    private void OnDrawGizmosSelected()
    {
        Transform cam = Camera.main.transform;
        Debug.DrawLine(firePoint.position, firePoint.position + cam.forward * range);
    }

    protected void SendLine(Vector3 dir)
    {
        float min = UnityEngine.Random.Range(0.1f, 0.4f);
        Vector3 start = firePoint.position + dir * min;
        trailRenderer.transform.position = start;
        trailRenderer.Clear(); // clear existing positions from the trail renderer
        trailRenderer.enabled = true; // enable the trail renderer

        Vector3 end = CalculateBulletTargetPos(Camera.main.transform);
        trailRenderer.AddPosition(start);
        trailRenderer.AddPosition(end);

        StartCoroutine(DisableTrailAfterDelay(end)); // disable the trail renderer after a delay
    }

    protected Vector3 CalculateBulletTargetPos(Transform cam)
    {
        Vector3 targetPos = cam.position + cam.forward * 10;
        var hits = Physics.RaycastAll(cam.position, cam.forward, range, targetLayer);
        if (hits.Length > 0)
            targetPos = hits[0].point;
        return targetPos;
    }

    private IEnumerator DisableTrailAfterDelay(Vector3 end)
    {
        yield return new WaitForSeconds(0.03f);

        trailRenderer.enabled = false; // disable the trail renderer
    }

}




/* [SerializeField] private GameObject[] muzzles;
    private int muzzleIndex = 0;
 */


/* protected void ActiveMuzzle()
{
    muzzleIndex = Random.Range(0, muzzles.Length);
    muzzles[muzzleIndex].SetActive(true);
}

protected void PassiveMuzzle()
{
    muzzles[muzzleIndex].SetActive(false);
} */
