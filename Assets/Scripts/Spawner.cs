using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem; // Import the new Input System

public class Spawner : MonoBehaviour
{
    private Vector3 objectPosition;

    public GameObject perlerBeadPrefab; // Original prefab
    private GameObject currentBead; // "Ghost" bead

    private RaycastHit raycastHit;
    [SerializeField] private LayerMask layerMask;
    bool canPlaceBead = false;

    public float gridSize;
    bool gridOn = true;
    [SerializeField] private Toggle gridToggle;

    void Start()
    {
        currentBead = Instantiate(perlerBeadPrefab);
        ChangePerlerColor(currentBead);
    }

    void Update()
    {
        if(gridOn)
        {
            currentBead.transform.position = new Vector3(
                RoundToNearestGrid(objectPosition.x) - gridSize/2,
                0.50637f,
                RoundToNearestGrid(objectPosition.z) - gridSize/2
            );
        }
        else
        {
            currentBead.transform.position = new Vector3(
                objectPosition.x,
                0.50637f,
                objectPosition.z
            );
        }

        if(Input.GetMouseButtonDown(0))
        {
            CreatePerlerBead();
        }
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Cast a ray from mouse position

        if(Physics.Raycast(ray, out raycastHit, 2000, layerMask))
        {
            objectPosition = raycastHit.point;
            canPlaceBead = true;
            Debug.DrawRay(ray.origin, ray.direction * 2000, Color.green);
        }
        else
        {
            canPlaceBead = false;
            Debug.DrawRay(ray.origin, ray.direction * 2000, Color.red);
        }
    }

    public void ChangePerlerColor(GameObject perler)
    {
        Debug.Log("Changing perler color!");
        Renderer renderer = perler.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = perler.GetComponentInChildren<Renderer>();
        }

        if (renderer != null)
        {
            renderer.material = new Material(renderer.material); // copies material to make sure we don't change all clone colors
            renderer.material.color = PerlerColorChanger.SelectedColor;
        }
    }
    
    public void UpdateGhostBeadColor()
    {
        if (currentBead != null)
        {
            ChangePerlerColor(currentBead);
        }
    }

    public void CreatePerlerBead()
    {
        if (!canPlaceBead) return;
        Debug.Log("Placing a perler!");

        Vector3 spawnPosition = currentBead.transform.position;
        spawnPosition.y = 0.50637f; // Maintain consistent Y height

        GameObject newBead = Instantiate(perlerBeadPrefab, spawnPosition, transform.rotation);
        ChangePerlerColor(newBead);
        newBead.tag = "Perler"; 
    }

    public void ToggleGrid()
    {
        if(gridToggle.isOn)
        {
            gridOn = true;
        }
        else
        {
            gridOn = false;
        }
    }

    float RoundToNearestGrid(float position)
    {
        float xDiff = position % gridSize;
        position -= xDiff;
        if(xDiff > (gridSize / 2))
        {
            position += gridSize;
        }
        return position;
    }
}
