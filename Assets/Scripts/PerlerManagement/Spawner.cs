using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour
{
    public Image pencilButtonImage;
    public Image eraserButtonImage;

    public Sprite pencilActiveSprite;
    public Sprite pencilGreyedOutSprite;

    public Sprite eraserActiveSprite;
    public Sprite eraserGreyedOutSprite;

    public Transform beadParent; // Assigned in the Inspector

    private Vector3 objectPosition;

    public GameObject perlerBeadPrefab; // Original prefab
    private GameObject currentBead; // "Ghost" bead
    public float ghostBeadAlpha = 0.5f;
    private GameObject highlightedBead = null;
    private Material[] originalMaterials = null;

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

        SetPencilMode();
    }


    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // Don't run spawner script when hovering UI

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteLastBead();
        }

        // Update ghost bead position (same as before)
        UpdateGhostBeadPosition();

        // Handle placing or erasing beads on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            if (eraserMode)
                TryEraseBead();
            else
                CreatePerlerBead();
        }

        // Highlight bead under cursor if eraser mode active
        if (eraserMode)
        {
            HandleEraserHighlight();
            currentBead.SetActive(false);  // Hide ghost bead in eraser mode
        }
        else
        {
            ClearHighlight();
            currentBead.SetActive(true);   // Show ghost bead in normal mode
        }
    }



    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Cast a ray from mouse position

        if (Physics.Raycast(ray, out raycastHit, 2000, layerMask))
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


    void LateUpdate()
    {
        currentBead.SetActive(!eraserMode && canPlaceBead);
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


    public void ChangeGhostPerlerColor()
    {
        if (currentBead != null)
        {
            MakeGhostPerlerTransparent(currentBead, ghostBeadAlpha);
        }
    }


    void MakeGhostPerlerTransparent(GameObject ghostBead, float alpha)
    {
        Renderer renderer = ghostBead.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = ghostBead.GetComponentInChildren<Renderer>();
        }

        if (renderer != null)
        {
            // Create a new instance of the material so we don't affect other objects
            Material mat = new Material(renderer.material);

            // Enable transparency mode on the shader
            mat.SetFloat("_Mode", 3); // For Standard shader: 3 = Transparent
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3100;

            // Set the color with reduced alpha
            Color color = PerlerColorChanger.SelectedColor;
            color.a = alpha;  // Set transparency level here
            mat.color = color;

            // Assign the transparent material
            renderer.material = mat;
        }
    }


    public void UpdateGhostBeadColor()
    {
        {
            ChangeGhostPerlerColor();
        }
    }


    void HandleEraserHighlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.CompareTag("PerlerTag"))
            {
                GameObject bead = hitInfo.collider.gameObject;
                if (highlightedBead != bead)
                {
                    ClearHighlight();
                    HighlightBead(bead, ghostBeadAlpha);
                }
                return;
            }
        }
        ClearHighlight();
    }


    void HighlightBead(GameObject bead, float alpha)
    {
        Renderer[] renderers = bead.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        // Save original materials
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }

        foreach (Renderer renderer in renderers)
        {
            Material transparentMat = new Material(renderer.material);
            transparentMat.SetFloat("_Mode", 3);
            transparentMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            transparentMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            transparentMat.SetInt("_ZWrite", 0);
            transparentMat.DisableKeyword("_ALPHATEST_ON");
            transparentMat.EnableKeyword("_ALPHABLEND_ON");
            transparentMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            transparentMat.renderQueue = 3100;

            Color color = transparentMat.color;
            color.a = alpha;
            transparentMat.color = color;

            renderer.material = transparentMat;
        }

        highlightedBead = bead;
    }


    void ClearHighlight()
    {
        if (highlightedBead != null)
        {
            Renderer[] renderers = highlightedBead.GetComponentsInChildren<Renderer>();
            if (renderers.Length == originalMaterials.Length)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material = originalMaterials[i];
                }
            }
            highlightedBead = null;
            originalMaterials = null;
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


    public void SetEraserMode()
    {
        eraserMode = true;
        currentBead.SetActive(false); // Hide ghost bead when erasing

        // Update button visuals
        pencilButtonImage.sprite = pencilGreyedOutSprite;
        eraserButtonImage.sprite = eraserActiveSprite;
    }

    public void SetPencilMode()
    {
        eraserMode = false;
        currentBead.SetActive(true);

        // Update button visuals
        pencilButtonImage.sprite = pencilActiveSprite;
        eraserButtonImage.sprite = eraserGreyedOutSprite;
    }

    void UpdateGhostBeadPosition()
    {
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
        if (xDiff > (gridSize / 2))
        {
            position += gridSize;
        }
        return position;
    }
    

}
