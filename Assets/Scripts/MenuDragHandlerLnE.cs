using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuDragHandlerLnE : MonoBehaviour, IDragHandler, IEndDragHandler {
	public SelectedElementType objectType;
	public GameObject cloneObject;
	private GameManager gameManager;
	private SelectionManager SelectionManagerCode;
	private GameObject dragObject;

	public static bool dragging = false;

	void Start() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		SelectionManagerCode = GameObject.Find("SelectedObject").GetComponent<SelectionManager>();
	}

	public void OnDrag(PointerEventData eventData) {
		dragging = true;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		if (dragObject == null) {
			if (objectType == SelectedElementType.Line) dragObject = gameManager.AssignLine(cloneObject);
			if (objectType == SelectedElementType.Emitter) dragObject = gameManager.AssignEmitter(cloneObject);
			dragObject.transform.position = mousePos;
			dragObject.transform.rotation = cloneObject.transform.rotation;

			SelectionManagerCode.NewSelection(new GameObject[] { dragObject });
		}
		if (dragObject) {
			dragObject.transform.position = mousePos;
		}

	}

	public void OnEndDrag(PointerEventData eventData) {
		dragging = false;
		dragObject = null;
	}

}