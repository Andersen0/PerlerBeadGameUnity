using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class PerlerColorChanger : MonoBehaviour
{
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    public GameObject perlerModel; // Assign the lowVertixPerler GameObject in the inspector
    private Renderer modelRenderer;

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

        modelRenderer = perlerModel.GetComponent<Renderer>();
        if (modelRenderer == null)
        {
            modelRenderer = perlerModel.GetComponentInChildren<Renderer>();
        }

        // Subscribe to slider value changes
        redSlider.onValueChanged.AddListener(delegate { UpdateModelColor(); });
        greenSlider.onValueChanged.AddListener(delegate { UpdateModelColor(); });
        blueSlider.onValueChanged.AddListener(delegate { UpdateModelColor(); });

        UpdateModelColor(); // Set initial color
    }

    void UpdateModelColor()
    {
        if (modelRenderer != null)
        {
            float r = redSlider.value / 255f;
            float g = greenSlider.value / 255f;
            float b = blueSlider.value / 255f;

            Color newColor = new Color(r, g, b);
            modelRenderer.material.color = newColor;
        }
    }
}
