using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MidiTest : MonoBehaviour {
	[DllImport("__Internal")]
	static extern void JSSendNote(int noteNumber); // in the javascript

	public int midiNumber = 60;

	public void PlayNote() {
		JSSendNote(midiNumber);
	}
}
