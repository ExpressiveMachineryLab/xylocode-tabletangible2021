using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOnAnyClick : MonoBehaviour
{

	void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
		{
			gameObject.SetActive(false);
		}
	}

	private IEnumerator LateClose()
	{
		yield return new WaitForSeconds(0.1f);

		gameObject.SetActive(false);
	}
}
