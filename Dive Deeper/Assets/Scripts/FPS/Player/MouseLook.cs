using System;
using UnityEngine;

public class MouseLook : Singleton<MouseLook>
{
    [SerializeField] private Transform playerBody;
    [SerializeField][Range(100, 300)] private float mouseSensitivity = 250f;
    public float Sensitivity
    {
        get { return (mouseSensitivity - 100) / (300 - 100); }
        set { mouseSensitivity = Mathf.Lerp(100, 300, value); }
    }

    public float WantedCameraXRotation { get; set; }
    public float WantedYRotation { get; set; }


    private float currentCameraXRotation;
    private float currentYRotation;

    private float rotationYVelocity;
    [SerializeField] private float yRotationSpeed;
    private float cameraXVelocity;
    [SerializeField] private float xCameraSpeed;

    protected override void Awake()
    {
        base.Awake();
        Sensitivity = PlayerPrefs.GetFloat("sensitivity", 0.5f);
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
    }

    private void MouseControl()
    {
        WantedYRotation += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        WantedCameraXRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        WantedCameraXRotation = Mathf.Clamp(WantedCameraXRotation, -90f, 90f);

        currentYRotation = Mathf.SmoothDamp(currentYRotation, WantedYRotation, ref rotationYVelocity, yRotationSpeed);
        currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, WantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);

        Quaternion playerRotation = Quaternion.Euler(0, currentYRotation, 0).normalized;
        Quaternion cameraRotation = Quaternion.Euler(currentCameraXRotation, 0, 0).normalized;

        playerBody.rotation = playerRotation;
        transform.localRotation = cameraRotation;
    }


}

