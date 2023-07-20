using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundMidiBank", menuName = "SoundMidiBank")]
public class SoundBankMidi : SoundBank {

	[DllImport("__Internal")]
	static extern void JSSendNote(int[] noteChannelPair); // in the javascript

	public int[] midiNotes = new int[5];
	public int midiChannel = 0;

	public override GameObject PlayAudioClip(GameObject source, int index) {

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
#if !UNITY_EDITOR && UNITY_WEBGL
			JSSendNote(new[] { midiNotes[index], midiChannel });
#endif
			sound.Play();
		}

		if (sound.clip != null) Destroy(playClip, sound.clip.length);
		else Destroy(playClip, 0.1f);

		return playClip;
	}
}
