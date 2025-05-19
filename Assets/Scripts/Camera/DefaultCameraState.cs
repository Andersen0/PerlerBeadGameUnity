using UnityEngine;

public class DefaultCameraState : ICameraState
{
    public void Enter(CameraController controller)
    {
        controller.newZoom = new Vector3(0, 1.75f, -1.25f); // Top-down height
        controller.cameraTransform.localRotation = Quaternion.Euler(45f, 0f, 0f); // Look straight down
    }

    public void Exit(CameraController controller) { }

    public void Update(CameraController controller)
    {
        float lerpSpeed = controller.isTransitioning ? controller.transitionLerpSpeed : controller.inputLerpSpeed;

        controller.transform.position = Vector3.Lerp(
            controller.transform.position,
            controller.newPosition,
            Time.deltaTime * lerpSpeed
        );

        controller.transform.rotation = Quaternion.Lerp(
            controller.transform.rotation,
            controller.newRotation,
            Time.deltaTime * lerpSpeed
        );

        controller.cameraTransform.localPosition = Vector3.Lerp(
            controller.cameraTransform.localPosition,
            controller.newZoom,
            Time.deltaTime * lerpSpeed
        );
    }

    public void HandleInput(CameraController controller)
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? controller.fastSpeed : controller.normalSpeed;

        // Keyboard movement
        if (Input.GetKey(KeyCode.W)) controller.newPosition += controller.transform.forward * speed;
        if (Input.GetKey(KeyCode.S)) controller.newPosition -= controller.transform.forward * speed;
        if (Input.GetKey(KeyCode.D)) controller.newPosition += controller.transform.right * speed;
        if (Input.GetKey(KeyCode.A)) controller.newPosition -= controller.transform.right * speed;

        // Keyboard rotation
        if (Input.GetKey(KeyCode.Q)) controller.newRotation *= Quaternion.Euler(0f, -controller.rotationAmount * speed, 0f);
        if (Input.GetKey(KeyCode.E)) controller.newRotation *= Quaternion.Euler(0f, controller.rotationAmount * speed, 0f);

        // Zoom keys
        if (Input.GetKey(KeyCode.R)) controller.newZoom += controller.zoomAmount * speed;
        if (Input.GetKey(KeyCode.F)) controller.newZoom -= controller.zoomAmount * speed;

        // Zoom with mouse scroll
        if (Input.mouseScrollDelta.y != 0)
        {
            controller.newZoom += controller.zoomAmount * 10f * Input.mouseScrollDelta.y;
        }

        controller.ClampZoom();

        // Dragging camera with right mouse (move)
        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float entry))
                controller.dragStartPosition = ray.GetPoint(entry);
        }

        if (Input.GetMouseButton(1))
        {
            Plane plane = new(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float entry))
            {
                Vector3 currentDragPosition = ray.GetPoint(entry);
                Vector3 delta = controller.dragStartPosition - currentDragPosition;
                controller.newPosition += delta;
                controller.dragStartPosition = currentDragPosition; // update for next frame
            }
        }

        // Middle mouse drag (rotate)
        if (Input.GetMouseButtonDown(2))
        {
            controller.rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            controller.rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = controller.rotateStartPosition - controller.rotateCurrentPosition;
            controller.rotateStartPosition = controller.rotateCurrentPosition;

            float rotationSpeed = difference.x / 5f;
            controller.newRotation *= Quaternion.Euler(0f, -rotationSpeed, 0f);
        }
    }
}
