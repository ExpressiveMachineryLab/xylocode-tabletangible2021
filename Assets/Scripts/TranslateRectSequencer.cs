using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Component to schedual translation with a RectTransform
public class TranslateRectSequencer : MonoBehaviour {
	public GameObject mover;
	public RectTransform pointA;
	public RectTransform pointB;

	public float preDelay = 0f;
	public float duration = 1f;
	public float postDelay = 0f;

	public bool moveOnAwake = false;

	public GameObject next;

	private RectTransform startTransform;
	private RectTransform endTransform;
	private RectTransform moverTransform;

	void OnEnable() {
		startTransform = pointA;
		endTransform = pointB;
		moverTransform = mover.GetComponent<RectTransform>();

		if (moveOnAwake) Go();
	}

	public void Go() {
		StartCoroutine(Happen());
	}

	public void SetMoveOnAwake(bool value) {
		moveOnAwake = value;
	}

	IEnumerator Happen() {
		yield return new WaitForSeconds(preDelay);

		float startTime = Time.time;
		float endTime = startTime + duration;
		float now = Time.time;

		while (Time.time <= endTime) {
			now = Time.time;

			moverTransform.position = Vector3.Lerp(startTransform.position, endTransform.position, (now - startTime) / duration);
			moverTransform.localScale = Vector3.Lerp(startTransform.localScale, endTransform.localScale, (now - startTime) / duration);

			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(postDelay);

		if (next != null) next.SendMessage("Go");
	}

}
