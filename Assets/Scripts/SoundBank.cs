using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A container to hold all the values needed for an instrument
[CreateAssetMenu(fileName = "SoundBank", menuName = "SoundBank")]
public class SoundBank : ScriptableObject {
	public string bankName = "New Sound Bank";
	public Phonic voice;
	public float fadeTime = 0.25f;

	public float[] volumes = new float[5];
	public AudioClip[] clips = new AudioClip[5];

	protected float lastPlayTime = 0f;

	void OnEnable() {
		lastPlayTime = 0f;
	}

	public virtual GameObject PlayAudioClip(GameObject source, int index) {

		GameObject playClip = new GameObject("SoundSource");
		playClip.transform.position = source.transform.parent.position;
		playClip.transform.SetParent(source.transform.parent.transform);
		AudioSource sound = playClip.AddComponent<AudioSource>();
		playClip.GetComponent<AudioSource>().spatialBlend = 1;
		playClip.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
		playClip.GetComponent<AudioSource>().dopplerLevel = 0;

		sound.clip = clips[index];
		sound.volume = volumes[index];

		if (Time.time - lastPlayTime > Time.deltaTime) {
			lastPlayTime = Time.time;
			sound.Play();
		}

		Destroy(playClip, sound.clip.length);

		return playClip;
	}
}

public enum Phonic {
	Poly,
	Mono
}