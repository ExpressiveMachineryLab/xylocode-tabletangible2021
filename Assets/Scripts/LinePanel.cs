using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinePanel : MonoBehaviour {
	public string id = "";

	public PanelMode mode = PanelMode.Chord;

	[HideInInspector]
	public SelectedElement ballElement, lineElement;

	public SelectedPM selectedChord = SelectedPM.next;
	public int selectedRhythm = 1;

	private bool chordPlus = true;
	
	public Text[] rhythmText;
	public GameObject chordItems;
	public GameObject rhythmItems;
	public GameObject visualItems;

	public GameObject flashPanel;

	public GameObject[] expandedElements;
	public GameObject[] retractedElements;
	public bool expanded;
	private string copyStartState;

	private CountLogger countLogger;

	public SoundChordIndicator chordIndicator;

	bool init = false;
	private void Start() {
		SelectedElement[] elements = GetComponentsInChildren<SelectedElement>();
		foreach (SelectedElement element in elements) {
			if (element.type == SelectedElementType.Ball) ballElement = element;
			if (element.type == SelectedElementType.Line) lineElement = element;
		}

		if (id == "") {
			id = "3";
			RandomString randomstring = new RandomString();
			id += randomstring.CreateRandomString(1);
		} else if (!id[0].Equals("3".ToCharArray()[0])) {
			id = "3" + id;
		}

		copyStartState = LinePanelToSO();
		countLogger = FindObjectOfType<CountLogger>();
		chordIndicator.SetIndicator(1);

		SetExpand(false);
		init = true;
	}

	//Flash the box
	public void FlashBox() {
		if (flashPanel != null) StartCoroutine(Flash(flashPanel));
	}

	IEnumerator Flash(GameObject flashObject) {
		flashObject.SetActive(true);
		yield return new WaitForSeconds(.1f);
		flashObject.SetActive(false);
	}

	public ElemColor GetBallColor() {
		return ballElement.color;
	}

	public ElemColor GetLineColor() {
		return lineElement.color;
	}

	public void CollapseAllLinePanels() {
		LinePanel[] panels = FindObjectsOfType<LinePanel>();
		foreach (LinePanel lp in panels) {
			lp.SetExpand(false);
		}
	}

	public void SetExpand(bool toExpand) {
		if (!this.gameObject.activeInHierarchy) return;
		foreach (GameObject go in expandedElements) {
			go.SetActive(toExpand);
		}
		foreach (GameObject go in retractedElements) {
			go.SetActive(!toExpand);
		}
		RectTransform rect = GetComponent<RectTransform>();
		rect.sizeDelta = new Vector2(rect.sizeDelta.x, toExpand ? 216f : 108f);
		LayoutElement layoutElement = this.GetComponent<LayoutElement>();
		if (layoutElement != null)
        {
			layoutElement.minHeight = toExpand ? 216f : 108f;

		}
		expanded = toExpand;
	}

	//Panel modes
	public void SetModeToChord() {
		mode = PanelMode.Chord;
		if (chordItems == null || rhythmItems == null || visualItems == null || !init) return;
		chordItems.SetActive(true);
		rhythmItems.SetActive(false);
		visualItems.SetActive(false);
	}

	public void SetModeToRhythm() {
		mode = PanelMode.Rhythm;
		if (chordItems == null || rhythmItems == null || visualItems == null || !init) return;
		chordItems.SetActive(false);
		rhythmItems.SetActive(true);
		visualItems.SetActive(false);
	}

	public void SetModeToVisual() {
		mode = PanelMode.Visual;
		if (chordItems == null || rhythmItems == null || visualItems == null || !init) return;
		chordItems.SetActive(false);
		rhythmItems.SetActive(false);
		visualItems.SetActive(true);
	}

	public void UpdateFromInt(int value) {
		switch (value) {
			case 0:
				SetModeToChord();
				break;
			case 1:
				SetModeToRhythm();
				break;
			case 2:
				SetModeToVisual();
				break;
		}
	}

	//Chord options

	public void SetChordToPlus() {
		chordPlus = true;
		SetSelectedChord();
	}

	public void SetChordToPlus(Toggle toggle)
    {
		if (toggle.isOn)
        {
			SetChordToPlus();
        }
    }
	public void SetChordToMinus() {
		chordPlus = false;
		SetSelectedChord();
	}

	public void SetChordToMinus(Toggle toggle)
    {
		if (toggle.isOn)
        {
			SetChordToMinus();
        }
    }

	public void ToggleChord() {
		chordPlus = !chordPlus;
		SetSelectedChord();
	}

	private void SetSelectedChord() {
		if (!init) return;
		if (chordPlus) {
			selectedChord = SelectedPM.next;
			chordIndicator.SetIndicator(1);
		} else {
			selectedChord = SelectedPM.last;
			chordIndicator.SetIndicator(-1);
		}
	}

	//Rhythm options
	public void SetRhythmToNumber(int number) {
		selectedRhythm = number;
		if (selectedRhythm > 8) selectedRhythm = 8;
		if (selectedRhythm < 1) selectedRhythm = 1;
		if (rhythmText.Length > 0) {
			for (int i = 0; i < rhythmText.Length; i++) {
				rhythmText[i].text = "" + selectedRhythm;
			}
		}
	}

	public void IncRhythm() {
		selectedRhythm++;
		if (selectedRhythm > 8) selectedRhythm = 8;
		if (rhythmText.Length > 0) {
			for (int i = 0; i < rhythmText.Length; i++) {
				rhythmText[i].text = "" + selectedRhythm;
			}
		}
	}

	public void DecRhyth() {
		selectedRhythm--;
		if (selectedRhythm < 1) selectedRhythm = 1;
		if (rhythmText.Length > 0) {
			for (int i = 0; i < rhythmText.Length; i++) {
				rhythmText[i].text = "" + selectedRhythm;
			}
		}
	}

	//Interface CountLogger

	public void IncLinePanelClicks() {
		countLogger?.IncLinePanelClicks();
	}

	//Save and load

	public void ResetPanel() {
		LinePanelFromSO(copyStartState);
	}

	public string LinePanelToSO() {
		string SOstring = id;
		SOstring += "," + (int)mode;
		SOstring += selectedRhythm;
		SOstring += (chordPlus ? "1" : "0");
		SOstring += ("0");
		SOstring += ("0");
		SOstring += ("0");
		SOstring += (int)ballElement.color;
		SOstring += (int)lineElement.color;

		return SOstring;
	}

	public void LinePanelFromSO(string SOlinePanel) {
		chordItems.SetActive(true);
		rhythmItems.SetActive(true);
		visualItems.SetActive(true);
		string[] SOstring = SOlinePanel.Split(new[] { "," }, System.StringSplitOptions.None);

		mode = (PanelMode)int.Parse(SOstring[1][0].ToString());
		UpdateFromInt((int)mode);

		selectedRhythm = int.Parse(SOstring[1][1].ToString());
		if (rhythmText.Length > 0) {
			for (int i = 0; i < rhythmText.Length; i++) {
				rhythmText[i].text = "" + selectedRhythm;
			}
		}

		chordPlus = int.Parse(SOstring[1][2].ToString()) == 1 ? true : false;
		SetSelectedChord();

		ballElement.color = (ElemColor)int.Parse(SOstring[1][6].ToString());
		ballElement.UpdateImage();

		lineElement.color = (ElemColor)int.Parse(SOstring[1][7].ToString());
		lineElement.UpdateImage();
	}

	public void UpdateInspectorData()
    {
		InspectorData data = this.GetComponent<InspectorData>();
		LinePanel panel = this;
		string code = "";
		if (panel.ballElement != null && panel.lineElement != null && panel.ballElement.color != ElemColor.None && panel.lineElement.color != ElemColor.None)
		{
			code = "If (" + panel.GetBallColor() + " ball hits a " + panel.GetLineColor() + " line)\n";
			if (panel.mode == PanelMode.Chord)
			{
				code += "Then { go to the " + panel.selectedChord + " sound in the soundbank }";
			}
			if (panel.mode == PanelMode.Rhythm)
			{
				code += "Then { play sound " + panel.selectedRhythm + " time(s) }";
			}
			if (panel.mode == PanelMode.Visual)
			{
				code += "Then { trigger visual effect }";
			}
		}
		else
        {
			//code = "<i>Select a ball and line color!</i>";
        }
		data.code = code;
		data.ForceUpdate();
	}
}

public enum PanelMode {
	Chord,
	Rhythm,
	Visual
}

public enum SelectedPM {
	next,
	last
}