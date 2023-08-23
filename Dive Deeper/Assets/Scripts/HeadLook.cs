using UnityEngine;

public class HeadLook : MonoBehaviour
{
    [SerializeField] float rotateXSpeed = 2.0f; // Sensitivity for vertical (up/down) rotation.
    [SerializeField] float rotateYSpeed = 2.0f; // Sensitivity for horizontal (left/right) rotation.
    [SerializeField] float minX = -90f; // Minimum vertical rotation angle.
    [SerializeField] float maxX = 90f;  // Maximum vertical rotation angle.
    [SerializeField] float minY = -45f; // Minimum horizontal rotation angle.
    [SerializeField] float maxY = 45f;  // Maximum horizontal rotation angle.

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input for horizontal and vertical rotation.
        float mouseX = Input.GetAxis("Mouse X") * rotateXSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotateYSpeed;

        // Calculate new vertical and horizontal rotations.
        rotationX -= mouseY;
        rotationY += mouseX;

        // Clamp the vertical rotation between the min and max angles.
        rotationX = Mathf.Clamp(rotationX, minX, maxX);

        // Clamp the horizontal rotation between the min and max angles.
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        // Apply rotations to the player's head.
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
