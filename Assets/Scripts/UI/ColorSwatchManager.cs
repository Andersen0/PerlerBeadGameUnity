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

    private void Start()
    {
        StartCoroutine(WaitForDependencies());
    }


    IEnumerator WaitForDependencies()
    {
        while (ColorPreview == null || RedSlider == null || GreenSlider == null || BlueSlider == null)
        {
            if (ColorPreview == null)
            {
                GameObject previewObj = GameObject.Find("ColorPreview");
                if (previewObj != null)
                    ColorPreview = previewObj.GetComponent<Image>();
            }

            if (RedSlider == null)
            {
                GameObject redObj = GameObject.Find("RedSlider");
                if (redObj != null)
                    RedSlider = redObj.GetComponent<Slider>();
            }

            if (GreenSlider == null)
            {
                GameObject greenObj = GameObject.Find("GreenSlider");
                if (greenObj != null)
                    GreenSlider = greenObj.GetComponent<Slider>();
            }

            if (BlueSlider == null)
            {
                GameObject blueObj = GameObject.Find("BlueSlider");
                if (blueObj != null)
                    BlueSlider = blueObj.GetComponent<Slider>();
            }

            yield return null; // wait a frame
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

