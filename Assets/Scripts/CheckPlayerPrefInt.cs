using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Triggers a UnityEvent based on the value of a PLayerPref
public class CheckPlayerPrefInt : MonoBehaviour {
	public string playerPrefName;
	public int checkAgainstValue = 1;
	public int dafaultValue = 0;
	public Conditianals conditional;

	public float preDelay = 0f;
	public float postDelay = 0f;

	public bool goOnAwake = false;

	public GameObject next;

	public UnityEvent ifTrue = null;
	public UnityEvent ifFalse = null;

	void OnEnable() {
		if (goOnAwake) Go();
	}

	public void Go() {
		StartCoroutine(Happen());
	}

	public void AssignThisPlayerPrefInt(int value) {
		PlayerPrefs.SetInt(playerPrefName, value);
	}

	IEnumerator Happen() {
		yield return new WaitForSeconds(preDelay);

		int theValue = PlayerPrefs.GetInt(playerPrefName, dafaultValue);

		switch (conditional) {
			case Conditianals.Equal:
				if (theValue == checkAgainstValue) ifTrue?.Invoke();
				else ifFalse?.Invoke();
				break;
			case Conditianals.GreaterThan:
				if (theValue > checkAgainstValue) ifTrue?.Invoke();
				else ifFalse?.Invoke();
				break;
			case Conditianals.GreaterThanOrEqualTo:
				if (theValue >= checkAgainstValue) ifTrue?.Invoke();
				else ifFalse?.Invoke();
				break;
			case Conditianals.LessThan:
				if (theValue < checkAgainstValue) ifTrue?.Invoke();
				else ifFalse?.Invoke();
				break;
			case Conditianals.LessThanOrEqualTo:
				if (theValue <= checkAgainstValue) ifTrue?.Invoke();
				else ifFalse?.Invoke();
				break;
		}

		yield return new WaitForSeconds(postDelay);

		if (next != null) next.SendMessage("Go");
	}

	public enum Conditianals {
		Equal,
		GreaterThan,
		GreaterThanOrEqualTo,
		LessThan,
		LessThanOrEqualTo
	}
}
