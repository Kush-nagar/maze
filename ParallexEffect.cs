using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallexEffect : MonoBehaviour
{

    public Camera cam;
    public Transform followTarget;

    //Starting Z value of the parallex game object 
    Vector2 startingPosition;

    // Start Z value of the parallex game object
    float startingZ;

    Vector2 camMoveSinceStart;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingLane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingLane;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the distance that the camera has moved since the start of the game
        camMoveSinceStart = (Vector2)cam.transform.position - startingPosition;

        // when the target moves, movethe parallex object the same distance times a multipler
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        // The X/Y position changes based on target travel speed times the parallax factor, but x stays consistent
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}