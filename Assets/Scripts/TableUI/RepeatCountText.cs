using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepeatCountText : MonoBehaviour
{
    public TMP_Text text;
    string initText;
    Rule rule;
    // Start is called before the first frame update
    void Start()
    {
        rule = this.GetComponentInParent<Rule>();
        initText = text.text;
    }

    private void OnGUI()
    {
        text.text = string.Format(initText, rule.repeatCount.ToString());
    }
}
