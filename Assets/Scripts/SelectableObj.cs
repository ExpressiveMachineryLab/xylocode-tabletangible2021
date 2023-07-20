using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectableObj : MonoBehaviour
{
	protected float startPosX;
	protected float startPosY;
	protected bool isBeingHeld = false;
	protected bool isBeingRotated = false;
	protected float clickTimer;
	public float speed = 5f;

	public int pointerID = -1;

	protected SelectionManager selectionManager;
	protected TouchController touchController;

	bool selectionsEnabled;
    void Start()
    {
		selectionManager = SelectionManager.selectionManager;
		touchController = TouchController.touch;
    }
	protected void SelectUpdate()
	{
		return;
		bool inSquareSelect = !(this.transform.parent == null || this.transform.parent.tag != "SelectionParent");
	
		if (Input.touchCount > 0)
		{
			
			if (isBeingHeld && pointerID != -1 && TryGetTouchFromID(out Touch touch1))
			{
				Touch touch = touch1;
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(touch.position);
				if (touch.phase == TouchPhase.Moved)
				{
					transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					isBeingHeld = false;
				}
			}
			else if (isBeingRotated && pointerID != -1 && TryGetTouchFromID(out Touch touch2))
			{
				Touch touch = touch2;
				if (touch.phase == TouchPhase.Moved)
				{
					Rotate();
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					isBeingRotated = false;
				}
			}
			else
            {
				isBeingHeld = false;
				isBeingRotated = false;
				if (Input.touchCount > 0)
				{
					for (int i = 0; i < Input.touchCount; i++)
					{
						Touch touch = Input.GetTouch(i);
						if (touch.phase == TouchPhase.Began)
						{
							Debug.Log(" a touch has begun");
							RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
							Debug.Log(hit.collider);
							if (hit.collider == this.GetComponent<Collider>())
							{
								Debug.Log(" it hit me!");
								pointerID = touch.fingerId;
								MouseDown();
							}
							else if (hit.collider != null && hit.collider.CompareTag("Rotator") && hit.collider == gameObject.transform.GetChild(1).GetComponent<Collider2D>())
							{
								isBeingRotated = true;
								pointerID = touch.fingerId;
							}
						}
					}
				}
			}
		}
		else if (Input.touchCount <= 0)
        {
			isBeingHeld = false;
			isBeingRotated = false;	
        }

			//if (isBeingRotated && !inSquareSelect) Rotate();
		/*
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
			if (hit.collider != null && hit.collider.CompareTag("Rotator") && hit.collider == gameObject.transform.GetChild(1).GetComponent<Collider2D>())
			{
				isBeingRotated = true;
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			isBeingRotated = false;
		}*/
	}

	void Update()
	{
		SelectUpdate();
	}

	private int GetTouchID()
	{
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					return Input.GetTouch(i).fingerId;
				}
			}
		}
		return -1;
	}

	private bool TryGetTouchFromID(out Touch touch)
    {
		touch = new Touch();
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				if (Input.GetTouch(i).fingerId == pointerID)
				{
					touch = Input.GetTouch(i);
					return true;
				}
			}
		}
		return false;
	}
	protected void MouseDown()
    {
		if (Input.touchCount <= 0) return;
		if (!TryGetTouchFromID(out Touch touch)) return;
		//pointerID = Input.touchCount - 1;
		//Touch touch = Input.GetTouch(pointerID);

		//pointerID = GetTouchID();
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint((pointerID == -1) ? (Vector2)Input.mousePosition : touch.position);

		/*
		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch(i);
			Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, );

			if (hit.collider == this.GetComponent<Collider>())
            {
				pointerID = i;
				mousePos = pos;
				break;
            }
        }*/

		// Drag with left click
		if (this.transform.parent == null || this.transform.parent.tag != "SelectionParent")
		{
			startPosX = mousePos.x - this.transform.localPosition.x;
			startPosY = mousePos.y - this.transform.localPosition.y;
		}
		else
		{
			startPosX = mousePos.x - this.transform.parent.localPosition.x;
			startPosY = mousePos.y - this.transform.parent.localPosition.y;
		}

		clickTimer = 0;
		isBeingHeld = true;
	}
	void OnMouseDown()
	{
		//MouseDown();
	}

    protected void MouseUp()
    {
		isBeingHeld = false;
    }
    private void OnMouseUp()
    {
		MouseUp();
	}

	private void Rotate()
	{
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.GetTouch(pointerID).position) - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		rotation *= Quaternion.Euler(0, 0, -90);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
	}
	public virtual void Select() { }
    public virtual void Deselect() { }
}
