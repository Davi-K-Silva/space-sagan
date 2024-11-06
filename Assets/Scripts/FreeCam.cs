using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float movementSpeed = 10f;  // Movement speed of the camera
    public float lookSensitivity = 2f; // Sensitivity for mouse look
    public float sprintMultiplier = 2f; // Speed multiplier when holding Shift
    public bool lockCursor = true;      // Locks the mouse cursor in place

    private float yaw = 0f;  // Horizontal rotation
    private float pitch = 0f;  // Vertical rotation

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;  // Locks the cursor in the center of the screen
            Cursor.visible = false;  // Hides the cursor
        }
    }

    void Update()
    {
        // Handle mouse look
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // Prevent flipping over

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // Handle movement
        float speed = movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;  // A/D or Left/Right arrows
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;    // W/S or Up/Down arrows
        float moveY = 0f;

        // Move up/down with Space and Left Ctrl
        if (Input.GetKey(KeyCode.Space)) moveY += speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl)) moveY -= speed * Time.deltaTime;

        Vector3 movement = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;
        transform.position += movement;
    }
}
