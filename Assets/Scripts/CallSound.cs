using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallSound : MonoBehaviour {
	public AudioClip sound;
	public bool local = true;

	private SoundManager soundMan;

	void Start() {
		soundMan = GameObject.Find("GameManager").GetComponent<SoundManager>();
	}

	public void PlaySound() {
		soundMan?.PlaySound(sound, local ? gameObject : soundMan.gameObject);
	}
}
