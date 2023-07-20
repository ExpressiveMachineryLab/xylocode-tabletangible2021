using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject popup;
    public GameObject pointer;
    public Button prevButton;
    public Button nextButton;
    public TMP_Text titleText;
    public TMP_Text pageText;
    public TMP_Text pageNumberText;
    public GameObject challengeIcon;
    public GameObject progressText;
    public GameObject progressItemPrefab;
    public GameObject tutorialPanel;

    public TutSequence[] sequences;

    public TMP_Text description;

    public Color regularColor;
    public Color challengeColor;
    public Color popupButtonColor;
    public Color popupButtonDisabled;

    public Sprite startButton;
    public Sprite resumeButton;

    private int tutorialIndex = 0;
    private int popupIndex = 0;

    private bool tutorialActive = false;

    public TMP_Text popupPrevText;
    public TMP_Text popupNextText;

    public Image popupPrevButton;
    public Image popupNextButton;

    public Vector3 popupOffset;

    void Start()
    {
       // popup.SetActive(false);
       // pointer.SetActive(false);
       // progressText.SetActive(false);
       // tutorialPanel.SetActive(true);


        foreach (Transform tutSelector in tutorialPanel.transform)
        {
            tutSelector.GetComponent<Image>().sprite = startButton;
        }

    }

    void doPopupButtons()
    {
        //do prev button
        if (popupIndex == 0)
        {
            popupPrevButton.color = popupButtonDisabled;
            popupPrevText.color = popupButtonDisabled;
        }
        else
        {
            popupPrevButton.color = popupButtonColor;
            popupPrevText.color = popupButtonColor;
        }

        //do next button
        if (popupIndex == sequences[tutorialIndex].sequnce.Length - 1)
        {
            popupNextButton.color = popupButtonDisabled;
            popupNextText.color = popupButtonDisabled;
        }
        else
        {
            popupNextButton.color = popupButtonColor;
            popupNextText.color = popupButtonColor;
        }
    }

    public void StartTurotial(int index)
    {
        if (tutorialActive) return;
        tutorialIndex = index;
        if (sequences[tutorialIndex].progress.Count > 0)
        {
            popupIndex = sequences[tutorialIndex].progress[sequences[tutorialIndex].progress.Count - 1];
        }
        else
        {
            popupIndex = 0;
            sequences[tutorialIndex].progress.Add(0);
        }

        FillPopup();
        setupProgressText();
        FillPorgressText();
        popup.SetActive(true);
        progressText.SetActive(true);
       // tutorialPanel.SetActive(false);
        tutorialActive = true;
        //updateTutButtons();
        sequences[tutorialIndex].progressBar.updateTutorialProgress(sequences[tutorialIndex].progress.Count, sequences[tutorialIndex].sequnce.Length);
    }

    public void StopTutorial()
    {
        popup.SetActive(false);
        pointer.SetActive(false);
       // progressText.SetActive(false);
       // tutorialPanel.SetActive(true);
        tutorialActive = false;
       // updateTutButtons();
    }

    void updateTutButtons()
    {
        int index = 0;
        foreach (Transform tutSelector in tutorialPanel.transform)
        {
            if (sequences[index].progress.Count > 0)
            {
                tutSelector.GetComponent<Image>().sprite = resumeButton;
            }
            index++;
        }
    }

    public void NextPopup()
    {
        if (popupIndex >= sequences[tutorialIndex].sequnce.Length - 1) return;
        popupIndex++;
        FillPopup();
        FillPorgressText();
        updateTutButtons();
    }

    public void PreviousPopup()
    {
        if (popupIndex <= 0) return;
        popupIndex--;
        FillPopup();
        updateTutButtons();
    }

    public void navigateToIndex(Button i)
    {
        int index = int.Parse(i.GetComponent<TMP_Text>().text.Split('.')[0]) - 1;
        if (index >= sequences[tutorialIndex].sequnce.Length) return;

        popupIndex = index;
        FillPopup();

        updateTutButtons();
        FillPorgressText();
    }

    private void FillPopup()
    {
        popup.SetActive(true);
        if (!sequences[tutorialIndex].progress.Contains(popupIndex))
        {
            sequences[tutorialIndex].progress.Add(popupIndex);
        }
        sequences[tutorialIndex].progressBar.updateTutorialProgress(sequences[tutorialIndex].progress.Count, sequences[tutorialIndex].sequnce.Length);


        titleText.text = sequences[tutorialIndex].sequnce[popupIndex].cardTitle;
        pageText.text = sequences[tutorialIndex].sequnce[popupIndex].cardText;
        pageNumberText.text = "" + (popupIndex + 1).ToString() + "/" + (sequences[tutorialIndex].sequnce.Length);
        popup.GetComponent<RectTransform>().anchoredPosition = sequences[tutorialIndex].sequnce[popupIndex].popupPosition + popupOffset;

        if (sequences[tutorialIndex].sequnce[popupIndex].challenge)
        {
            titleText.color = challengeColor;
            titleText.fontSize = 24f;
        }
        else
        {
            titleText.color = regularColor;
            titleText.fontSize = 18f;
        }

        prevButton.interactable = popupIndex >= 1;
        nextButton.interactable = popupIndex < sequences[tutorialIndex].sequnce.Length - 1;
        //prevButton.SetActive(popupIndex >= 1);
        //nextButton.SetActive(popupIndex < sequences[tutorialIndex].sequnce.Length - 1);
        challengeIcon.SetActive(sequences[tutorialIndex].sequnce[popupIndex].challenge);

        pointer.SetActive(sequences[tutorialIndex].sequnce[popupIndex].usePointer);
        pointer.GetComponent<RectTransform>().anchoredPosition = sequences[tutorialIndex].sequnce[popupIndex].pointerPosition + popupOffset;
        pointer.transform.rotation = Quaternion.Euler(0f, 0f, sequences[tutorialIndex].sequnce[popupIndex].pointerRotation);
        doPopupButtons();
        FillPorgressText();
    }

    private void FillPorgressText()
    {

        GameObject[] tableOfContents = new GameObject[progressText.transform.childCount];
        int index = 0;

        foreach (Transform childObj in progressText.transform)
        {
            tableOfContents[index] = childObj.gameObject;
            if (sequences[tutorialIndex].progress.Contains(index))
            {
                TMP_Text toBold = tableOfContents[index].GetComponent<TMP_Text>();
                toBold.fontStyle = FontStyles.Bold;
            }
            index++;
        }

        //string newText = "<b>" + sequences[tutorialIndex].tutTitle + "\n\n";
        //for (int i = 0; i < sequences[tutorialIndex].sequnce.Length; i++) {
        //	newText += i + ". " + sequences[tutorialIndex].sequnce[i].cardTitle;

        //	if (i == sequences[tutorialIndex].progress) newText += "</b>";
        //	newText += "\n\n";
        //}

        //progressText.GetComponent<TMP_Text>().text = newText;
    }

    private void setupProgressText()
    {
        //clear any existing children
        foreach (Transform t in progressText.transform)
        {
            Destroy(t.gameObject);
        }
        //GameObject firstChild = Instantiate(progressItemPrefab, progressText.transform);
        //Destroy(firstChild.GetComponent<Button>());
       // firstChild.GetComponent<TMP_Text>().SetText(sequences[tutorialIndex].tutTitle);
       // firstChild.GetComponent<TMP_Text>().fontSize = 16;

        for (int i = 0; i < sequences[tutorialIndex].sequnce.Length; i++)
        {
            GameObject newChild = Instantiate(progressItemPrefab, progressText.transform);
            newChild.GetComponent<TMP_Text>().SetText((i + 1).ToString() + ". " + sequences[tutorialIndex].sequnce[i].cardTitle);

            Button b = newChild.GetComponent<Button>();
            b.onClick.AddListener(delegate { navigateToIndex(b); });

            if (sequences[tutorialIndex].progress.Contains(i))
            {
                TMP_Text toBold = newChild.GetComponent<TMP_Text>();
                toBold.fontStyle = FontStyles.Bold;
            }

            if (sequences[tutorialIndex].sequnce[i].cardTitle == "Challenge")
            {
                newChild.GetComponent<TMP_Text>().color = challengeColor;
            }
            //	newText += i + ". " + sequences[tutorialIndex].sequnce[i].cardTitle;

            //	if (i == sequences[tutorialIndex].progress) newText += "</b>";

        }


    }
}

[System.Serializable]
public class TutSequence
{
    public string tutTitle;
    public string tutDescription;
    public TutPopup[] sequnce;
    public List<int> progress = new List<int>();
    public ProgressBar progressBar;
}

[System.Serializable]
public class TutPopup
{
    public string cardTitle;
    public Vector3 popupPosition;
    public Vector3 pointerPosition;
    public float pointerRotation;
    public bool usePointer;
    public bool challenge;
    [TextArea]
    public string cardText;
}