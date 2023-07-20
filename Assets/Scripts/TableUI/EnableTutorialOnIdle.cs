using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTutorialOnIdle : MonoBehaviour
{
    public Tutorial tutorial;
    public float checkDelay = 5f;
    float clock = 0f;
    bool enabledDueToIdle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorial.IsActive())
        {
            enabledDueToIdle = false;
        }
        if (clock > 0f)
        {
            clock -= Time.deltaTime;
        }
        else
        {
            clock = checkDelay;
            if (!GameManager.IsAnyElementActive())
            {
                tutorial.EnableTutorial();
                enabledDueToIdle = true;
            }
            else if (enabledDueToIdle)
            {
                tutorial.DisableTutorial();
            }
        }
    }
}
