using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlotText : MonoBehaviour {
	public int slot;

	private TMP_Text text;

	void Start() {
		text = GetComponent<TMP_Text>();
		text.text = PlayerPrefs.GetString("SaveSlot" + slot + "lable", "");
	}

	public void setName() {
		PlayerPrefs.SetString("SaveSlot" + slot + "lable", System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
		text.text = PlayerPrefs.GetString("SaveSlot" + slot + "lable", "");
	}

	public void getName() {
		text.text = PlayerPrefs.GetString("SaveSlot" + slot + "lable", "");
	}
}
