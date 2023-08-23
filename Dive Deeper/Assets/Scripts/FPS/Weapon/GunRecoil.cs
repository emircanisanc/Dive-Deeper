using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private WeaponHandler weaponHandler;
    private GunBase gun;

    private float currentRecoilYPos;
    private float currentRecoilXPos;

    [Header("Gun Recoil")]
    private float recoilAmountX;
    private float recoilAmountY;
    private float maxRecoilTime;
    private float timePressed;
    private bool clear;

    [Header("Movement Recoil")]
    [SerializeField] private float movementSprayMultiplier = 0.1f;

    


    void Awake()
    {
        weaponHandler.OnWeaponSwitched += ChangeTargetWeapon;
        playerMovement.OnMove += ApplyRecoil;
    }

    private void ChangeTargetWeapon(WeaponBaseAbstract newWeapon)
    {
        if (newWeapon is GunBase newGun)
        {
            if(gun != null)
            {
                gun.OnFire -= ApplyRecoil;
                gun.OnFire -= RecoilMath;
                gun.OnRelease -= ClearRecoil;
            }
            newGun.OnFire += ApplyRecoil;
            newGun.OnFire += RecoilMath;
            newGun.OnRelease += ClearRecoil;

            recoilAmountX = newGun.RecoilAmountX;
            recoilAmountY = newGun.RecoilAmountY;
            maxRecoilTime = newGun.MaxRecoilTime;

            gun = newGun;
        }
    }

    private void ApplyRecoil()
    {
        timePressed += Time.deltaTime * gun.RecoilMultiplier;
        timePressed = timePressed >= maxRecoilTime ? maxRecoilTime : timePressed;
    }
    private void ApplyRecoil(Vector2 movement)
    {
        if (gun)
            gun.SprayAmount = movement.magnitude * movementSprayMultiplier;
    }

    private  void ClearRecoil()
    {
        timePressed = 0f;
        clear = true;
    }


    private void RecoilMath()
    {
        clear = false;

        float recoilXCalculation = ((Random.value - 0.5f) / 2) * (timePressed < maxRecoilTime ? recoilAmountX / 3 : recoilAmountX * (timePressed * 2));
        currentRecoilXPos = recoilXCalculation;
        
        float recoilYCalculation = (Mathf.Abs(Random.value - 0.5f) / 2) * (timePressed >= maxRecoilTime ? recoilAmountY / 20 : recoilAmountY * (timePressed * 2));
        currentRecoilYPos = recoilYCalculation;

        mouseLook.WantedCameraXRotation -= Mathf.Abs(currentRecoilYPos) * Time.deltaTime;
        mouseLook.WantedYRotation -= currentRecoilXPos * Time.deltaTime;
    }

}
