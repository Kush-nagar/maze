using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    Vector3 velocity = Vector3.zero;
    public Vector3 positionOffset;

    [Range(0, 1)]
    public float smoothTime;

    public void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(target.name); // Check if the target is assigned correctly
    }


    public void LateUpdate()
    {
        Vector3 targetPosition = target.position + positionOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
