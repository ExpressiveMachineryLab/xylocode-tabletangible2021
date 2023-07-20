using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
	public SelectedElementType type = SelectedElementType.Ball;
	public string id = "";
	public ElemColor color;
	public float speed = 1f;
	public Sprite originalSprite;
	public Sprite hitSprite;
	public EmitterTangible sourceTangible;

	private Rigidbody2D rb;
	private GameManager gameManager;

	private bool markForDestruction = false;

	TrailRenderer trail;
	void Start() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = transform.up * speed * gameManager.GetSpeedMultiplier();

		//Create unique ID
		if (id == "") {
			id = "0" + (int)color;
			RandomString randomstring = new RandomString();
			id += randomstring.CreateRandomString(5);
		} else if (!id[0].Equals("0".ToCharArray()[0])) {
			id = "0" + id;
		}

		trail = GetComponentInChildren<TrailRenderer>();
	}

    void Update() {

		if (trail != null && !trail.emitting)
        {
			trail.emitting = true;
        }
		//Apply movement
		rb.velocity = (rb.velocity.normalized) * speed * gameManager.GetSpeedMultiplier();
		if (markForDestruction && rb.velocity.sqrMagnitude == 0f) gameObject.SetActive(false);
		if (rb.velocity.sqrMagnitude == 0f) markForDestruction = false;
		else markForDestruction = false;
	}

	//Can't see it, don't need it
	void OnBecameInvisible() {
		gameObject.SetActive(false);
		TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
		if (trail != null)
        {
			trail.emitting = false;
        }
	}

	public void ResetVelocity() {
		GetComponent<Rigidbody2D>().velocity = transform.up * speed * GameObject.Find("GameManager").GetComponent<GameManager>().GetSpeedMultiplier();
	}

	//Copy the values of another ball, used by the game manager to manage the ball object pool
	public void BecomeCloneOf(GameObject ballModel) {
		color = ballModel.GetComponent<Ball>().color;
		originalSprite = ballModel.GetComponent<Ball>().originalSprite;
		hitSprite = ballModel.GetComponent<Ball>().hitSprite;
		GetComponent<SpriteRenderer>().sprite = ballModel.GetComponent<SpriteRenderer>().sprite;
		GetComponent<SpriteRenderer>().color = ballModel.GetComponent<SpriteRenderer>().color;

		TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
		if (trail != null)
        {
			trail.startColor = ballModel.GetComponent<SpriteRenderer>().color;
		}
		transform.position = ballModel.transform.position;
		transform.rotation = ballModel.transform.rotation;

		GetComponent<Rigidbody2D>().velocity = transform.up * speed * GameObject.Find("GameManager").GetComponent<GameManager>().GetSpeedMultiplier();
	}

	//Create a string to encapulate the ball's properties
	public string BallToSO() {
		string SOstring = id;
		SOstring += "," + transform.position.x.ToString("0.00") + "," + transform.position.y.ToString("0.00");
		SOstring += "," + transform.rotation.eulerAngles.z.ToString("0.00");

		return SOstring;
	}

	//Assign properties from a string
	public void BallFromSO(string SOball) {
		string[] SOstring = SOball.Split(new[] { "," }, StringSplitOptions.None);

		id = SOstring[0];
		transform.position = new Vector3(float.Parse(SOstring[1]), float.Parse(SOstring[2]), 0);
		transform.eulerAngles = new Vector3(0, 0, float.Parse(SOstring[3]));
	}
}
