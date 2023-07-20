using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour, IPointerDownHandler
{
    public bool useTouchTracker;
    public List<TouchTracker> touchTrackers;
    public GameObject touchTrackerPrefab;
    public int count;
    public UnityEvent OnTouch;
    public PointerEventData lastTouchData;

    public static TouchController touch;

    void Awake()
    {
        touch = this;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("touch " + eventData.pointerId);
        if (useTouchTracker && touchTrackers.Count <= eventData.pointerId)
        {
            GameObject trackerObj = GameObject.Instantiate(touchTrackerPrefab, this.transform);
            TouchTracker tracker = trackerObj.GetComponent<TouchTracker>();
            tracker.SetIndex(eventData.pointerId);
            touchTrackers.Add(tracker);
        }
        lastTouchData = eventData;
        OnTouch.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {

        if (useTouchTracker)
        {
            touchTrackers = new List<TouchTracker>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        count = Input.touchCount;
    }
}
