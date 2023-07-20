using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElementExpander : MonoBehaviour {
	public GameObject[] elements;
	public GameObject addState;
	public GameObject subtractState;
	public GameObject bothState;

	void Start() {
		SetState();
	}

	private void SetState() {
		int currentActive = 0;
		foreach (GameObject element in elements) {
			if (element.activeSelf) currentActive++;
		}
		addState.SetActive(false);
		subtractState.SetActive(false);
		bothState.SetActive(false);
		if (currentActive <= 0) addState.SetActive(true);
		else if (currentActive >= elements.Length) subtractState.SetActive(true);
		else bothState.SetActive(true);
	}

	public void AddButtonPressed() {
		for (int i = 0; i < elements.Length; i++) {
			if (!elements[i].activeSelf) {
				elements[i].SetActive(true);
				SetState();
				return;
			}
		}
	}

	public void SubtractButtonPressed() {
		for (int i = elements.Length-1; i >= 0; i--) {
			if (elements[i].activeSelf) {
				elements[i].SetActive(false);
				SetState();
				return;
			}
		}
	}
}
