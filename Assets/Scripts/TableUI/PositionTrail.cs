using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTrail : MonoBehaviour
{
    float offset;
    Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void OnEnable()
    {
        offset = transform.localPosition.magnitude;
        rigidbody = this.GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = transform.parent.InverseTransformDirection(rigidbody.velocity.normalized) * -offset;
    }
}
