using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float inputLerpSpeed = 10f;
    public float transitionLerpSpeed = 4f;
    public float rotationAmount;
    public Vector3 zoomAmount = new(0, 0.5f, 0.5f);

    [HideInInspector] public Vector3 newPosition;
    [HideInInspector] public Quaternion newRotation;
    [HideInInspector] public Vector3 newZoom;

    [HideInInspector] public Vector3 dragStartPosition;
    [HideInInspector] public Vector3 dragCurrentPosition;

    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    private ICameraState currentState;
    private ICameraState defaultState = new DefaultCameraState();
    private ICameraState topDownState = new TopDownCameraState();
    [HideInInspector] public bool isTransitioning = false;

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

        SetCameraState(defaultState);
    }

    private void Update()
    {
        currentState.HandleInput(this);
        currentState.Update(this);

        if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.O))
        {
            if (currentState == defaultState)
                SetCameraState(topDownState);
            else
                SetCameraState(defaultState);
        }
    }

    public void ClampZoom()
    {
        newZoom.y = Mathf.Clamp(newZoom.y, 0.75f, 4.75f);
        newZoom.z = Mathf.Clamp(newZoom.z, -4.25f, -0.25f);
    }

    public void SetCameraState(ICameraState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        isTransitioning = true; // Start transition
        currentState.Enter(this);

        // Stop transition after a delay
        StartCoroutine(EndTransitionAfterDelay(1f));
    }

    private IEnumerator EndTransitionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTransitioning = false;
    }
}
