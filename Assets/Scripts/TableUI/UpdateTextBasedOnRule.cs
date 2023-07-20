using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTextBasedOnRule : MonoBehaviour
{
    public Rule rule;
    public EmitterTangible frog;
    TMP_Text text;
    string initText;

    static Dictionary<ElemColor, string> elemColorDict = new Dictionary<ElemColor, string>()
    {
        {ElemColor.red, "Red" },
        {ElemColor.blue, "Blue" },
        {ElemColor.green, "Purple" },
        {ElemColor.yellow, "Yellow" },
    };

    static Dictionary<TangibleGameController.TargetColors, string> targetColorDict = new Dictionary<TangibleGameController.TargetColors, string>()
    {
        {TangibleGameController.TargetColors.Red, "Red" },
        {TangibleGameController.TargetColors.Blue, "Blue" },
        {TangibleGameController.TargetColors.Green, "Purple" },
        {TangibleGameController.TargetColors.Yellow, "Yellow" }
    };
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        initText = text.text;
    }

    private void OnGUI()
    {
        if (elemColorDict.TryGetValue(frog.color, out string frogC) && targetColorDict.TryGetValue(rule.TargetColor, out string ruleC))
        {
            text.text = initText.Replace("{0}", frogC.ToLower()).Replace("{1}", ruleC).Replace("{2}", ruleC.ToLower());
        }
        
    }
}
