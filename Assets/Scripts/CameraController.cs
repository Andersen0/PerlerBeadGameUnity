using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    public float movementSpeed;
    public float normalSpeed;
    public float fastSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    public bool cameraInTopView = false;
    // public Vector3 rotationCenter = Vector3.zero; // center of the table
    public Vector3 rotationCenter = new(0, 0, -0.36f);

    public enum CameraViewMode { Default, TopDown }
    public CameraViewMode cameraViewMode = CameraViewMode.Default;
    public float topDownDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = new Vector3(0f, 1.75f, -1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
        if (cameraViewMode == CameraViewMode.TopDown)
        {
            cameraTransform.localRotation = Quaternion.Euler(45f, 0f, 0f);
        }
    }

    void ResetCamera() // BROKEN, resets to wrong position
    {
        if(Input.GetKey(KeyCode.O))
        {
            newPosition = Vector3.zero;
            newRotation = Quaternion.identity;
            newZoom = Vector3.zero;
        }
    }

    void ClampZoom()
    {
        newZoom.y = Mathf.Clamp(newZoom.y, 0.75f, 4.75f);
        newZoom.z = Mathf.Clamp(newZoom.z, -4.25f, -0.25f);
    }

    void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += 10 * Input.mouseScrollDelta.y * zoomAmount;
            ClampZoom();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Plane plane = new(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        if (Input.GetMouseButtonDown(2)) // When the user presses the button
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2)) // Middle mouse drag
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;
/* 
            if (cameraViewMode == CameraViewMode.TopDown)
            {
                // Rotate the RIG around Y-axis
                float angle = -difference.x / 5f;

                // Calculate rig's local up axis relative to current rotation
                Vector3 localUp = Quaternion.Inverse(newRotation) * Vector3.up;

                // Rotate rig around that axis in local space
                newRotation *= Quaternion.AngleAxis(angle, localUp);
            } */
            if (cameraViewMode == CameraViewMode.TopDown)
            {
                float angle = -difference.x / 5f;

                Vector3 spinAxis = newRotation * Vector3.forward; // Spin around rotated local-Z
                newRotation *= Quaternion.AngleAxis(angle, spinAxis);
            }
            else
            {
                newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
        }
    }

    void ToggleCameraTopView()
    {
        if (cameraViewMode == CameraViewMode.Default)
        {
            cameraViewMode = CameraViewMode.TopDown;
            Debug.Log("Camera is in " + cameraViewMode.ToString());
            newPosition = new Vector3(0, 0, 0f);  // Top-down position
            newRotation = Quaternion.Euler(45f, 0f, 0f); // Looking straight down
            newZoom = new Vector3(0, 2.25f, -1.75f); // Adjust based on your setup
        }
        else
        {
            cameraViewMode = CameraViewMode.Default;
            Debug.Log("Camera is in " + cameraViewMode.ToString());
            newPosition = new Vector3(0, 0, 0); // Normal
            newRotation = Quaternion.Euler(0f, 0f, 0f);
            newZoom = new Vector3(0f, 1.75f, -1.25f);
        }
    }

    void HandleMovementInput()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += transform.forward * movementSpeed;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += transform.forward * -movementSpeed;
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right * movementSpeed;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += transform.right * -movementSpeed;
        }

        if(Input.GetKey(KeyCode.Q))
        {
            newRotation  *= Quaternion.Euler(movementSpeed * rotationAmount * Vector3.up);
        }
        if(Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(movementSpeed * -rotationAmount * Vector3.up);
        }

        if(Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount * movementSpeed;
        }
        if(Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount * movementSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.O))
        {
            ToggleCameraTopView();
        }
        ClampZoom();

        // Interpolate (find points some fraction along the line between the two positions for smoother movement)
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime), 
        Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime));

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
