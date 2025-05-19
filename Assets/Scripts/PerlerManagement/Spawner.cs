using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public Transform beadParent; // Assigned in the Inspector

    private Vector3 objectPosition;

    public GameObject perlerBeadPrefab; // Original prefab
    private GameObject currentBead; // "Ghost" bead

    public List<GameObject> placedBeads = new List<GameObject>();
    public bool eraserMode = false;

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
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteLastBead();
        }

        if (gridOn)
        {
            currentBead.transform.position = new Vector3(
                RoundToNearestGrid(objectPosition.x) - gridSize / 2,
                0.50637f,
                RoundToNearestGrid(objectPosition.z) - gridSize / 2
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

        if (Input.GetMouseButtonDown(0))
        {
            if (eraserMode)
            {
                TryEraseBead();
            }
            else
            {
                CreatePerlerBead();
            }
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
        if (!canPlaceBead || eraserMode) return;
        Debug.Log("Placing a perler!");

        Vector3 spawnPosition = currentBead.transform.position;
        spawnPosition.y = 0.50637f; // Maintain consistent Y height

        GameObject newBead = Instantiate(perlerBeadPrefab, spawnPosition, transform.rotation, beadParent);
        ChangePerlerColor(newBead);
        newBead.tag = "PerlerTag";

        placedBeads.Add(newBead);
    }
    
    void DeleteLastBead()
    {
        if (placedBeads.Count > 0)
        {
            GameObject lastBead = placedBeads[placedBeads.Count - 1];
            placedBeads.RemoveAt(placedBeads.Count - 1);
            Destroy(lastBead);
        }
    }

    void TryEraseBead()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.CompareTag("PerlerTag"))
            {
                GameObject bead = hitInfo.collider.gameObject;
                placedBeads.Remove(bead);
                Destroy(bead);
                Debug.Log("Bead erased!");
            }
        }
    }

    public void ToggleEraserMode()
    {
        eraserMode = !eraserMode;
        currentBead.SetActive(!eraserMode); // Hide ghost bead when erasing
    }

    public void ToggleGrid()
    {
        if (gridToggle.isOn)
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
