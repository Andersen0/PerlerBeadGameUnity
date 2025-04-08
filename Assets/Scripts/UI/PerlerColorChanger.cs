using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class PerlerColorChanger : MonoBehaviour
{
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    public static Color SelectedColor = Color.white;

    void Start()
    {

        redSlider = GameObject.Find("redSlider").GetComponent<Slider>();
        greenSlider = GameObject.Find("greenSlider").GetComponent<Slider>();
        blueSlider = GameObject.Find("blueSlider").GetComponent<Slider>();

        // Changes the max value of the slider to 20;
        redSlider.maxValue = 255;
        greenSlider.maxValue = 255;
        blueSlider.maxValue = 255;

        blueSlider.wholeNumbers = true;
        greenSlider.wholeNumbers = true;
        blueSlider.wholeNumbers = true;

        redSlider.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });
        greenSlider.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });
        blueSlider.onValueChanged.AddListener(delegate { UpdateSelectedColor(); });

        UpdateSelectedColor(); // Set initial color
    }

    void UpdateSelectedColor()
    {
        float r = redSlider.value / 255f;
        float g = greenSlider.value / 255f;
        float b = blueSlider.value / 255f;

        SelectedColor = new Color(r, g, b);
    }
}
