using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class PerlerColorChanger : MonoBehaviour
{
    public Slider redSliderInfo;
    public Slider greenSliderInfo;
    public Slider blueSliderInfo;

    public static Color SelectedColor = Color.white;

    void Start()
    {
        // UI Canvas
        GameObject canvasUI = new GameObject("Canvas", typeof(Canvas));
        Canvas canvas = canvasUI.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // HUD Placement

        canvasUI.AddComponent<CanvasScaler>(); // resolution scaling
        canvasUI.AddComponent<GraphicRaycaster>(); // allows interaction

        // Create the Slider GameObject
        GameObject redSlider = new GameObject("Slider", typeof(Slider), typeof(RectTransform));
        GameObject greenSlider = new GameObject("Slider", typeof(Slider), typeof(RectTransform));
        GameObject blueSlider = new GameObject("Slider", typeof(Slider), typeof(RectTransform));

        redSlider.transform.SetParent(canvasUI.transform, false);
        greenSlider.transform.SetParent(canvasUI.transform, false);
        blueSlider.transform.SetParent(canvasUI.transform, false);

        // Set slider position/size (example)
        RectTransform top = redSlider.GetComponent<RectTransform>();
        RectTransform middle = greenSlider.GetComponent<RectTransform>();
        RectTransform bottom = blueSlider.GetComponent<RectTransform>();

        top.sizeDelta = new Vector2(200, 20);
        top.anchoredPosition = Vector2.zero;

        middle.sizeDelta = new Vector2(180, 20);
        middle.anchoredPosition = Vector2.zero;

        bottom.sizeDelta = new Vector2(160, 20);
        bottom.anchoredPosition = Vector2.zero;


        redSliderInfo = GameObject.Find("redSlider").GetComponent<Slider>();
        greenSliderInfo = GameObject.Find("greenSlider").GetComponent<Slider>();
        blueSliderInfo = GameObject.Find("blueSlider").GetComponent<Slider>();

        // Changes the max value of the slider to 20;
        redSliderInfo.maxValue = 255;
        greenSliderInfo.maxValue = 255;
        blueSliderInfo.maxValue = 255;

        redSliderInfo.wholeNumbers = true;
        greenSliderInfo.wholeNumbers = true;
        blueSliderInfo.wholeNumbers = true;

        redSliderInfo.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });
        greenSliderInfo.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });
        blueSliderInfo.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });

        UpdateSelectedColor(); // Set initial color
    }

    void UpdateSelectedColor()
    {
        float r = redSliderInfo.value / 255f;
        float g = greenSliderInfo.value / 255f;
        float b = blueSliderInfo.value / 255f;

        SelectedColor = new Color(r, g, b);
    }
}
