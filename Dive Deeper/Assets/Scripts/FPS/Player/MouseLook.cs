using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 100f;

    public float WantedCameraXRotation{get; set;}
    public float WantedYRotation{get; set;}


    private float currentCameraXRotation;
    private float currentYRotation;
    
    private float rotationYVelocity;
    [SerializeField] private float yRotationSpeed;
    private float cameraXVelocity;
    [SerializeField] private float xCameraSpeed;

    void Awake()
    {
        currentYRotation = playerBody.eulerAngles.y;
        WantedYRotation = currentYRotation;

        currentCameraXRotation = transform.eulerAngles.x;
        WantedCameraXRotation = currentCameraXRotation;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MouseControl();
        /* float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX); */

    }

    private void MouseControl()
    {
        WantedYRotation += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        WantedCameraXRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        WantedCameraXRotation = Mathf.Clamp(WantedCameraXRotation, -90f, 90f);

        currentYRotation = Mathf.SmoothDamp(currentYRotation, WantedYRotation, ref rotationYVelocity, yRotationSpeed);
        currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, WantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);

        playerBody.rotation = Quaternion.Euler(0, currentYRotation, 0);
        transform.localRotation = Quaternion.Euler(currentCameraXRotation, 0, 0);
    }

}
