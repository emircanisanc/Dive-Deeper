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
            GameManager.Instance.PauseGame();
        }
    }

    void Update()
    {
        if (!isPaused)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            weaponInfoUI.SetActive(false);
            GameManager.Instance.UnPauseGame();
            AfterCollect();
        }
    }
}
