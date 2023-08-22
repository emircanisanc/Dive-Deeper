using UnityEngine;
using System;

/// <summary>
/// Abstract base class for weapons in the game.
/// </summary>
public abstract class WeaponBaseAbstract : MonoBehaviour
{
    [Header("Ammo Settings")]
    [SerializeField] protected int maxAmmo = 200; // Maximum ammo capacity.
    [SerializeField] protected int clipSize = 25; // Maximum clip capacity.
    public int MaxAmmo => maxAmmo; 
    protected int currentAmmo; // Current ammo count.
    public int CurrentAmmo => currentAmmo;
    protected int totalAmmo; // Current total ammo count.
    public int TotalAmmo => totalAmmo;
    public Action<int> OnCurrentAmmoReduced;
    public Action<int> OnTotalAmmoReduced;

    [SerializeField] protected float damage = 5; // Damage per shot.
    [SerializeField] protected LayerMask targetLayer; // Layer mask for targeting.

    // Events to handle firing and releasing.
    public Action OnFire;
    public Action OnRelease;

    // State variables.
    protected bool isShooting; // Indicates if the weapon is currently firing.
    public bool IsShooting => isShooting;

    protected bool canFire; // Indicates if the weapon can fire.
    public bool CanFire => canFire && currentAmmo > 0;

    private bool isInitialized;


    protected virtual void Start()
    {
        if (isInitialized)
            return;

        isInitialized = true;
        InitWeapon();
    }

    /// <summary>
    /// Handles firing the weapon.
    /// </summary>
    /// <param name="cam">The camera's transform.</param>
    /// <returns>Returns true if the weapon fired successfully.</returns>
    public abstract bool HandleFire(Transform cam);

    /// <summary>
    /// Handles releasing the fire button.
    /// </summary>
    public abstract void HandleReleaseFire();

    /// <summary>
    /// Starts the reload process for the weapon.
    /// </summary>
    public abstract void StartReload();

    /// <summary>
    /// Reduces the ammo count when a shot is fired.
    /// </summary>
    protected abstract void ReduceAmmo();

    /// <summary>
    /// Called when the weapon's clip is empty.
    /// </summary>
    protected abstract void OnClipFinished();

    /// <summary>
    /// Initialization logic for the weapon.
    /// </summary>
    protected virtual void InitWeapon()
    {
        currentAmmo = clipSize;
        totalAmmo = maxAmmo;
        canFire = true;
    }

}
