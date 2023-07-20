using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public TMP_Text titleText;
    string initTitleText;
    public string replaceString;
    public Color tutorialColor = new Color(1f, .847f, .451f, 1f);
    public string tutorialColorHex = "FFD873";
    [Space(5)]
    public Canvas canvas;
    public RectTransform tutorialContainer;
    public Toggle toggle;
    public TMP_Text pseudoCode;
    string initPseudoCode;
    public bool enableOnUpdate;
    public bool disableOnUpdate;
    [Space(5)]
    public Tutorial nextTutorial;
    public Tutorial prevTutorial;
    public int tutorialLayer = 10;
    public int inactiveTutorialLayer = 9;
    bool active;

    // Start is called before the first frame update
    void Start()
    {
        initTitleText = titleText.text;
        DisableTutorial();
        //initPseudoCode = pseudoCode.text;
    }


    private void Update()
    {
        if (enableOnUpdate)
        {
            enableOnUpdate = false;
            EnableTutorialKeepToggle();
        }
        else if (disableOnUpdate)
        {
            disableOnUpdate = false;
            DisableTutorial();
        }
    }
    public void CheckToggle()
    {
        if (toggle.isOn)
        {
            if (!active)
            {
                EnableTutorial();
            }
        }

        else
        {
            if (nextTutorial != null && nextTutorial.active)
            {
                nextTutorial.DisableTutorialKeepToggle();
            }
            if (prevTutorial != null && prevTutorial.active)
            {
                prevTutorial.DisableTutorialKeepToggle();
            }
            if (active)
            {
                DisableTutorial();
            }
        }
    }

    public void EnableTutorial()
    {
        EnableTutorialKeepToggle();
        toggle.isOn = true;
    }

    public void EnableTutorialKeepToggle()
    {
        active = true;
        tutorialContainer.gameObject.SetActive(true);

        if (nextTutorial != null)
        {
            nextTutorial.DisableTutorialKeepToggle();
        }
        if (prevTutorial != null)
        {
            prevTutorial.DisableTutorialKeepToggle();
        }

        titleText.text = initTitleText.Replace(replaceString, string.Format("<color=#{0}>{1}</color>", tutorialColorHex, replaceString));

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.MarkTutorialOpened(this);
        }
    }
    public void DisableTutorial()
    {
        DisableTutorialKeepToggle();
        toggle.isOn = false;
    }
    public void DisableTutorialKeepToggle()
    {
        active = false;
        tutorialContainer.gameObject.SetActive(false);
        titleText.text = initTitleText;
    }

    public bool IsActive()
    {
        return active;
    }
}
