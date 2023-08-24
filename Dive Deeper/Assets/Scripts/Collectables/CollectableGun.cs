using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableGun : CollectableBase
{
    [SerializeField] GameObject weaponPf;
    [SerializeField] GameObject weaponInfoUI;
    bool isPaused;
    protected override void Collect(Collider other)
    {
        if (other.TryGetComponent<WeaponHandler>(out var weaponHandler))
        {
            weaponInfoUI.SetActive(true);
            weaponHandler.AddWeapon(weaponPf);
            isPaused = true;
            GameManager.Instance.canPauseGame = false;
            Time.timeScale = 0f;
        }
    }

    void LateUpdate()
    {
        if (!isPaused)
            return;

        if (Input.anyKeyDown)
        {
            weaponInfoUI.SetActive(false);
            Time.timeScale = 1f;
            GameManager.Instance.SetCanPauseGame();
            AfterCollect();
        }
    }
}
