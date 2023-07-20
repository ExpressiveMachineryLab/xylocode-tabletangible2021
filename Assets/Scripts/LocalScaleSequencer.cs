using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalScaleSequencer : MonoBehaviour {
	public GameObject scalor;
	public Vector3 scaleFrom = new Vector3(1f, 1f, 1f);
	public Vector3 scaleTo = new Vector3(1.2f, 1.2f, 1.2f);

	public float preDelay = 0f;
	public float duration = 0f;
	public float postDelay = 0f;

	public bool goOnAwake = false;

	public GameObject next;

	private RectTransform scalorTransform;

	void OnEnable() {
		scalorTransform = scalor.GetComponent<RectTransform>();
		if (goOnAwake) Go();
	}

	public void Go() {
		StopCoroutine(Happen());
		StartCoroutine(Happen());
	}

	public void ResetObjectScale() {
		StopAllCoroutines();
		scalorTransform.localScale = Vector3.one;
	}

	IEnumerator Happen() {
		yield return new WaitForSeconds(preDelay);

		float startTime = Time.time;
		float endTime = startTime + duration;
		float now = Time.time;

		while (Time.time <= endTime) {
			now = Time.time;

			scalorTransform.localScale = Vector3.Lerp(scaleFrom, scaleTo, (now - startTime) / duration);

			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(postDelay);

		if (next != null) next.SendMessage("Go");
	}
}
