using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSizer : MonoBehaviour
{


    Vector3 startScale;
    float camScale;
    float scaleFactor;

    public Camera dragCam;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        saveCameraScale(dragCam.orthographicSize);
    }


    //call on start to set default camera scale
    public void saveCameraScale(float scale)
    {
        camScale = scale;
        scaleFactor = camScale / startScale.x;
    }

    public void updateScaleAndPosition(float cameraScale)
    {
        // Rect locVec = RectTransformUtility.PixelAdjustRect()
       
        Vector3 newVec = dragCam.transform.position;
        newVec.z = transform.position.z;
        transform.position = newVec;

        float scaleVal = cameraScale / scaleFactor;
        transform.localScale = new Vector3(scaleVal, scaleVal, 1);
    }
}
