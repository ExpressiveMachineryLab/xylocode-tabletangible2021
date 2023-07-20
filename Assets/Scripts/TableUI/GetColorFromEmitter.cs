using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetColorFromEmitter : MonoBehaviour
{
    public EmitterTangible emitter;
    public Color red;
    public Color blue;
    public Color yellow;
    public Color green;
    public bool editTransition;
    ColorBlock colors;
    // Update is called once per frame

    private void Start()
    {
        if (editTransition)
        {
            colors = this.GetComponent<Selectable>().colors;
        }
    }
    void OnGUI()
    {
        Color c;
        switch (emitter.color)
        {
            default:
            case ElemColor.red:
                c = red;
                break;
            case ElemColor.yellow:
                c = yellow;
                break;
            case ElemColor.blue:
                c = blue;
                break;
            case ElemColor.green:
                c = green;
                break;
        }
        if (!editTransition)
        {
            this.GetComponent<Image>().color = c;
        }
        else
        {
            colors.pressedColor = c;
            this.GetComponent<Selectable>().colors = colors;
        }
        
    }
}
