using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RightPanelExpansion : MonoBehaviour
{


    public Button expandButton;
    public Button collapseButton;

    public GameObject panel;

    public GameObject tutorials;
    public GameObject inspector;

    public TutorialManager tutMan;

    public Toggle tutorialToggle;
    public Toggle inspectorToggle;
    [Header("Click to expand in inspector")]
    public bool ShowInspector;
    public bool ShowTutorial;

    bool first = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ShowInspector)
        {
            ShowInspector = false;
            SwitchToInspector();
        }
        if (ShowTutorial)
        {
            ShowTutorial = false;
            SwitchToTutorial();
        }
        
    }

    public void Expand()
    {
        panel.SetActive(true);
        expandButton.gameObject.SetActive(false);
        collapseButton.gameObject.SetActive(true);

        //SwitchToTutorial();
        //tutorialToggle.Select();
       // tutorialToggle.GetComponentInChildren<TMP_Text>().color = Color.white;
    }

    public void Collapse()
    {
        panel.SetActive(false);
        expandButton.gameObject.SetActive(true);
        collapseButton.gameObject.SetActive(false);
    }

    public void SwitchToInspector(Toggle toggle)
    {
        if (toggle.isOn)
        {
            SwitchToInspector();
        }
    }

    public void SwitchToInspector()
    {
        inspector.SetActive(true);
        tutorials.SetActive(false);
    }

    public void SwitchToTutorial(Toggle toggle)
    {
        
        if (toggle.isOn)
        {
            SwitchToTutorial();
        }
    }

    public void SwitchToTutorial()
    {
        inspector.SetActive(false);

        tutorials.SetActive(true);
        tutMan.StartTurotial(0);
    }
}
