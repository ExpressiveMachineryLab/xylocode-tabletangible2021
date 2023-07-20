using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPref : MonoBehaviour {
	public string playerPref;

	public void SetString(string value) {
		PlayerPrefs.SetString(playerPref, value);
	}

	public void SetFloat(float value) {
		PlayerPrefs.SetFloat(playerPref, value);
	}

	public void SetInt(int value) {
		PlayerPrefs.SetInt(playerPref, value);
	}
}
