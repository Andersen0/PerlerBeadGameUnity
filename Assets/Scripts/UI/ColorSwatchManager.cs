using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSwatchManager : MonoBehaviour
{
    public List<Button> colorButtons;
    public List<Image> buttonImages;
    public List<Color> savedColors;

    public Image ColorPreview;
    public Slider RedSlider, GreenSlider, BlueSlider;

    private bool initialized = false;
    private const float doubleClickThreshold = 0.3f;


    private void Start()
    {
        StartCoroutine(WaitForDependencies());
    }

    IEnumerator WaitForDependencies()
    {
        while (ColorPreview == null || RedSlider == null || GreenSlider == null || BlueSlider == null)
        {
            yield return null;
        }

        InitializeSwatches();
    }


    private void InitializeSwatches()
    {
        
        UpdateSwatchVisuals();
        Debug.Log("Color Manager Initialized");

        initialized = true;
    }


    public void UpdateSwatchVisuals()
    {
        for (int i = 0; i < savedColors.Count; i++)
        {
            if (i < buttonImages.Count)
            {
                buttonImages[i].color = savedColors[i];
            }
        }
    }

    public void OnSingleClick(int index)
    {
        if (!initialized || index >= savedColors.Count) return;
        ApplySavedColor(index);
    }


    public void OnDoubleClick(int index)
    {
        if (!initialized || index >= savedColors.Count) return;
        SaveColorToSlot(index);
    }


    public void ApplySavedColor(int index)
    {
        if (!initialized || index >= savedColors.Count) return;

        Color color = savedColors[index];
        ColorPreview.color = color;
        RedSlider.value = Mathf.RoundToInt(color.r * 255f);
        GreenSlider.value = Mathf.RoundToInt(color.g * 255f);
        BlueSlider.value = Mathf.RoundToInt(color.b * 255f);
    }


    public void SaveColorToSlot(int index)
    {
        if (!initialized || index >= savedColors.Count) return;

        Color currentColor = ColorPreview.color;
        savedColors[index] = currentColor;
        UpdateSwatchVisuals();
    }
}

