using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MidiControlField : MonoBehaviour {
	public SoundBankMidi midiBank;
	public TMP_InputField[] noteInputs;
	public TMP_InputField channelInput;

	void OnEnable() {
		for (int i = 0; i < 5; i++) {
			noteInputs[i].text = midiBank.midiNotes[i].ToString();
		}
		channelInput.text = (midiBank.midiChannel + 1).ToString();
	}

	public void SetMidiNumbers() {
		for (int i = 0; i < 5; i++) {
			midiBank.midiNotes[i] = int.Parse(noteInputs[i].text);
			midiBank.midiNotes[i] = Mathf.Clamp(midiBank.midiNotes[i], 0, 127);
			noteInputs[i].text = midiBank.midiNotes[i].ToString();
		}
	}

	public void SetMidiChannel() {
		midiBank.midiChannel = int.Parse(channelInput.text) - 1;
		midiBank.midiChannel = Mathf.Clamp(midiBank.midiChannel, 0, 15);
		channelInput.text = (midiBank.midiChannel + 1).ToString();
	}
}
