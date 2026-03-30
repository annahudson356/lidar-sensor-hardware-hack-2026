// FreeCam.cs
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float moveSpeed   = 5f;
    public float fastSpeed   = 15f;
    public float mouseSensitivity = 2f;

    float rotX = 0f;
    float rotY = 0f;

    void Start()
    {
        // Initialize rotation from current camera angle
        rotX = transform.eulerAngles.y;
        rotY = transform.eulerAngles.x;
    }

    void Update()
    {
        // Only look around while right mouse button is held
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;

            rotX += Input.GetAxis("Mouse X") * mouseSensitivity;
            rotY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotY  = Mathf.Clamp(rotY, -89f, 89f); // prevent flipping upside down

            transform.rotation = Quaternion.Euler(rotY, rotX, 0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }

        // Speed boost while holding Shift
        float speed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : moveSpeed;

        // WASD + Q/E movement
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),          // A/D
            (Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.Q) ? 1f : 0f), // up/down
            Input.GetAxis("Vertical")             // W/S
        );

        transform.Translate(move * speed * Time.deltaTime, Space.Self);
    }
}