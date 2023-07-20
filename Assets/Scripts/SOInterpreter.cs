using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class SOInterpreter : MonoBehaviour {
	[DllImport("__Internal")]
	static extern void JSGetGameQuery(); // in the javascript

	[HideInInspector]
	public string textInput = "";
	public InputField textField;

	public GameObject[] emitterPrefabs;
	public GameObject[] linePrefabs;
	public GameObject[] ballPrefabs;

	private SessionManager sessionManager;

	private void Start() {
		sessionManager = FindObjectOfType<SessionManager>();

#if !UNITY_EDITOR && UNITY_WEBGL
        JSGetGameQuery();
#endif
	}

	public void CopyToField() {
		textField.text = textInput;
		textInput.CopyToClipboard();
	}

	public void CopyFromField() {
		textInput = textField.text;
		textInput.CopyToClipboard();
	}

	public void SetTextInput(string newInput) {
		textInput = newInput;
	}

	public string GetTextInput() {
		return textInput;
	}

	public void SaveCanvasToSlot(int slot) {
		GenerateSaveDataString();
		PlayerPrefs.SetString("SaveSlot" + slot, textInput);
		if (sessionManager != null) sessionManager.CallLogSession(textInput);
	}

	public void LoadCanvasFromSlot(int slot) {
		FindObjectOfType<GameManager>().ResetApplication();
		textInput = PlayerPrefs.GetString("SaveSlot" + slot, "");
		ParseSaveDataString();
	}

	public void GenerateSaveDataString() {
		textInput = "";

		SoundManager soundMan = FindObjectOfType<SoundManager>();
		textInput += soundMan.SoundManagerToSO() + "_";

		EmitterPanel[] emitterPanels = FindObjectsOfType<EmitterPanel>();
		foreach (EmitterPanel emitterPanel in emitterPanels) {
			textInput += emitterPanel.EmitterPanelToSO() + "_";
		}

		LinePanel[] linePanels = FindObjectsOfType<LinePanel>();
		foreach (LinePanel linePanel in linePanels) {
			textInput += linePanel.LinePanelToSO() + "_";
		}

		Emitter[] emitters = FindObjectsOfType<Emitter>();
		foreach (Emitter emitter in emitters) {
			textInput += emitter.BirdToSO() + "_";
		}

		EmitterTangible[] emitterTangibles = FindObjectsOfType<EmitterTangible>();
		foreach (EmitterTangible emitterT in emitterTangibles)
		{
			textInput += emitterT.BirdToSO() + "_";
		}

		Line[] lines = FindObjectsOfType<Line>();
		foreach (Line line in lines) {
			textInput += line.LineToSO() + "_";
		}

		Ball[] balls = FindObjectsOfType<Ball>();
		foreach (Ball ball in balls) {
			textInput += ball.BallToSO() + "_";
		}

		textInput = textInput.Substring(0, textInput.Length - 1);
	}

	public void ParseSaveDataString() {
		if (textInput == "") return;

		string[] contentsArray = textInput.Split(new[] { "_" }, StringSplitOptions.None);
		EmitterPanel[] emitterPanels = FindObjectsOfType<EmitterPanel>();
		LinePanel[] linePanels = FindObjectsOfType<LinePanel>();
		Emitter[] emitters = FindObjectsOfType<Emitter>();
		Line[] lines = FindObjectsOfType<Line>();
		Ball[] balls = FindObjectsOfType<Ball>();

		GameManager gameManager = FindObjectOfType<GameManager>();

		foreach (string item in contentsArray) {
			if (int.Parse(item[0].ToString()) == 0) {
				string[] SOstring = item.Split(new[] { "," }, StringSplitOptions.None);
				foreach (Ball ball in balls) {
					if (ball.id == SOstring[0]) Destroy(ball.gameObject);
				}
				GameObject newBall = Instantiate(ballPrefabs[int.Parse(item[1].ToString())]);
				newBall.GetComponent<Ball>().BallFromSO(item);
			} else if (int.Parse(item[0].ToString()) == 1) {
				string[] SOstring = item.Split(new[] { "," }, StringSplitOptions.None);
				foreach (Line line in lines) {
					if (line.id == SOstring[0]) line.gameObject.SetActive(false);
				}
				GameObject newLine = gameManager.AssignLine(linePrefabs[int.Parse(item[1].ToString())]);
				newLine.GetComponent<Line>().LineFromSO(item);
				newLine.transform.GetChild(0).gameObject.SetActive(false);
				newLine.transform.GetChild(1).gameObject.SetActive(false);
			} else if (int.Parse(item[0].ToString()) == 2) {
				string[] SOstring = item.Split(new[] { "," }, StringSplitOptions.None);
				foreach (Emitter emitter in emitters) {
					if (emitter.id == SOstring[0]) Destroy(emitter.gameObject);
				}
				GameObject newBird = gameManager.AssignEmitter(emitterPrefabs[int.Parse(item[1].ToString())]);
				newBird.GetComponent<Emitter>().BirdFromSO(item);
				newBird.transform.GetChild(0).gameObject.SetActive(false);
				newBird.transform.GetChild(1).gameObject.SetActive(false);
			} else if (int.Parse(item[0].ToString()) == 3) {
				string[] SOstring = item.Split(new[] { "," }, StringSplitOptions.None);
				foreach (LinePanel linePanel in linePanels) {
					if (SOstring[0] == linePanel.id) {
						linePanel.LinePanelFromSO(item);
					}
				}
			} else if (int.Parse(item[0].ToString()) == 4) {
				string[] SOstring = item.Split(new[] { "," }, StringSplitOptions.None);
				foreach (EmitterPanel emitterPanel in emitterPanels) {
					if (SOstring[0] == emitterPanel.id) {
						emitterPanel.EmitterPanelFromSO(item);
					}
				}

			} else if (int.Parse(item[0].ToString()) == 5) {
				FindObjectOfType<SoundManager>().SoundManagerFromSO(item);
			}
		}


	}

	public void GetSharableLink() {
		GenerateSaveDataString();

		string[] useURL = Application.absoluteURL.Split(new[] { "?" }, StringSplitOptions.None);

		textField.text = useURL[0] + "?game=" + textInput;
		textInput.CopyToClipboard();
	}
}

