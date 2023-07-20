using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSelected : MonoBehaviour {
	public Sprite UnselectedSprite;
	public Sprite SelectedSprite;
	public bool isToggled;
	//public GameObject emitter;
	//private EmitterController TangibleController;

	// Start is called before the first frame update
	void Start() {
		//isToggled = false;
		//TangibleController = GameObject.Find("Controller").GetComponent<EmitterController>();
		if (isToggled) {
			this.gameObject.GetComponent<Image>().sprite = SelectedSprite;
		}
	}

	public void toggled() {
		isToggled = !isToggled;

		if (isToggled) {
			this.gameObject.GetComponent<Image>().sprite = SelectedSprite;
		} else {
			this.gameObject.GetComponent<Image>().sprite = UnselectedSprite;
		}
	}

	public void SetToggle(bool newToggle) {
		isToggled = newToggle;

		if (isToggled) {
			this.gameObject.GetComponent<Image>().sprite = SelectedSprite;
		} else {
			this.gameObject.GetComponent<Image>().sprite = UnselectedSprite;
		}

	}
}
