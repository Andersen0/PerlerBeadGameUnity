using UnityEngine;

public class TopDownCameraState : ICameraState
{
    public void Enter(CameraController controller)
    {
        controller.newPosition = Vector3.zero;
        controller.newZoom = new Vector3(0, 5f, 0); // Top-down height
        controller.cameraTransform.localRotation = Quaternion.Euler(90f, 0f, 0f); // Look straight down

        controller.newRotation = Quaternion.Euler(0f, 0f, 0f);
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

        // Drag with right mouse button
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

        // Zoom with mouse scroll
        if (Input.mouseScrollDelta.y != 0)
        {
            controller.newZoom += Vector3.up * 10f * controller.zoomAmount.y * Input.mouseScrollDelta.y;
            controller.newZoom.y = Mathf.Clamp(controller.newZoom.y, 1.75f, 5f);
            controller.newZoom.x = 0f;
            controller.newZoom.z = 0f;
        }

        // Smooth rotation with Q/E keys
        if (Input.GetKey(KeyCode.Q))
        {
            controller.newRotation *= Quaternion.Euler(0f, -speed * 250f, 0f);
        }
        if (Input.GetKey(KeyCode.E)) {
            controller.newRotation *= Quaternion.Euler(0f, speed * 250f, 0f);
        }
    }
}
