using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchTracker : MonoBehaviour
{
    public int index = -1;
    public GameObject display;
    public Text text;
    bool show;
    public void SetIndex(int i)
    {
        index = i;
        text.text = index.ToString();
    }

    private void Update()
    {
        if (index > -1 && index < Input.touchCount)
        {
            Touch touch = Input.GetTouch(index);
            this.display.SetActive(true);
            this.transform.position = touch.position;
        }
        else
        {
            this.display.SetActive(false);
        }
    }
}
