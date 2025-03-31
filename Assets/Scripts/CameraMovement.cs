using UnityEngine;
using UnityEngine.InputSystem; // Import the new Input System

public class CameraMovement : MonoBehaviour
{
    public float zoomSpeed = 2.5f;   // Zoom speed
    public float minZoom = 10f;    // Minimum zoom (FOV)
    public float maxZoom = 60f;    // Maximum zoom (FOV)

    private new Camera camera; // Reference to the camera

    void Start()
    {
        // Get the camera component attached to the GameObject this script is attached to
        camera = Camera.main;
    }

    void Update()
    {
        // Scroll Wheel Zoom (Change FOV)
        float scrollInput = Mouse.current.scroll.ReadValue().y;  // Get the scroll wheel input
        if (scrollInput != 0)
        {
            // Zoom in or out
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - scrollInput * zoomSpeed, minZoom, maxZoom);
        }
    }
}
