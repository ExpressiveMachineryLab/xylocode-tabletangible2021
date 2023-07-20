using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdleTracker : MonoBehaviour
{
    public float clock = 0f;
    public float TimeToShow = 60f;
    public CanvasGroup objectToShow;

    public bool resetOnEnable;
    public UnityEvent OnShow;
    public UnityEvent OnHide;

    bool showing;
    // Start is called before the first frame update
    void Start()
    {
        clock = resetOnEnable ? TimeToShow : 0f;
    }

    private void OnEnable()
    {
        if (resetOnEnable)
        {
            Reset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (clock > 0f)
        {
            clock -= Time.deltaTime;
            if (!showing)
            {
                showing = true;
                OnShow.Invoke();
            }
        }
        else
        {
            if (showing)
            {
                showing = false;
                OnHide.Invoke();
            }
        }
        if (objectToShow != null)
        objectToShow.alpha = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f).Evaluate(1f - Mathf.Clamp01(clock));


    }

    public void Reset()
    {
        clock = TimeToShow;
    }
}
