using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateColorBasedOnEmitter : MonoBehaviour
{
    public EmitterTangible emitter;
    public Image image;

    private void OnGUI()
    {
        image.color = TangibleGameController.GetColor(emitter.color);
    }
}
