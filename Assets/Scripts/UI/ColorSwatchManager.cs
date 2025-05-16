using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorSwatchManager : MonoBehaviour
{
    public Button[] colorButtons; // Assign 6 buttons in the Inspector
    public Image[] buttonImages;  // These should match the colorButtons visually
    public Color[] savedColors = new Color[6]; // Persistent storage
    public Image colorPreview; // The main color preview UI
    public Slider redSlider, greenSlider, blueSlider;

    private float[] lastClickTimes = new float[6];
    private const float doubleClickThreshold = 0.3f;

    void Start()
    {
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i; // Avoid closure issues
            colorButtons[i].onClick.AddListener(() => OnSwatchClicked(index));
            buttonImages[i].color = savedColors[i]; // Initialize visuals
        }
    }

    void OnSwatchClicked(int index)
    {
        float time = Time.time;
        if (time - lastClickTimes[index] < doubleClickThreshold)
        {
            // Double click: save color
            savedColors[index] = colorPreview.color;
            buttonImages[index].color = savedColors[index];
        }
        else
        {
            // Single click: apply color
            Color c = savedColors[index];
            colorPreview.color = c;
            redSlider.value = Mathf.RoundToInt(c.r * 255f);
            greenSlider.value = Mathf.RoundToInt(c.g * 255f);
            blueSlider.value = Mathf.RoundToInt(c.b * 255f);
        }
        lastClickTimes[index] = time;
    }
}
