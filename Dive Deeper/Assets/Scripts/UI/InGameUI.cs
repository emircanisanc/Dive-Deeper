using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] Float health;
    [SerializeField] Float maxHealth;
    [SerializeField] TextMeshProUGUI bulletTMP;
    [SerializeField] TextMeshProUGUI maxBulletTMP;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthTMP;
    [SerializeField] TextMeshProUGUI maxHealthTMP;

    WeaponBaseAbstract lastWeapon;

    void Start()
    {
        lastWeapon = WeaponHandler.Instance.Weapon;
        lastWeapon.OnCurrentAmmoReduced += UpdateBullet;
        lastWeapon.OnTotalAmmoReduced += UpdateMaxBullet;
        UpdateBullet(lastWeapon.CurrentAmmo);
        UpdateMaxBullet(lastWeapon.TotalAmmo);

        WeaponHandler.Instance.OnWeaponSwitched += UpdateListeners;

        UpdateHealth(health.Value);
        UpdateMaxHealth(maxHealth.Value);
    }

    private void UpdateListeners(WeaponBaseAbstract newWeapon)
    {
        if (lastWeapon != null)
        {
            lastWeapon.OnCurrentAmmoReduced -= UpdateBullet;
            lastWeapon.OnTotalAmmoReduced -= UpdateMaxBullet;
        }

        if (newWeapon != null)
        {
            lastWeapon = newWeapon;
            lastWeapon.OnCurrentAmmoReduced += UpdateBullet;
            lastWeapon.OnTotalAmmoReduced += UpdateMaxBullet;
            UpdateBullet(lastWeapon.CurrentAmmo);
            UpdateMaxBullet(lastWeapon.TotalAmmo);
        }
        else
        {
            UpdateBullet(0);
            UpdateMaxBullet(0);
        }
    }

    void OnEnable()
    {
        health.OnValueChanged.AddListener(UpdateHealth);
    }

    void OnDisable()
    {
        if (WeaponHandler.Instance)
        {
            WeaponHandler.Instance.OnWeaponSwitched -= UpdateListeners;
        }
        if (lastWeapon)
        {
            lastWeapon.OnCurrentAmmoReduced -= UpdateBullet;
            lastWeapon.OnTotalAmmoReduced -= UpdateMaxBullet;
        }
        health.OnValueChanged.RemoveListener(UpdateHealth);
    }

    private void UpdateBullet(int value)
    {
        bulletTMP.SetText(value.ToString());
    }

    private void UpdateMaxBullet(int value)
    {
        maxBulletTMP.SetText(value.ToString());
    }

    private void UpdateMaxHealth(float value)
    {
        maxHealthTMP.SetText(value.ToString());
    }

    private void UpdateHealth(float value)
    {
        healthBar.fillAmount = value / maxHealth.Value;
        healthTMP.SetText(value.ToString());
    }

}
