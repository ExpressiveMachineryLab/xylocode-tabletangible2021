using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
	public GameObject cloneObject;
	private GameManager gameManager;
	private SelectionManager SelectionManagerCode;
	private GameObject dragObject;

	void Start() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		SelectionManagerCode = GameObject.Find("SelectedObject").GetComponent<SelectionManager>();
	}

	public void OnDrag(PointerEventData eventData) {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		if (dragObject == null) {
			dragObject = Instantiate(cloneObject, mousePos, cloneObject.transform.rotation, DragCam.drag.transform);
			//SelectionManagerCode.NewSelection(new GameObject[] { dragObject });
		}
		if (dragObject) {
			dragObject.transform.position = mousePos;
		}

	}

	public void OnEndDrag(PointerEventData eventData) {
		dragObject = null;
	}

}