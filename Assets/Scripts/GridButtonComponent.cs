using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridButtonComponent : MonoBehaviour {
	public int index = 0;
	public GridButtonType type = GridButtonType.Sound;
	public StyleHUD hud;

	private CountLogger logger;

	void Start() {
		logger = FindObjectOfType<CountLogger>();
	}

	public void Execute() {
		if (logger != null) logger.IncSoundBankClicks();

		if (type == GridButtonType.Style) {
			hud.SetStyle(index);
			hud.MarkChange();
		}
		if (type == GridButtonType.Sound) {
			hud.SetSound(index);
			hud.ResetTextNames();
			hud.MarkChange();
		}

	}

	public void ExecuteIfOn(Toggle toggle)
    {
		if (toggle.isOn)
        {
			Execute();
        }
    }

	void OnGUI()
    {
		//this.transform.localScale = new Vector3(1f, 1f, 1f);
	}

}

public enum GridButtonType {
	Style,
	Sound
}