using UnityEngine;
using System;

public class WeaponHandler : Singleton<WeaponHandler>
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private WeaponBaseAbstract weapon;
    public WeaponBaseAbstract Weapon => weapon;
    public Action<WeaponBaseAbstract> OnWeaponSwitched;

    [SerializeField] WeaponBaseAbstract[] weapons;

    protected override void Awake()
    {
        base.Awake();
        SwitchWeapon(weapon);
    }

    void Start()
    {
        PlayerHealth.Instance.OnPlayerDied += DisableWeapon;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (Weapon == weapons[0])
            {
                SwitchWeapon(weapons[1]);
            }
            else
            {
                SwitchWeapon(weapons[0]);
            }
        }
        if (Input.GetAxis("Fire1") > 0)
        {
            if (weapon.CanFire)
            {
                weapon.HandleFire(camTransform);
            }

        }
        else
        {
            if (weapon.IsShooting)
            {
                weapon.HandleReleaseFire();
            }
        }

        if (Input.GetAxis("Reload") > 0 && weapon.CanFire)
            weapon.StartReload();
    }

    private void SwitchWeapon(WeaponBaseAbstract newWeapon)
    {
        weapon.gameObject.SetActive(false);
        weapon = newWeapon;
        newWeapon.gameObject.SetActive(true);
        OnWeaponSwitched?.Invoke(weapon);
    }

    private void DisableWeapon()
    {
        enabled = false;
        if (weapon.IsShooting)
        {
            weapon.HandleReleaseFire();
        }
    }
}
