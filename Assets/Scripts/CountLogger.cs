using System.Collections.Generic;
using UnityEngine;

public class CountLogger : MonoBehaviour {
	private string version = "v0.1.1";

	//Clicks
	public int emitterPanelClicks;
	public int linePanelClicks;
	public int soundBankClicks;
	public int lineClicks;
	public int emitterClicks;
	public int shapesButtonClicks;
	public int deleteButtonClicks;
	public int helpButtonClicks;
	public int restartButtonClicks;

	//Quantities
	public int totalLinesCreated;
	public int totalEmittersCreated;
	public int totalBallsCreated;

	public int totalRulesCreated;

	Dictionary<string, int> tutorialClicks;
	void Start() {
		string lastLog = Omnipresent.instance?.logContinuation;
		JsonUtility.FromJsonOverwrite(lastLog, this);
		tutorialClicks = new Dictionary<string, int>();
	}

	void OnDestroy() {
		if (Omnipresent.instance != null) Omnipresent.instance.logContinuation = JsonUtility.ToJson(this);
	}

	public string GetLog() {
		string output = version;
		output += "," + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		output += "," + Time.time.ToString("0");
		output += "," + emitterPanelClicks;
		output += "," + linePanelClicks;
		output += "," + soundBankClicks;
		output += "," + lineClicks;
		output += "," + emitterClicks;
		output += "," + shapesButtonClicks;
		output += "," + deleteButtonClicks;
		output += "," + helpButtonClicks;
		output += "," + restartButtonClicks;
		output += "," + totalLinesCreated;
		output += "," + totalEmittersCreated;
		output += "," + totalBallsCreated;
		output += "," + PlayerPrefs.GetInt("RhythmTutorialCompleted", 0);
		output += "," + PlayerPrefs.GetInt("InstrumentTutorialCompleted", 0);

		output += "," + totalRulesCreated;
		output += "," + GetTutorialClicks();
		
		return output;
	}

	public void IncEmitterPanelClicks() {
		emitterPanelClicks++;
	}

	public void IncLinePanelClicks() {
		linePanelClicks++;
	}

	public void IncSoundBankClicks() {
		soundBankClicks++;
	}

	public void IncShapesButtonClicks() {
		shapesButtonClicks++;
	}

	public void IncDeleteButtonClicks() {
		deleteButtonClicks++;
	}

	public void IncHelpButtonClicks() {
		helpButtonClicks++;
	}

	public void IncRestartButtonClicks() {
		restartButtonClicks++;
	}

	public void IncTutorialClicks(string name)
    {
		if (tutorialClicks == null)
        {
			tutorialClicks = new Dictionary<string, int>();
        }
		string formattedName = name.Replace(' ', '_');
		if (tutorialClicks.TryGetValue(formattedName, out int clicks))
        {
			tutorialClicks[formattedName] = clicks + 1;
        }
		else
        {
			tutorialClicks[formattedName] = 1;
		}
		
    }

	public string GetTutorialClicks()
    {
		
		string output = "";
		if (tutorialClicks == null) return output;
		foreach (KeyValuePair<string, int> pair in tutorialClicks)
		{
			output += "?'" + pair.Key + "':" + pair.Value;
		}
		return output;
	}
}
