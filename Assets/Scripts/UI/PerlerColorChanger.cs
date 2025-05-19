using UnityEngine;
using UnityEngine.UI;

public class PerlerColorChanger : MonoBehaviour
{
    public GameObject sliderPrefab;
    public Image colorPreviewPrefab;
    public Font defaultFont;

    private Slider redSlider, greenSlider, blueSlider;
    private Text redValueText, greenValueText, blueValueText;
    private Image colorPreview;

    public static Color SelectedColor = Color.white;

    private Spawner spawner;

    void Start()
    {
        Debug.Log("PerlerColorChanger script is running!");

        spawner = FindFirstObjectByType<Spawner>();

        // Load resources
        sliderPrefab = Resources.Load<GameObject>("slider");
        colorPreviewPrefab = Resources.Load<Image>("UISprite");
        defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        if (sliderPrefab == null || colorPreviewPrefab == null)
        {
            Debug.LogError("Required prefab(s) not found in Resources folder!");
            return;
        }

        // Create UI Canvas
        GameObject canvasGO = new GameObject("UICanvas", typeof(Canvas));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Define vertical spacing between sliders
        float verticalSpacing = 30f;

        // Starting offset from the top-left corner
        Vector2 startOffset = new Vector2(10, -50);

        // Create RGB sliders and labels
        redSlider = CreateSliderWithLabel(canvasGO.transform, "Red", startOffset, out redValueText);
        greenSlider = CreateSliderWithLabel(canvasGO.transform, "Green", startOffset + new Vector2(0, -verticalSpacing), out greenValueText);
        blueSlider = CreateSliderWithLabel(canvasGO.transform, "Blue", startOffset + new Vector2(0, -2 * verticalSpacing), out blueValueText);

        // Create color preview positioned below the sliders
        Vector2 previewOffset = startOffset + new Vector2(0, -3 * verticalSpacing - 10); // Additional spacing
        colorPreview = CreateColorPreview(canvasGO.transform, previewOffset);
        
        
        // Add listeners to sliders
        redSlider.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });
        greenSlider.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });
        blueSlider.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });


        // Initialize color
        UpdateSelectedColor();
    }


    Slider CreateSliderWithLabel(Transform parent, string label, Vector2 offset, out Text valueText)
    {
        // Instantiate the slider prefab
        GameObject sliderGO = Instantiate(sliderPrefab, parent);
        sliderGO.name = $"{label}Slider";

        // Access the RectTransform component
        RectTransform rt = sliderGO.GetComponent<RectTransform>();

        // Set anchors to top-left
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);

        // Set pivot to top-left
        rt.pivot = new Vector2(0, 1);

        // Set the anchored position with the provided offset
        rt.anchoredPosition = offset;

        // Set the size of the slider
        rt.sizeDelta = new Vector2(200, 20);

        // Configure the Slider component
        Slider slider = sliderGO.GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 255;
        slider.value = 0;

        // Create a label for the slider
        GameObject labelGO = new GameObject($"{label}Label");
        labelGO.transform.SetParent(sliderGO.transform, false);
        Text labelText = labelGO.AddComponent<Text>();
        labelText.text = label;
        labelText.font = defaultFont;
        labelText.material = defaultFont.material;
        labelText.fontSize = 14;
        labelText.alignment = TextAnchor.MiddleLeft;

        // Position the label to the left of the slider
        RectTransform labelRT = labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = new Vector2(0, 0.5f);
        labelRT.anchorMax = new Vector2(0, 0.5f);
        labelRT.pivot = new Vector2(1, 0.5f);
        labelRT.anchoredPosition = new Vector2(-10, 0);
        labelRT.sizeDelta = new Vector2(50, 20);

        // Create a value text to the right of the slider
        GameObject valueGO = new GameObject($"{label}Value");
        valueGO.transform.SetParent(sliderGO.transform, false);
        valueText = valueGO.AddComponent<Text>();
        valueText.text = "0";
        valueText.font = defaultFont;
        valueText.material = defaultFont.material;
        valueText.fontSize = 14;
        valueText.alignment = TextAnchor.MiddleLeft;

        // Position the value text to the right of the slider
        RectTransform valueRT = valueGO.GetComponent<RectTransform>();
        valueRT.anchorMin = new Vector2(1, 0.5f);
        valueRT.anchorMax = new Vector2(1, 0.5f);
        valueRT.pivot = new Vector2(0, 0.5f);
        valueRT.anchoredPosition = new Vector2(10, 0);
        valueRT.sizeDelta = new Vector2(30, 20);

        return slider;
    }


    Image CreateColorPreview(Transform parent, Vector2 offset)
    {
        // Instantiate the color preview prefab
        Image preview = Instantiate(colorPreviewPrefab, parent);
        preview.name = "ColorPreview";

        // Access the RectTransform component
        RectTransform rt = preview.GetComponent<RectTransform>();

        // Set anchors to top-left
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);

        // Set pivot to top-left
        rt.pivot = new Vector2(0, 1);

        // Set the anchored position with the provided offset
        rt.anchoredPosition = offset;

        // Set the size of the color preview
        rt.sizeDelta = new Vector2(60, 60);

        return preview;
    }


    void UpdateSelectedColor()
    {
        float r = redSlider.value / 255f;
        float g = greenSlider.value / 255f;
        float b = blueSlider.value / 255f;
        SelectedColor = new Color(r, g, b);

        // Update UI elements
        colorPreview.color = SelectedColor;
        redValueText.text = Mathf.RoundToInt(redSlider.value).ToString();
        greenValueText.text = Mathf.RoundToInt(greenSlider.value).ToString();
        blueValueText.text = Mathf.RoundToInt(blueSlider.value).ToString();

        // Update ghost bead color
        if (spawner != null)
        {
            spawner.UpdateGhostBeadColor();
        }
    }
}
