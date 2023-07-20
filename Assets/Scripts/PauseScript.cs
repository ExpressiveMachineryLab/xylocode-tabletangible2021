using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {
	public void SetTimeScale(float newTimeScale) {
		Time.timeScale = Mathf.Clamp01(newTimeScale);
	}
}
