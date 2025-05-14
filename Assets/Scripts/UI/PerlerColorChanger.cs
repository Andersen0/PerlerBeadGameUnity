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

    void Start()
    {
        Debug.Log("PerlerColorChanger script is running!");

        // Load resources
        sliderPrefab = Resources.Load<GameObject>("slider");
        colorPreviewPrefab = Resources.Load<Image>("UISprite");
        if (defaultFont == null)
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

        // Create RGB sliders and labels
        redSlider = CreateSliderWithLabel(canvasGO.transform, "Red", new Vector2(-630, 300), out redValueText);
        greenSlider = CreateSliderWithLabel(canvasGO.transform, "Green", new Vector2(-630, 270), out greenValueText);
        blueSlider = CreateSliderWithLabel(canvasGO.transform, "Blue", new Vector2(-630, 240), out blueValueText);

        // Create color preview
        colorPreview = CreateColorPreview(canvasGO.transform, new Vector2(-450, 270));

        // Initialize color
        UpdateSelectedColor();
    }

    Slider CreateSliderWithLabel(Transform parent, string colorName, Vector2 position, out Text valueText)
    {
        // Instantiate slider
        GameObject sliderGO = Instantiate(sliderPrefab, parent);
        sliderGO.name = $"{colorName}Slider";
        RectTransform sliderRT = sliderGO.GetComponent<RectTransform>();
        sliderRT.sizeDelta = new Vector2(200, 20);
        sliderRT.anchoredPosition = position;

        Slider slider = sliderGO.GetComponent<Slider>();
        slider.maxValue = 255;
        slider.wholeNumbers = true;
        slider.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });

        // Create value text
        GameObject textGO = new GameObject($"{colorName}ValueText", typeof(Text));
        textGO.transform.SetParent(parent, false);
        valueText = textGO.GetComponent<Text>();
        valueText.font = defaultFont;
        valueText.text = "0";
        valueText.fontSize = 20;
        valueText.alignment = TextAnchor.MiddleLeft;
        RectTransform textRT = textGO.GetComponent<RectTransform>();
        textRT.sizeDelta = new Vector2(100, 20);
        textRT.anchoredPosition = new Vector2(position.x + 160, position.y);

        return slider;
    }

    Image CreateColorPreview(Transform parent, Vector2 position)
    {
        GameObject previewGO = Instantiate(colorPreviewPrefab.gameObject, parent);
        previewGO.name = "ColorPreview";
        Image previewImage = previewGO.GetComponent<Image>();
        RectTransform previewRT = previewImage.rectTransform;
        previewRT.sizeDelta = new Vector2(50, 50);
        previewRT.anchoredPosition = position;
        return previewImage;
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
    }
}
