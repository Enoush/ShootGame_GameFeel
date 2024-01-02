using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offsetVector;
    public float smoothInputSpeed = .2f;

    private Vector3 smoothInputVelocity;
    private Vector3 targetPos;

    private void FixedUpdate()
    {
        targetPos = followTarget.position + offsetVector;
        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref smoothInputVelocity, smoothInputSpeed);
    }
}
