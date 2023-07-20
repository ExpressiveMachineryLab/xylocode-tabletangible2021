using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adds a group to the SelectionManager
public class DropGroup : MonoBehaviour {
	public GameObject[] dropGroup;

	void OnEnable() {
		for (int i = 0; i < dropGroup.Length; i++) {
			dropGroup[i].SetActive(true);
		}
		FindObjectOfType<SelectionManager>().NewSelection(dropGroup);
	}

	void Update() {
		if (Input.GetMouseButtonUp(0)) {
			for (int i = 0; i < dropGroup.Length; i++) {
				dropGroup[i].transform.SetParent(null);
			}
			Destroy(gameObject);
		}
	}
}