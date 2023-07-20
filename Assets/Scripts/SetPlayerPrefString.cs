using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPrefString : MonoBehaviour {
	public string playerPref;
	public string value;

	public void SetPlayerPref() {
		PlayerPrefs.SetString(playerPref, value);
	}
}
