using UnityEngine;
using UnityEngine.InputSystem; // Import the new Input System

public class Spawner : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector3 objectPos;

    public GameObject myPerlerBead;

    public void SpawnPerlerBead(Vector3 position)
    {
        GameObject newBead = Instantiate(myPerlerBead, position, Quaternion.identity); // Rotate 90 degrees on X-axis


        Renderer renderer = newBead.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = newBead.GetComponentInChildren<Renderer>();
        }

        if (renderer != null)
        {
            renderer.material = new Material(renderer.material); // copies material to make sure we don't change all clone colors
            renderer.material.color = PerlerColorChanger.SelectedColor;
        }

    }


    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) // New Input System check
        {
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = 2.0f;
            objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            SpawnPerlerBead(objectPos);
        }
    }
}
