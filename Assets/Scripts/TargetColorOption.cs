using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetColorOption : MonoBehaviour
{
    public TangibleGameController.TargetColors MyColor;

    public TangibleGameController.TargetColors GetColor()
    {
        return MyColor;
    }
}
