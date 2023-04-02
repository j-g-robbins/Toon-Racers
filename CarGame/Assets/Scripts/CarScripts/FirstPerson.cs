using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{

    //     PUBLIC VARIABLES     //
    public Transform cameraTarget;

    public float cameraDistance = 0.0f;
    public float cameraHeight = 0.6f;
    
    public float heightDamping = 0.2f;
    public float rotationYDamping = 0.001f;
    public float rotationXZDamping = 0.1f;

    ///////////////////////////////

    private float wantedHeight;
    private Vector3 wantedRotation;
    private Vector3 wantedPosition;

    private float currentHeight;
    private Vector3 currentRotation;

    private Vector3 offset;

    private float distanceToMove;

    private float rotationYVelocity = 0.0f;
    private float rotationXZVelocity = 1.0f;
    private float distanceVelocity = 0.0f;
    private float heightVelocity = 0.0f;

    private Quaternion returnRotation;

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
        wantedHeight = cameraTarget.position.y + cameraHeight;
        currentHeight = transform.position.y;

        // Update currentHeight with damping
        currentHeight = Mathf.SmoothDampAngle(currentHeight, wantedHeight, ref heightVelocity, heightDamping);


        wantedRotation.x = cameraTarget.eulerAngles.x;
        currentRotation.x = transform.eulerAngles.x;
        currentRotation.x = Mathf.SmoothDampAngle(currentRotation.x, wantedRotation.x, ref rotationXZVelocity, rotationXZDamping);

        wantedRotation.y = cameraTarget.eulerAngles.y;
        currentRotation.y = transform.eulerAngles.y;
        currentRotation.y = Mathf.SmoothDampAngle(currentRotation.y, wantedRotation.y, ref rotationYVelocity, rotationYDamping);

        wantedRotation.z = cameraTarget.eulerAngles.z;
        currentRotation.z = transform.eulerAngles.z;
        currentRotation.z = Mathf.SmoothDampAngle(currentRotation.z, wantedRotation.z, ref rotationXZVelocity, rotationXZDamping);


        // Wanted position
        wantedPosition = cameraTarget.position;

        distanceToMove = Mathf.SmoothDampAngle(Vector3.Distance(wantedPosition, transform.position), cameraDistance, ref distanceVelocity, 0.0f);

        wantedPosition += Quaternion.Euler(0, currentRotation.y, 0) * new Vector3(0, 0, -(cameraDistance + distanceToMove));
        wantedPosition.y = currentHeight;

        transform.position = wantedPosition;

        returnRotation.eulerAngles = currentRotation;
        transform.rotation = returnRotation;
    }
}
