using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleTriggerIfOn : MonoBehaviour
{
    public Toggle toggle;
    public UnityEvent TriggerEvent;
    public void OnChanged()
    {
       if (toggle.isOn)
        {
            TriggerEvent.Invoke();
        }
    }
}
