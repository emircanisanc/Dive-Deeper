using System.Collections;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class GunBase : WeaponBaseAbstract, IBackfireable
{
    protected Animator animator;
    [SerializeField] protected AudioClipsSO fireClips;
    [SerializeField] protected AudioClipsSO reloadClips;
    [SerializeField] protected GameObject muzzle;
    protected AudioSource audioSource;
    [Header("Visuals")]
    // [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] protected Transform firePoint;
    
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


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = AudioManager.Instance.SoundVolume;
        audioSource.playOnAwake = false;
        AudioManager.Instance.OnSoundVolumeChanged += ChangeSoundVolume;
    }

    private void ChangeSoundVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public override void HandleSecondFire(Transform cam)
    {

    }

    public override void HandleReleaseSecondFire()
    {

    }

    protected IEnumerator HandleMuzzle()
    {
        muzzle.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        muzzle.SetActive(false);
    }

    protected override void OnDisable() {
        base.OnDisable();
        if (muzzle != null)
        {
            muzzle.SetActive(false);
        }
            
    }

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
            Vector3 dir = cam.forward + UnityEngine.Random.insideUnitSphere * sprayAmount;
            Vector3 startPos = cam.position;
            if (!muzzle.activeSelf)
            {
                StartCoroutine(HandleMuzzle());
            }
                
            SendLine(dir); // DEBUG LINE
            FireBullet(startPos, dir);
            nextAttackTime = Time.timeSinceLevelLoad + fireRate;
            return true;
        }
        return false;
    }

    protected void FireBullet(Vector3 start, Vector3 dir)
    {
        audioSource.PlayOneShot(fireClips.RandomAudioClip);
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
        if (currentAmmo == 0 && CanReload)
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
        if (CanReload)
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
        reloadTime = maxReloadTime;
        audioSource.PlayOneShot(reloadClips.RandomAudioClip);
        OnReloadTimeChanged?.Invoke(reloadTime);
        while (reloadTime > 0)
        {
            yield return new WaitForSeconds(0f);
            reloadTime -= Time.deltaTime;
            OnReloadTimeChanged?.Invoke(reloadTime);
        }
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

    protected virtual void SendLine(Vector3 dir)
    {
        float min = UnityEngine.Random.Range(0.1f, 0.3f);
        //Vector3 start = firePoint.position + dir * min;
        /* trailRenderer.transform.position = start;
        trailRenderer.Clear(); // clear existing positions from the trail renderer
        trailRenderer.enabled = true; // enable the trail renderer */

        Vector3 end = CalculateBulletTargetPos(Camera.main.transform);
        Vector3 start = firePoint.position + (end - firePoint.position).normalized;
        /* trailRenderer.AddPosition(start);
        trailRenderer.AddPosition(end); */

        // StartCoroutine(DisableTrailAfterDelay(end)); // disable the trail renderer after a delay
        TrailManager.Instance.CreateTrail(start, end, Color.blue);
    }

    protected virtual Vector3 CalculateBulletTargetPos(Transform cam)
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

        // trailRenderer.enabled = false; // disable the trail renderer
    }

    protected virtual void OnEnable()
    {
        if (currentAmmo == 0 && CanReload)
        {
            StartReload();
        }
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
