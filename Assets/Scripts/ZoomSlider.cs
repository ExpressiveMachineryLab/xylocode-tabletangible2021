using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZoomSlider : MonoBehaviour
{
    public DragCam drag;
    public Slider slider;
    public TMP_Text percentageText;

    public void UpdateZoom()
    {
        drag.Zoom(slider.value);
    }

    public void OnGUI()
    {
        if (drag.scrollThisFrame)
        {
            slider.value = drag.zoomPercent;
        }
        percentageText.text = Mathf.RoundToInt(drag.zoomPercent * 100f) + "%";
    }
}
