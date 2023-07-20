using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Rule : MonoBehaviour
{

    [Header("Rule Setting")]
    public TangibleGameController.RuleType RuleType;
    public TangibleGameController.TargetColors TargetColor;
    public bool Active = false;
    public bool Invert = false;
    
    [Header("Default Colors")]
    public Color BgColor;
    public Color IcColor;
    
    [Header("Components")]
    public List<Toggle> ColorToggles;
    public Image ToggleBackground;
    public Image ToggleIcon;
    public GameObject RuleBlockVertical;
    public GameObject RuleBlockHorizontal;
        
    [Header("Invertible Component")]
    public RectTransform HorBackground;
    public RectTransform DeleteButton;
    public RectTransform QuestionButton;
    public RectTransform TutorialBlock;
    public RectTransform CollapseButton;
    public RectTransform Content;
    public RectTransform Label;
    public RectTransform tutorialParent;
    public Tutorial tutorial1;
    public Tutorial tutorial2;

    [Header("Rule Settings")]
    public int repeatCount;
    int repeatMin = 1;
    int repeatMax = 4;

    [HideInInspector]
    public TangibleController sourceTangible;

    void Start()
    {
        RuleBlockVertical.SetActive(false);
        RuleBlockHorizontal.SetActive(true);
        if (RuleType == TangibleGameController.RuleType.Repeat)
        {
            repeatCount = repeatMin;
        }
    }
    


    void Update()
    {
        if (ColorToggles.Count == 0)
        {
            var colorOptions = GetComponentsInChildren<TargetColorOption>();
            foreach (var co in colorOptions)
            {
                var t = co.gameObject.GetComponent<Toggle>();
                var img = t.GetComponentInChildren<Image>();
                img.color = TangibleGameController.Singleton.ColorBinding[co.MyColor];
                ColorToggles.Add(t);
                if (co.GetColor() == TargetColor)
                {
                    t.isOn = true;
                }
                else
                {
                    t.isOn = false;
                }
            }
        }
        
        if (!RuleBlockVertical.activeInHierarchy && !RuleBlockHorizontal.activeInHierarchy)
        {
            return;
        }
        ToggleIcon.color = Color.white;
        foreach (Toggle t in ColorToggles)
        {
            if (t.isOn)
            {
                TargetColor = t.gameObject.GetComponent<TargetColorOption>().GetColor();
                ToggleBackground.color = t.GetComponentInChildren<Image>().color;
            }
        }
    }

    public void ResetColorToggles()
    {
        ColorToggles = new List<Toggle>();
    }

    public void ResetColor()
    {
        if (Active)
        {
            ToggleBackground.color = TangibleGameController.Singleton.ColorBinding[TargetColor];
            ToggleIcon.color = Color.white;
        }
        else
        {
            ToggleBackground.color = BgColor;
            ToggleIcon.color = IcColor;
        }
    }

    public void ShowRuleBlock()
    {
        if (Active)
        {
            RuleBlockHorizontal.SetActive(!RuleBlockHorizontal.activeInHierarchy);
            sourceTangible.HideOtherRules(this);
        }
        else
        {
            RuleBlockVertical.SetActive(!RuleBlockVertical.activeInHierarchy);
        }
    }

    public void CheckSide(bool i)
    {
        if (!Active) return;
        if (i != Invert)
        {
            Invert = i;
            RuleBlockHorizontal.GetComponent<RectTransform>().localPosition = HorInvert(RuleBlockHorizontal.GetComponent<RectTransform>());
            var s = i ? -1 : 1;
            HorBackground.localScale = new Vector3(s, 1, 1);
            HorBackground.localPosition = HorInvert(HorBackground);
            DeleteButton.localPosition = HorInvert(DeleteButton);
            QuestionButton.localPosition = HorInvert(QuestionButton);
            TutorialBlock.localPosition = HorInvert(TutorialBlock);
            CollapseButton.localPosition = HorInvert(CollapseButton);
            CollapseButton.localRotation = Quaternion.Euler(0f, 0f, 90f + (-90f * s));
            Content.localPosition = HorInvert(Content);
            Label.localPosition = HorInvert(Label);

            tutorialParent.localPosition = HorInvert(tutorialParent);

            /*
            RectTransform t1r = tutorial1.GetComponent<RectTransform>();
            t1r.localPosition = HorInvert(t1r);

            RectTransform t2r = tutorial2.GetComponent<RectTransform>();
            t2r.localPosition = HorInvert(t2r);
            */
            RectTransform ttext = tutorial1.titleText.GetComponent<RectTransform>();
            ttext.localPosition = HorInvert(ttext);

            RectTransform ttoggle = tutorial1.toggle.GetComponent<RectTransform>();
            ttoggle.localPosition = HorInvert(ttoggle);
        }       
    }

    public void SetRuleType(TangibleGameController.RuleType type)
    {
        RuleType = type;
    }

    public void SetRuleType(int type)
    {
        RuleType = (TangibleGameController.RuleType)type;
    }
    private Vector3 HorInvert(RectTransform rt)
    {
        var p = rt.localPosition;
        return new Vector3(-p.x, p.y, p.z);
    }

    public void SetActive(bool a)
    {
        Active = a;
    }
    
    public void Hide()
    {
        RuleBlockHorizontal.SetActive(false);
        RuleBlockVertical.SetActive(false);
        this.GetComponent<Toggle>().isOn = false;
    }
    
    public void IncRepeat()
    {
        repeatCount++;
        repeatCount = Mathf.Clamp(repeatCount, repeatMin, repeatMax);
    }

    public void DecRepeat()
    {
        repeatCount--;
        repeatCount = Mathf.Clamp(repeatCount, repeatMin, repeatMax);
    }
    
}
