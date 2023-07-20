using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorTextOnToggle : MonoBehaviour
{
    public TMP_Text text;
    public Color onColor;
    public Color offColor;
    Toggle toggle;
    bool on;
    public UnityEvent OnToggleOn;

    private void Start()
    {
        toggle = this.GetComponent<Toggle>();
    }
    public void UpdateColor()
    {
        //toggle = this.GetComponent<Toggle>();
        if (toggle != null)
        {
            text.color = (toggle.isOn) ? onColor : offColor;
            if (toggle.isOn)
            {
                OnToggleOn.Invoke();
            }
            on = toggle.isOn;
        }
    }

    private void OnGUI()
    {
        if (toggle != null && toggle.isOn != on)
        {
            UpdateColor();
        }
    }
}
