using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeOffset : MonoBehaviour {
	public GameObject referanceObject;

	private Vector3 relativePosition;
	private Vector3 myPosition;

	void Start() {
		relativePosition = referanceObject.transform.position;
		myPosition = gameObject.transform.position;
	}

	void Update() {
		gameObject.transform.position = myPosition + (referanceObject.transform.position - relativePosition);
	}

}
