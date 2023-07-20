using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetTextTMPFromSlider : MonoBehaviour {
	public TMP_InputField textBox;
	public Slider slider;

	public void Set() {
		textBox.text = slider.value.ToString("0.0");
	}
}
