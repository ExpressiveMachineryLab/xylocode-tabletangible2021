using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InspectorData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string itemName;
    [TextArea(3, 10)]
    public string definition;
    [TextArea(3, 10)]
    public string interaction;
    [TextArea(3, 10)]
    public string code;
    public string colorReplace;

    public bool isUI = true;
    public UnityEvent OnHover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isUI) OnEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isUI) OnExit();
    }

    void OnMouseEnter()
    {
        if (!isUI) OnEnter();
    }

    void OnMouseExit()
    {
        if (!isUI) OnExit();
    }
    public void OnEnter()
    {
        OnHover.Invoke();
        InspectorPanel.inspector.SetInspectorData(this);
    }

    public void OnExit()
    {
        InspectorPanel.inspector.UnsetInspectorData(this);
    }
    public void ForceUpdate()
    {
        InspectorPanel.inspector.ForceUpdate();
    }
}