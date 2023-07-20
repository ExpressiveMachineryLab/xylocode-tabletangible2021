using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandWindow : MonoBehaviour {
	public GameObject Window;

	public void Expand() {
		if (Window == null) return;
		Window.SetActive(!Window.activeSelf);
	}

	public void Open() {
		if (Window == null) return;
		Window.SetActive(true);
	}

	public void Close() {
		if (Window == null) return;
		Window.SetActive(false);
	}
}
