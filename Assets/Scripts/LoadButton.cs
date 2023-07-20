using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour {
	public int slot;

	private Button button;

	void Start() {
		button = GetComponent<Button>();

		if (PlayerPrefs.GetString("SaveSlot" + slot, "") == "") {
			button.interactable = false;
		}
	}
}
