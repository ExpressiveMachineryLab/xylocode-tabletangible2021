using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A container for a collection of soundbanks
[CreateAssetMenu(fileName = "StyleBank", menuName = "StyleBank")]
public class StyleBank : ScriptableObject {
	public string styleName = "New Style Bank";

	public SoundBank redBank;
	public SoundBank yellowBank;
	public SoundBank blueBank;
	public SoundBank greenBank;
}
