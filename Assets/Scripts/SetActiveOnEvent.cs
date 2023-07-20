using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnEvent : MonoBehaviour
{
    public GameObject target;
    public GameObject inverseTarget;
    public bool active;

    public void Toggle()
    {
        active = !target.activeSelf;
        Set();
    }

    public void ToggleTargetOnly(bool setInverseToFalse)
    {
        if (target != null) target.SetActive(!target.activeSelf);
        if (inverseTarget != null && setInverseToFalse) inverseTarget.SetActive(false);
    }
    public void Activate()
    {
        active = true;
        Set();
    }

    public void Deactivate()
    {
        active = false;
        Set();
    }
    void Set()
    {
        if (target != null) target.SetActive(active);
        if (inverseTarget != null) inverseTarget.SetActive(!active);
    }
}
