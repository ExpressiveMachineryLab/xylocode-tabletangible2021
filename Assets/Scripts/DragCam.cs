using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCam : MonoBehaviour
{

    Camera cam;
    public static DragCam drag;

    public bool inertia = true;
    public float decaySpeed = 1f;
    [Space(5)]
    public float maxWidth = 50;
    public float maxHeight = 50;
    Vector3 initialPosition;
    [Space(5)]
    public Vector3 delta;
    public Vector3 lastMouse;
    [Space(10)]
    public float zoomSpeed = 1f;
    public float minZoom = 0.1f;
    public float maxZoom = 10f;
    [Space(5)]
    public float zoom = 1f;
    public float zoomPercent = 0f;
    public bool scrollThisFrame;

    public TrashCollider trashCollider;
    public BackgroundSizer backgroundObj;
    public GameObject soundBank;

    public void Drag(Vector3 delt)
    {
        this.delta = delt;
        //cam.transform.Translate(-delta);
        this.transform.Translate(-delta);

        Vector3 relativePosition = this.transform.position - initialPosition;
        Vector3 boundsAdjustment = Vector3.zero;
        if (relativePosition.x > maxWidth)
        {
            boundsAdjustment += new Vector3(relativePosition.x - maxWidth, 0, 0);
        }
        else if (relativePosition.x < -maxWidth)
        {
            boundsAdjustment += new Vector3(relativePosition.x + maxWidth, 0, 0);
        }
        if (relativePosition.y > maxHeight)
        {
            boundsAdjustment += new Vector3(0,relativePosition.y - maxHeight, 0);
        }
        else if (relativePosition.y < -maxHeight)
        {
            boundsAdjustment += new Vector3(0,relativePosition.y + maxHeight, 0);
        }
        transform.Translate(-boundsAdjustment);

        trashCollider.updateScaleAndPosition(zoom);
        backgroundObj.updateScaleAndPosition(zoom);
    }

    public void ZoomDelta(float percent)
    {
        Zoom(zoomPercent + percent);
        //transform.Translate(0, 0, zoom - this.transform.position.z);
        //transform.localScale = new Vector3(zoom, zoom, 1f);
    }

    public void Zoom(float percent)
    {
        if (soundBank.activeInHierarchy) return;
        zoomPercent = Mathf.Clamp(percent, 0f, 1f);

        zoom = Mathf.Lerp(minZoom, maxZoom, 1f - zoomPercent);

        cam.orthographicSize = zoom;
        trashCollider.updateScaleAndPosition(zoom);

        backgroundObj.updateScaleAndPosition(zoom);
    }

    private void Awake()
    {
        drag = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        initialPosition = this.transform.position;

        zoom = cam.orthographicSize;
        zoomPercent = (zoom - minZoom) / (maxZoom - minZoom);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));
        if (Input.GetMouseButtonDown(1))
        {
            lastMouse = mouseWorldPos;
        }
        if (Input.GetMouseButton(1))
        {
            
            Vector3 mouseWorldDelta = lastMouse - mouseWorldPos;
            //Vector3 mouse = new Vector3(Input.GetAxisRaw("Mouse X") * mouseSpeed, Input.GetAxisRaw("Mouse Y") * mouseSpeed);
            Drag(mouseWorldDelta);
            lastMouse = mouseWorldPos;
        }
        if (!Input.GetMouseButton(1))
        {
            if (inertia && delta.magnitude > 0)
            {
                Vector3 deltaLerp = Vector3.MoveTowards(delta, Vector3.zero, decaySpeed * Time.deltaTime);
                Drag(deltaLerp);
            }
        }
        ZoomDelta(Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed);
        scrollThisFrame = Input.GetAxisRaw("Mouse ScrollWheel") != 0f;
    }
}
