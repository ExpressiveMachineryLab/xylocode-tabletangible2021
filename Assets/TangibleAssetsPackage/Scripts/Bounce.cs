using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Bounce : MonoBehaviour
{
    public float Frequency;
    public float Magnitude;
    private RectTransform _rt;
    private Vector3 _startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        _rt = GetComponent<RectTransform>();
        _startPos = _rt.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _rt.localPosition = _startPos + Vector3.up * Mathf.Sin(Time.time * Frequency) * Magnitude;
        
    }
}
