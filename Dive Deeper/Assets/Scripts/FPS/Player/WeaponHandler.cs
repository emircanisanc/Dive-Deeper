using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class WeaponHandler : Singleton<WeaponHandler>
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private WeaponBaseAbstract weapon;
    public WeaponBaseAbstract Weapon => weapon;
    public Action<WeaponBaseAbstract> OnWeaponSwitched;

    [SerializeField] List<WeaponBaseAbstract> weapons;

    protected override void Awake()
    {
        base.Awake();
        if (weapon)
            SwitchWeapon(weapon);
    }

    void Start()
    {
        PlayerHealth.Instance.OnPlayerDied += DisableWeapon;
        GameManager.Instance.OnGameEnd += DisableWeapon;
    }

    void Update()
    {
        if (Input.GetAxis("Scrool") < 0 && weapons.Count > 0)
        {
            if (Weapon != weapons[0])
            {
                SwitchWeapon(weapons[0]);   
            }
        }
        else if (Input.GetAxis("Scrool") > 0 && weapons.Count > 1)
        {
            if (Weapon != weapons[1])
            {
                SwitchWeapon(weapons[1]);   
            }
        }

        if (weapon == null)
            return;

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

            if (Input.GetMouseButtonDown(1))
            {
                weapon.HandleSecondFire(camTransform);
            }
            if (Input.GetMouseButtonUp(1))
            {
                weapon.HandleReleaseSecondFire();
            }
        }


        if (Input.GetAxis("Reload") > 0 && weapon.CanReload)
            weapon.StartReload();
    }

    private void SwitchWeapon(WeaponBaseAbstract newWeapon)
    {
        if (weapon)
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

    public void AddBullet(BulletType bulletType, int bulletAmount)
    {
        var filteredWeapons = weapons.Where(x => x.BulletType == bulletType).ToList();
        if (filteredWeapons.Count > 0)
        {
            filteredWeapons[0].AddBullet(bulletAmount);
        }
    }

    public void AddWeapon(GameObject weaponPf)
    {
        var newWeapon = Instantiate(weaponPf, Vector3.zero, Quaternion.identity);
        newWeapon.transform.SetParent(camTransform);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        weapons.Add(newWeapon.GetComponent<WeaponBaseAbstract>());
        SwitchWeapon(weapons[weapons.Count - 1]);
    }
}
