using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashCollider : MonoBehaviour
{
    Collider2D thisCollider;
    public RectTransform trashUI;
    public Camera dragCam;
    public Canvas uicanv;

    Vector3 savedOffset;
    Vector3 startScale;

    float camScale;
    float scaleFactor;

    public CallSound cs;

    public SelectionManager sel;

    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider2D>();
        Vector3 iconPos = dragCam.ScreenToWorldPoint(trashUI.transform.position);
        savedOffset = transform.position - iconPos;

        startScale = transform.localScale;
        saveCameraScale(dragCam.orthographicSize);
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            checkTrash();
        }
    }

    //call on start to set default camera scale
    public void saveCameraScale(float scale)
    {
        camScale = scale;
        scaleFactor = camScale/ startScale.x;
    }

    public void updateScaleAndPosition(float cameraScale)
    {
       // Rect locVec = RectTransformUtility.PixelAdjustRect()
        Vector3 iconPos = dragCam.ScreenToWorldPoint(trashUI.transform.position);
        Vector3 newVec = new Vector3(iconPos.x, iconPos.y, -1f) + savedOffset;
        newVec.z = -1f;
        transform.position = newVec;
        float scaleVal = cameraScale / scaleFactor;
        transform.localScale = new Vector3(scaleVal, scaleVal, 1);
    }

    public void checkTrash()
    {
        List<Collider2D> overlapping = new List<Collider2D>();
        int isOverlap = thisCollider.OverlapCollider(new ContactFilter2D(), overlapping);

        bool mouseOver = false;
        //get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if(hit.collider != null && hit.collider.tag == "trash")
        {
            mouseOver = true;
        }
        if (isOverlap > 0 && mouseOver && sel.square.GetSelected().Count == 0)
        {
            foreach(Collider2D toBeTrashed in overlapping)
            {
                toBeTrashed.gameObject.SetActive(false);
            }
            cs.PlaySound();
        }
        else if (sel.square.GetSelected() != null && sel.square.GetSelected().Count > 0 && mouseOver)
        {
            //then it's a square selected thing.
            sel.square.Delete();
            sel.square.StopSelecting();
            cs.PlaySound();
        }
        
    }
}
