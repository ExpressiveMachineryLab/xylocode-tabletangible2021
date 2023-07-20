using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedElement : MonoBehaviour {
	public SelectedElementType type;
	public ElemColor color;
	public string currentColor;

	public Sprite Red;
	public Sprite Yellow;
	public Sprite Blue;
	public Sprite Green;
	public Sprite Wild;
	public Sprite None;

	private Image image;

	private void Awake() {
		image = gameObject.GetComponent<Image>();
		UpdateImage();
	}

	public string GetCurrentColor() {
		return currentColor;
	}

	public void SetRed() {
		if (image == null) return;
		image.sprite = Red;
		currentColor = Red.name;
		color = ElemColor.red;
	}

	public void SetYellow() {
		image.sprite = Yellow;
		currentColor = Yellow.name;
		color = ElemColor.yellow;
	}

	public void SetBlue() {
		image.sprite = Blue;
		currentColor = Blue.name;
		color = ElemColor.blue;
	}

	public void SetGreen() {
		image.sprite = Green;
		currentColor = Green.name;
		color = ElemColor.green;
	}

	public void SetWild() {
		image.sprite = Wild;
		currentColor = "All";
		color = ElemColor.any;
	}

	public void SetNone() {
		image.sprite = None;
		currentColor = "None";
		color = ElemColor.None;
	}

	public void SetNext() {
		if (color == ElemColor.red) {
			SetYellow();
		} else if (color == ElemColor.yellow) {
			SetBlue();
		} else if (color == ElemColor.blue) {
			SetGreen();
		} else if (color == ElemColor.green) {
			SetWild();
		} else if (color == ElemColor.any) {
			SetNone();
		} else if (color == ElemColor.None) {
			SetRed();
		}
	}

	public void UpdateImage() {
		if (color == ElemColor.red) {
			SetRed();
		} else if (color == ElemColor.yellow) {
			SetYellow();
		} else if (color == ElemColor.blue) {
			SetBlue();
		} else if (color == ElemColor.green) {
			SetGreen();
		} else if (color == ElemColor.any) {
			SetWild();
		} else if (color == ElemColor.None) {
			SetNone();
		}

	}

}

public enum SelectedElementType {
	Ball,
	Line,
	Emitter
}

public enum ElemColor {
	red,
	yellow,
	blue,
	green,
	any,
	None
}