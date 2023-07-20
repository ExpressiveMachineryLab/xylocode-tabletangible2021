using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTogglesBasedOnColor : MonoBehaviour
{
    public Line line;
    [Space(20)]
    public Toggle red;
    public Toggle blue;
    public Toggle yellow;
    public Toggle green;
    bool checkOnUpdate;
    [Space(10)]
    public float updateInterval = 0.25f;
    float clock;
    private void OnEnable()
    {
        checkOnUpdate = true;
    }

    private void Start()
    {
        checkOnUpdate = true;
    }

    private void Update()
    {
        if (clock > 0f)
        {
            clock -= Time.deltaTime;
        }
        else
        {
            clock = updateInterval;
            checkOnUpdate = true;
        }
        if (checkOnUpdate)
        {
            UpdateColors();
            checkOnUpdate = false;
        }
        
    }

    public void UpdateColors()
    {
        red.SetIsOnWithoutNotify(line.color == ElemColor.red);
        blue.SetIsOnWithoutNotify(line.color == ElemColor.blue);
        yellow.SetIsOnWithoutNotify(line.color == ElemColor.yellow);
        green.SetIsOnWithoutNotify(line.color == ElemColor.green);
    }
}
