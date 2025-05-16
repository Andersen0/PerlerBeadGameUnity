using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SwatchButtonHandler : MonoBehaviour, IPointerClickHandler
{
    public int index;
    public ColorSwatchManager manager;

    private float lastClickTime = 0f;
    private const float doubleClickThreshold = 0.15f;
    private Coroutine singleClickCoroutine;

    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTime = Time.time;

        if (currentTime - lastClickTime <= doubleClickThreshold)
        {
            // Double click detected
            if (singleClickCoroutine != null)
            {
                StopCoroutine(singleClickCoroutine); // Cancel pending single click
                singleClickCoroutine = null;
            }

            manager.OnDoubleClick(index);
        }
        else
        {
            // Schedule single click with a delay
            singleClickCoroutine = StartCoroutine(DelayedSingleClick());
        }

        lastClickTime = currentTime;
    }

    private IEnumerator DelayedSingleClick()
    {
        yield return new WaitForSeconds(doubleClickThreshold);
        manager.OnSingleClick(index);
        singleClickCoroutine = null;
    }
}
