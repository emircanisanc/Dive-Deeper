using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : Singleton<InGameUI>
{
    [SerializeField] Float health;
    [SerializeField] Float maxHealth;
    [SerializeField] TextMeshProUGUI bulletTMP;
    [SerializeField] TextMeshProUGUI maxBulletTMP;
    [SerializeField] GameObject reloadBarParent;
    [SerializeField] Image reloadBar;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthTMP;
    [SerializeField] TextMeshProUGUI maxHealthTMP;

    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    WeaponBaseAbstract lastWeapon;

    void Start()
    {
        lastWeapon = WeaponHandler.Instance.Weapon;
        if (lastWeapon)
        {
            lastWeapon.OnCurrentAmmoReduced += UpdateBullet;
            lastWeapon.OnTotalAmmoReduced += UpdateMaxBullet;
            lastWeapon.OnReloadTimeChanged += UpdateReloadBar;

            UpdateBullet(lastWeapon.CurrentAmmo);
            UpdateMaxBullet(lastWeapon.TotalAmmo);
        }


        WeaponHandler.Instance.OnWeaponSwitched += UpdateListeners;

        UpdateHealth(health.Value);
        UpdateMaxHealth(maxHealth.Value);
    }

    public void OnContinueBtnPressed()
    {
        AudioManager.Instance.PlayButtonSound();
        GameManager.Instance.UnPauseGame();
    }

    public void OnRestartBtnPressed()
    {
        AudioManager.Instance.PlayButtonSound();
        RestartLevel();
    }

    public void OnSoundValueChanged(float value)
    {
        
    }

    public void OnMusicValueChanged(float value)
    {
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowPausePanel()
    {
        Cursor.lockState = CursorLockMode.None;
        pausePanel.SetActive(true);  
    }

    public void ClosePausePanel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pausePanel.SetActive(false);   
    }


    public void ShowWinGameUI()
    {

    }

    public void ShowLoseGameUI()
    {

    }

    private void UpdateListeners(WeaponBaseAbstract newWeapon)
    {
        if (lastWeapon != null)
        {
            lastWeapon.OnCurrentAmmoReduced -= UpdateBullet;
            lastWeapon.OnTotalAmmoReduced -= UpdateMaxBullet;
            lastWeapon.OnReloadTimeChanged -= UpdateReloadBar;
        }

        if (newWeapon != null)
        {
            lastWeapon = newWeapon;
            lastWeapon.OnCurrentAmmoReduced += UpdateBullet;
            lastWeapon.OnTotalAmmoReduced += UpdateMaxBullet;
            lastWeapon.OnReloadTimeChanged += UpdateReloadBar;
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
            lastWeapon.OnReloadTimeChanged -= UpdateReloadBar;
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

    private void UpdateReloadBar(float value)
    {
        if (value > 0)
        {
            reloadBarParent.SetActive(true);
            reloadBar.fillAmount = (lastWeapon.MaxReloadTime - value) / lastWeapon.MaxReloadTime;
        }
        else
        {
            reloadBarParent.SetActive(false);
        }
    }

}
