using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class InGameUI : Singleton<InGameUI>
{
    [SerializeField] Float health;
    [SerializeField] Float maxHealth;
    [SerializeField] TextMeshProUGUI bulletTMP;
    [SerializeField] TextMeshProUGUI maxBulletTMP;
    [SerializeField] TextMeshProUGUI bombTMP;
    [SerializeField] GameObject reloadBarParent;
    [SerializeField] Image reloadBar;
    [SerializeField] Image healthBar;

    [SerializeField] GameObject grenadePanel;
    [SerializeField] TextMeshProUGUI grenadeTMP;

    [Header("Pause Panel")]
    public GameObject pausePanel;
    public Slider soundSlider;
    public Slider musicSlider;
    public Slider sensitivitySlider;

    [Header("Win Panel")]
    public GameObject winPanel;

    [Header("Lose Panel")]
    public GameObject losePanel;

    WeaponBaseAbstract lastWeapon;

    public Action<float> OnSensitivityChanged;
    public string nextLevelName = "Map 2";
    float sensitivity;

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

    public void OpenNextLevel()
    {
        AudioManager.Instance.PlayButtonSound();
        SceneManager.LoadScene(nextLevelName);
    }

    public void OnSensitivitySliderChanged(float value)
    {
        MouseLook.Instance.Sensitivity = value;
    }

    public void OnSoundValueChanged(float value)
    {
        AudioManager.Instance.SoundVolume = value;
    }

    public void OnMusicValueChanged(float value)
    {
        AudioManager.Instance.MusicVolume = value;
    }

    public void RestartLevel()
    {
        AudioManager.Instance.PlayButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowPausePanel()
    {
        AudioManager.Instance.PlayButtonSound();
        sensitivitySlider.value = MouseLook.Instance.Sensitivity;
        soundSlider.value = AudioManager.Instance.SoundVolume;
        musicSlider.value = AudioManager.Instance.MusicVolume;
        Cursor.lockState = CursorLockMode.None;
        pausePanel.SetActive(true);
    }

    public void ClosePausePanel()
    {
        AudioManager.Instance.PlayButtonSound();
        PlayerPrefs.SetFloat("sensitivity", MouseLook.Instance.Sensitivity);
        PlayerPrefs.SetFloat("soundVolume", AudioManager.Instance.SoundVolume);
        PlayerPrefs.SetFloat("musicVolume", AudioManager.Instance.MusicVolume);
        PlayerPrefs.Save();
        Cursor.lockState = CursorLockMode.Locked;
        pausePanel.SetActive(false);
    }


    public void ShowWinGameUI()
    {
        winPanel.SetActive(true);
        Invoke(nameof(SetCursorVisible), 1.5f);
    }

    private void SetCursorVisible()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowLoseGameUI()
    {
        losePanel.SetActive(true);
        Invoke(nameof(SetCursorVisible), 1.5f);
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

    public void SetGreandeUI(bool visible)
    {
        grenadePanel.SetActive(visible);
    }

    public void SetGrenadeCount(int value)
    {
        grenadeTMP.SetText(value.ToString());
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

    private void UpdateBomb(int value)
    {
        bombTMP.SetText(value.ToString());
    }


    private void UpdateHealth(float value)
    {
        healthBar.fillAmount = value / maxHealth.Value;
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
