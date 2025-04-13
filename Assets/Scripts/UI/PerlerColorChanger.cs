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
        GameObject canvasUI = new GameObject("UICanvas", typeof(Canvas));
        Canvas canvas = canvasUI.GetComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // HUD Placement
        canvasUI.AddComponent<CanvasScaler>(); // resolution scaling
        canvasUI.AddComponent<GraphicRaycaster>(); // allows interaction

        string[] names = { "red", "green", "blue" };
        Slider[] sliders = new Slider[3];

        for (int i = 0; i < 3; i++)
        {
            GameObject newSlider = new GameObject(names[i] + "Slider", typeof(Slider), typeof(RectTransform));
            newSlider.transform.SetParent(canvasUI.transform, false);

            RectTransform rt = newSlider.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(200, 20);
            rt.anchoredPosition = new Vector2(0, 40 - i * 40); // Stack vertically

            Slider slider = newSlider.GetComponent<Slider>();
            slider.maxValue = 255;
            slider.wholeNumbers = true;
            slider.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });

            sliders[i] = slider;
        }


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
    }
}
