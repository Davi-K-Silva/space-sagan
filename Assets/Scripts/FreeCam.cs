using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float movementSpeed = 10f;  // Movement speed of the camera
    public float lookSensitivity = 2f; // Sensitivity for mouse look
    public float sprintMultiplier = 2f; // Speed multiplier when holding Shift
    public bool lockCursor = true;      // Locks the mouse cursor in place

    private float yaw = 0f;  // Horizontal rotation
    private float pitch = 0f;  // Vertical rotation
    private bool isDragging = false; // Tracks if the left mouse button is held down

    void Start()
    {
        SetCursorState(lockCursor);
    }

    void Update()
    {
        // Check if the left mouse button is held down to toggle camera control
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            SetCursorState(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            SetCursorState(false);
        }

        // Only handle mouse look and movement if the left mouse button is held down
        if (isDragging)
        {
            HandleMouseLook();
            HandleMovement();
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // Prevent flipping over

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }

    void HandleMovement()
    {
        float speed = movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;  // A/D or Left/Right arrows
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;    // W/S or Up/Down arrows
        float moveY = 0f;

        if (Input.GetKey(KeyCode.Space)) moveY += speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl)) moveY -= speed * Time.deltaTime;

        Vector3 movement = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;
        transform.position += movement;
    }

    void SetCursorState(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }
}
