using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Sets object to inactive if there is a mouse click outside of the bounds of this object or it's children
public class ClickOutsideToClose : MonoBehaviour {

	public GameObject[] alsoInclude;

	void Start() {
		Button button = gameObject.AddComponent<Button>();
		button.interactable = true;
		button.transition = Selectable.Transition.None;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
			gameObject.SetActive(ClickingSelfOrChild());
		}
	}

	//Checks if click was inside the bounds of this object or it's children
	private bool ClickingSelfOrChild() {
		RectTransform[] rectTransforms = gameObject.GetComponentsInChildren<RectTransform>();
		foreach (RectTransform rectTransform in rectTransforms) {
			if (EventSystem.current.currentSelectedGameObject == rectTransform.gameObject) {
				return true;
			}
		}

		foreach (GameObject parent in alsoInclude) {
			RectTransform[] rectTransforms2 = parent.GetComponentsInChildren<RectTransform>();
			foreach (RectTransform rectTransform in rectTransforms2) {
				if (EventSystem.current.currentSelectedGameObject == rectTransform.gameObject) {
					return true;
				}
			}
		}

		return false;
	}
}
