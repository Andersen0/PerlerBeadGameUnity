using UnityEngine;
using UnityEngine.InputSystem; // Import the new Input System

public class Spawner : MonoBehaviour
{
    private Vector3 objectPos;

    public GameObject myPerlerBead;

    private RaycastHit raycastHit;

    public void SpawnPerlerBead(Vector3 position)
    {
        position.y = 0.506375f;
        GameObject newBead = Instantiate(myPerlerBead, position, Quaternion.identity); // Rotate 90 degrees on X-axis


        Renderer renderer = newBead.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = newBead.GetComponentInChildren<Renderer>();
        }

        else
        {
            renderer.material = new Material(renderer.material); // copies material to make sure we don't change all clone colors
            renderer.material.color = PerlerColorChanger.SelectedColor;
        }

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Cast a ray from mouse position

            if(Physics.Raycast(ray, out raycastHit, 2000))
            {
                Debug.Log("Placing a perler bead");
                objectPos = raycastHit.point; 
                SpawnPerlerBead(objectPos);
            }
        }
    }
}
