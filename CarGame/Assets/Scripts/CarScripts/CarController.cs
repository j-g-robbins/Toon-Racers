using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody sphereRigidbody;
    public LayerMask ground;

    public float airDrag;
    public float groundDrag;
    public float slowDrag;
    public float boostDrag;

    private float directionInput;
    private float turnInput;
    private bool onGround;
    private float airTime = 0.2f;

    public float forwardSpeed;
    public float reverseSpeed;
    public float boostMultiplier;
    public float turnSpeed;
    public float turnMultiplier;

    public float turnLockTime = 0.1f;
    private float turnTime;

    public Transform FLW;
    public Transform FRW;
    public Transform RLW;
    public Transform RRW;

    private CheckPointManager track;
    public float resetPenalty = 0.5f;
    private float timeSinceLastReset;
    private bool isBoost;

    void Start()
    {
        sphereRigidbody.transform.parent = null;
        timeSinceLastReset = -2.5f;
        turnTime = 0.0f;
    }

    void Update()
    {
        timeSinceLastReset += Time.deltaTime;

        // Handle forward and back input

        directionInput = Input.GetAxisRaw("Vertical");
        directionInput *= directionInput > 0 ? forwardSpeed : reverseSpeed;

        if (timeSinceLastReset < resetPenalty)
        {
            directionInput = 0;
        }


        if(Input.GetKeyDown(KeyCode.F))
        {
            track = GameObject.FindGameObjectWithTag("GameController").GetComponent<CheckPointManager>();
            //Debug.Log(track.lastCheckPoint.position);
            sphereRigidbody.transform.position = track.lastCheckPointPos;
            //sphereRigidbody.transform.rotation = track.lastCheckPointRot;
            sphereRigidbody.transform.rotation = track.lastCheckPointRot;
            this.transform.rotation = track.lastCheckPointRot;
            sphereRigidbody.velocity = new Vector3(0, 0, 0);
            if (timeSinceLastReset > 0.0f) {
                timeSinceLastReset = 0.0f;
            }
            

        }

        // Make car have same position as sphere
        transform.position = sphereRigidbody.transform.position;

        float speed = Vector3.Dot(sphereRigidbody.velocity, transform.forward);
        
        // Handle side input

        turnInput = Input.GetAxisRaw("Horizontal");

        if (turnInput != 0) {
            turnTime += Time.deltaTime;
            turnInput = turnInput * Mathf.Clamp(((turnTime*turnTime)/(turnLockTime*turnLockTime)), 0, 1);
        }
        else {
            turnTime = 0.0f;
        }

        // Only update Y coordinate of car with respect to sphere so car stays horizontal, multiply by vertical input to stop car rotating hwen not moving
        if (onGround) {
            float newRotation = turnInput * turnSpeed * Time.deltaTime * sphereRigidbody.velocity.magnitude * turnMultiplier;

            if (speed >= 0) {
                transform.Rotate(0, newRotation, 0, Space.World);
            }
            else {
                transform.Rotate(0, -newRotation, 0, Space.World);
            }

            airTime = 0.0f;
           
        }
        else {
            airTime += Time.deltaTime;
            if (airTime > 0.2f) {
            
                Vector3 curRotation = transform.rotation.eulerAngles;
                transform.localEulerAngles = new Vector3(0, curRotation.y, 0);
            }
        }

        // Check ground direction with raycast to adjust tilt of car
        RaycastHit ray;
        onGround = Physics.Raycast(transform.position, -transform.up, out ray, 1f, ground);
        transform.rotation = Quaternion.FromToRotation(transform.up, ray.normal) * transform.rotation;

        if (onGround) {

            if (Input.GetKey(KeyCode.Space)){
                sphereRigidbody.drag = boostDrag;
                directionInput *= boostMultiplier;
                isBoost = true;
            }
            else {
                sphereRigidbody.drag = groundDrag;
                isBoost = false;
            }

            if (Input.GetAxisRaw("Vertical") < 0.9) {
                sphereRigidbody.drag = slowDrag;
            }
            

        }
        else {
            sphereRigidbody.drag = airDrag;
            isBoost = false;
        }

        rotateWheels(speed);
        turnWheels(turnInput);
        
    }

    private void FixedUpdate()
    {
        
        if (onGround) {
            sphereRigidbody.AddForce(transform.forward * directionInput, ForceMode.Acceleration);
        }
        else {
            sphereRigidbody.AddForce(new Vector3(0f, 1f, 0f) * -30f);
        }

    }

    private void rotateWheels(float input) {
        input = input * 0.25f;
        FLW.Rotate(input, 0, 0);
        FRW.Rotate(input, 0, 0);
        RLW.Rotate(input, 0, 0);
        RRW.Rotate(input, 0, 0);
    }

    private void turnWheels(float turnInput) {
        Vector3 carRotation = transform.rotation.eulerAngles;
        Vector3 tyreRotation = new Vector3(carRotation.x, carRotation.y + (turnInput * 30), carRotation.z);
        FLW.rotation = Quaternion.Slerp(FLW.rotation, Quaternion.Euler(tyreRotation), 2);
        FRW.rotation = Quaternion.Slerp(FRW.rotation, Quaternion.Euler(tyreRotation), 2);
    }

    public float carSpeed() {
        if (onGround) {
            return sphereRigidbody.velocity.magnitude;
        }
        return 0;
       
    }

    public float getTurnInput() {
        return turnInput;
    }

    public bool getIsBoost() {
        return isBoost;
    }
}
