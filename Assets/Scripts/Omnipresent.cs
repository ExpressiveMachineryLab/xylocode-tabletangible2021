using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omnipresent : MonoBehaviour {
	public static Omnipresent instance;

	public string logContinuation = "";

	void Start() {
		if (Omnipresent.instance == null) {
			Omnipresent.instance = this;
		} else {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}
}
