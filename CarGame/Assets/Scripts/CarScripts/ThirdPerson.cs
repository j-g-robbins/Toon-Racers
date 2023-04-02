using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{

    //     PUBLIC VARIABLES     //
    public Transform cameraTarget;

    public float heightDamping = 0.2f;
    public float rotationDamping = 0.3f;
    public float distanceDamping = 0.0f;

    ///////////////////////////////

    private float wantedHeight;
    private float wantedRotation;
    private Vector3 wantedPosition;

    private float currentHeight;
    private float currentRotation;

    private Vector3 offset;

    private float distanceToMove;

    private float rotationVelocity = 0.0f;
    private float distanceVelocity = 0.0f;
    private float heightVelocity = 0.0f;

    private float myTime;



    void Start() 
    {
    }

    // LateUpdate so we move the camera after the car has moved
    void LateUpdate()
    {
        myTime = (Time.deltaTime == 0) ? 0.001f : Time.deltaTime;
        cameraTarget = GameObject.FindWithTag("Car").transform;

        // Calculate current height and wantedheight of the camera
        wantedHeight = cameraTarget.position.y + PlayerPrefs.GetFloat("CameraHeight");
        currentHeight = transform.position.y;

        currentHeight = Mathf.SmoothDampAngle(currentHeight, wantedHeight, ref heightVelocity, heightDamping, Mathf.Infinity, myTime);
        


        // Camera only rotates in the y, so only get those values
        wantedRotation = cameraTarget.eulerAngles.y;
        currentRotation = transform.eulerAngles.y;

        currentRotation = Mathf.SmoothDampAngle(currentRotation, wantedRotation, ref rotationVelocity, rotationDamping, Mathf.Infinity, myTime);


        // Wanted position
        wantedPosition = cameraTarget.position;

        distanceToMove = Mathf.SmoothDampAngle(distanceToMove, PlayerPrefs.GetFloat("CameraDepth"), ref distanceVelocity, distanceDamping, Mathf.Infinity, myTime);
        
        wantedPosition += Quaternion.Euler(0, currentRotation, 0) * new Vector3(0, 0, -distanceToMove);
        wantedPosition.y = currentHeight;

        transform.position = wantedPosition;
        
        transform.LookAt(cameraTarget.position + offset);
    }   
}




