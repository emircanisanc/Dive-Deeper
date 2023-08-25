using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    public Transform weaponCamera; // Reference to your weapon camera
    public float bobbingSpeed = 0.2f;
    public float bobbingAmount = 0.1f;
    public bool isHeadBobbing = true;

    private Vector3 originalCameraPosition;
    private float timer = 0f;

    // Reference to the PlayerMovement script
    private PlayerMovement playerMovement;

    void Start()
    {
        originalCameraPosition = weaponCamera.localPosition;
        playerMovement = FindObjectOfType<PlayerMovement>();
    }


    // Event handler for the OnMove event
    private void LateUpdate()
    {
        float waveslice = 0.0f;
        float horizontal = playerMovement.Horizontal; // Use moveDirection from OnMove event
        float vertical = playerMovement.Vertical;

        Vector3 cSharpConversion = weaponCamera.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed * Time.deltaTime;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            if (isHeadBobbing)
                cSharpConversion.y = originalCameraPosition.y + translateChange;
            else
                cSharpConversion.x = translateChange;
        }
        else
        {
            if (isHeadBobbing)
                cSharpConversion.y = originalCameraPosition.y;
            else
                cSharpConversion.x = 0;
        }

        weaponCamera.localPosition = cSharpConversion;
    }
}
