using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Vector3 _dir;

    public float Speed;

    public float Life;

    private bool _init;

    private TangibleController _parent;

    // Update is called once per frame
    void Update()
    {
        if (!_init) return;
        if (Life > 0)
        {
            transform.position += _dir * Speed * Time.deltaTime;
            Life--;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void SetDir(Vector3 d)
    {
        _dir = d;
        Debug.Log("SET DIRECTION TO: " + d);
        _init = true;
    }

    public void SetParent(TangibleController p)
    {
        _parent = p;
    }

    public TangibleController GetParent()
    {
        return _parent;
    }
}
