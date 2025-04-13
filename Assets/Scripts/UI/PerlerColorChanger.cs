using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class PerlerColorChanger : MonoBehaviour
{
    public GameObject sliderPrefab;

    public Slider redSliderInfo;
    public Slider greenSliderInfo;
    public Slider blueSliderInfo;

    public Text redValueText;
    public Text greenValueText;
    public Text blueValueText;

    public Image UICircleSprite; // Color preview UI element
    public static Color SelectedColor = Color.white;

    public Font defaultFont;

    void Start()
    {
        Debug.Log("PerlerColorChanger script is running!");
        sliderPrefab = Resources.Load<GameObject>("slider");
        UICircleSprite = Resources.Load<Image>("UISprite");

        if (sliderPrefab == null)
        {
            Debug.LogError("Slider prefab not found in Resources folder!");
            return;
        }

        // UI Canvas
        GameObject canvasUI = new GameObject("UICanvas", typeof(Canvas));
        Canvas canvas = canvasUI.GetComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // HUD Placement
        canvasUI.AddComponent<CanvasScaler>(); // resolution scaling
        canvasUI.AddComponent<GraphicRaycaster>(); // allows interaction

        string[] names = { "red", "green", "blue" }; 
        Slider[] sliders = new Slider[3]; // to avoid duplicate code

        if (defaultFont == null)
        {
            defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // TODO: replace with better font
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject newSlider = Instantiate(sliderPrefab, canvasUI.transform);
            newSlider.name = names[i] + "Slider";
            newSlider.transform.SetParent(canvasUI.transform, false); // might be redundant

            RectTransform rt = newSlider.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(200, 20);
            rt.anchoredPosition = new Vector2(-600, 300 - (i*30) ); // Stack vertically

            Slider slider = newSlider.GetComponent<Slider>();
            slider.maxValue = 255;
            slider.wholeNumbers = true;
            slider.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });

            sliders[i] = slider;


            // Create a Text element for each slider value display
            GameObject valueTextObject = new GameObject(names[i] + "ValueText", typeof(Text));
            valueTextObject.transform.SetParent(canvasUI.transform, false);
            Text valueText = valueTextObject.GetComponent<Text>();
            valueText.font = defaultFont;
            valueText.text = "0"; // Initial value
            valueText.fontSize = 20;
            valueText.alignment = TextAnchor.MiddleLeft;
            RectTransform textRect = valueTextObject.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(100, 100);
            textRect.anchoredPosition = new Vector2(-440, 300 - (i*30)); // Align next to sliders


            // Assign text references
            if (i == 0) redValueText = valueText;
            if (i == 1) greenValueText = valueText;
            if (i == 2) blueValueText = valueText;
        }

        // Instantiate the color preview from the prefab
        GameObject colorPreviewObject = Instantiate(UICircleSprite.gameObject, canvasUI.transform);
        UICircleSprite = colorPreviewObject.GetComponent<Image>();

        UICircleSprite.rectTransform.sizeDelta = new Vector2(50, 50);
        UICircleSprite.rectTransform.anchoredPosition = new Vector2(-420, 270);

        // Assign to public variables
        redSliderInfo = sliders[0];
        greenSliderInfo = sliders[1];
        blueSliderInfo = sliders[2];

        UpdateSelectedColor(); // Set initial color
    }


    void UpdateSelectedColor()
    {
        float r = redSliderInfo.value / 255f;
        float g = greenSliderInfo.value / 255f;
        float b = blueSliderInfo.value / 255f;

        SelectedColor = new Color(r, g, b);

        // Update color preview
        UICircleSprite.color = SelectedColor;

        // Update the slider value labels
        redValueText.text = Mathf.RoundToInt(redSliderInfo.value).ToString();
        greenValueText.text = Mathf.RoundToInt(greenSliderInfo.value).ToString();
        blueValueText.text = Mathf.RoundToInt(blueSliderInfo.value).ToString();
    }
}
