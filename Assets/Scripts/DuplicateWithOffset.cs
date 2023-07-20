using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_EDITOR)
using UnityEditor;

[ExecuteInEditMode]
public class DuplicateWithOffset : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    public Vector3 initialPosition;
    public Vector3 offset;
    public int count;
    public bool go;

    private void Update()
    {
        if (go)
        {
            Duplicate();
            go = false;
        }
    }
    public void Duplicate()
    {
        Debug.Log("instantiating " + count + " " + prefab.name + "(s)");
        for (int i = 0; i < count; i++)
        {
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            newObj.transform.SetParent(parent);
            newObj.transform.position = initialPosition + parent.TransformPoint(i * offset);
            newObj.name = prefab.name + " ("+i+")";
        }
        
    }
}

#endif