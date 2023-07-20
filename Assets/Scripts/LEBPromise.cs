using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEBPromise : MonoBehaviour {
	private GameManager gameManager;
	void Start() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		if (gameObject.TryGetComponent(out Line line)) gameManager.ReplaceLine(gameObject);
		if (gameObject.TryGetComponent(out Emitter emitter)) gameManager.ReplaceEmitter(gameObject);
		if (gameObject.TryGetComponent(out Ball ball)) gameManager.ReplaceBall(gameObject);

	}
}
