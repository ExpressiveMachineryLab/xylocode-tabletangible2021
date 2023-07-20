using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiInput : MonoBehaviour {

	public void PassMidi(int midiNote) {
		int emitterSelect = midiNote % 4;
		ElemColor shootColor = ElemColor.None;

		switch (emitterSelect) {
			case 0:
				shootColor = ElemColor.red;
				break;
			case 1:
				shootColor = ElemColor.yellow;
				break;
			case 2:
				shootColor = ElemColor.blue;
				break;
			case 3:
				shootColor = ElemColor.green;
				break;
		}

		foreach (GameObjectAgePair frogPair in FindObjectOfType<GameManager>().emitterPool) {
			Emitter frog = frogPair.heldObject.GetComponent<Emitter>();
			if (frog.color == shootColor && frog.isActiveAndEnabled) {
				frog.ShootBall();
			}
		}
	}
}
