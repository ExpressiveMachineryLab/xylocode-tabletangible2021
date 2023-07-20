using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActiveSequencer : MonoBehaviour {
	public GameObject activator;
	public bool setTo = true;

	public float preDelay = 0f;
	public float postDelay = 0f;

	public bool goOnAwake = false;

	public GameObject next;

	void OnEnable() {
		if (goOnAwake) Go();
	}

	public void Go() {
		StartCoroutine(Happen());
	}

	IEnumerator Happen() {
		yield return new WaitForSeconds(preDelay);

		activator.SetActive(setTo);

		yield return new WaitForSeconds(postDelay);

		if (next != null) next.SendMessage("Go");
	}
}