public static class ClipboardExtension {
	/// <summary>
	/// Puts the string into the Clipboard.
	/// </summary>
	public static void CopyToClipboard(this string str) {
		GUIUtility.systemCopyBuffer = str;
	}
}

public class RandomString {
	public string CreateRandomString(int stringLength = 10) {
		int _stringLength = stringLength - 1;
		string randomString = "";
		string[] characters = new string[] { "a", "b", "c", "d", "e",
											 "f", "g", "h", "i", "j",
											 "k", "l", "m", "n", "o",
											 "p", "q", "r", "s", "t",
											 "u", "v", "w", "x", "y",
											 "z", "A", "B", "C", "D",
											 "E", "F", "G", "H", "I",
											 "J", "K", "L", "M", "N",
											 "O", "P", "Q", "R", "S",
											 "T", "U", "V", "W", "X",
											 "Y", "Z", "1", "2", "3",
											 "4", "5", "6", "7", "8",
											 "9", "0"};
		for (int i = 0; i <= _stringLength; i++) {
			randomString = randomString + characters[UnityEngine.Random.Range(0, characters.Length)];
		}
		return randomString;
	}
}

public class HexConverterTool {
	public string FloatToHex(float value) {
		byte[] vals = BitConverter.GetBytes(value);
		string str = BitConverter.ToString(vals).Replace("-", "");
		return str;
	}

	public float HexToFloat(string value) {
		uint num = uint.Parse(value, System.Globalization.NumberStyles.AllowHexSpecifier);

		byte[] floatVals = BitConverter.GetBytes(num);
		float f = BitConverter.ToSingle(floatVals, 0);

		return f;
	}
}