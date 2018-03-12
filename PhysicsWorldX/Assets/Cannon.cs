﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    public GameObject ballPrefab; // the prefab of the ball spawned everytime
    public GameObject ballSpawnPoint; // the parent of the ball object where the ball will spawn
    public GameObject ballExitPoint; // the end of the cannon

    public GameObject cannonHandle;
    private Vector3 cannonHandleStartPosition; // the position the handle needs to return to after releasing it 
    
    public GameObject cannonPivot; // the point that acts as the pivot for the cannon
    private Vector3 cannonPivotStartPosition; // the starting transform of the cannon
    //private Vector3 cannonPivotStartRotation; // the starting rotation of the cannon
    
    private GameObject currentBall; // the current ball in the slingshot
    private bool readyforReload = true; // true if there is no ball currently in the cannon 

    private SteamVR_TrackedObject trackedController;
    private bool controllerInCannonHandle = false; // looks to see if there's a controller in the cannon handle area or not

    // variables to keep track of the initial velocity and angle of the ball
    private float BASE_VELOCITY = 30f;
    private Vector3 shootDirection;
    private float extraVelocity; // extra velocity to the ball based on how far the handle is pulled back

    // private variables for the UI display, use helper functions below to get the values
    private float shootAngle;
    private float shootVelocity;

    // helper functions to get private variables for the UI display
    float getShootAngle()
    {
        return Vector3.Angle(Vector3.zero, shootDirection); // the angle parallel to the ground the ball flies out at
    }

    float getShootVelocity()
    {
        return shootVelocity;
    }

    float getHorizontalPosition()
    {
        // returns length of vector after it is projected onto the ground plane
        return (Vector3.ProjectOnPlane((currentBall.transform.position - ballSpawnPoint.transform.position), Vector3.up)).magnitude;
    }

    float getVerticalPosition()
    {
        return currentBall.transform.position.y;
    }

    // Use this for initialization
    void Start()
    {
        // logging the original transforms of the cannon so we can snap back to these positions 
        cannonHandleStartPosition = cannonHandle.transform.position; 
        cannonPivotStartPosition = cannonPivot.transform.position;
        //cannonPivotStartRotation = cannonPivot.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // reloads the ball in the slingshot if there is not one already
        if (readyforReload)
        {
            currentBall = Instantiate(ballPrefab); //creates a ball
            currentBall.transform.parent = ballSpawnPoint.transform; // parents the ball to the spawn point
            currentBall.transform.localPosition = Vector3.zero; // puts the position of the ball to the position of the slingshot (the parent) 
            readyforReload = false; // now there is already a ball in the slingshot so we turn the boolean false 
        }

        //points the forward direction of the ball towards the cannon exit point
        currentBall.transform.LookAt(ballExitPoint.transform.position);

        
        // record how far the handle has been pulled back and square it
        float handleDistance = (cannonHandle.transform.position - cannonHandleStartPosition).magnitude;
        extraVelocity = Mathf.Pow(handleDistance * 5, 1.5f);

        Debug.Log("Distance pulled: " + handleDistance + ", extraVelocity: " + extraVelocity);

        // changing how the slingshot behaves by making different inputs on the controller 
        if (trackedController != null) // checks to make sure we have a controller that has entered the cannon handle collider area
        {
            var device = SteamVR_Controller.Input((int)trackedController.index);
            if (controllerInCannonHandle)
            {
                
                shootDirection = (ballExitPoint.transform.position - ballSpawnPoint.transform.position); // the direction the ball flies out at
                shootVelocity = (BASE_VELOCITY * shootDirection.magnitude); // the velocity the ball will fly out at
                
                // if we release the trigger, the slingshot will reset to its original position
                if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
                {

                    // resets the "trigger variables" so the ball can be reloaded etc
                    readyforReload = true;
                    controllerInCannonHandle = false;

                    // Vector3 ballPosition = currentBall.transform.position; 
                    cannonHandle.transform.position = cannonHandleStartPosition; // handle is reset into start position
                    cannonPivot.transform.position = cannonPivotStartPosition; // position and rotation of cannon is reset into start position
                    //cannonPivot.transform.rotation = cannonPivotStartRotation;

                    currentBall.transform.parent = null; // "frees" the ball from the cannon
                    
                    //puts the ball at the end of cannon and add the velocity to the ball to send it flying
                    currentBall.transform.position = ballExitPoint.transform.position;
                    Rigidbody rigidbody = currentBall.GetComponent<Rigidbody>();
                    rigidbody.useGravity = true;
                    rigidbody.velocity = shootDirection * (shootVelocity + extraVelocity);

                    

                } 

                // if we press the controller trigger, the handle will follow our controller
                if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
                {
                    // *** Add code to make the cannon pivot and move ***
                    cannonHandle.transform.position = trackedController.transform.position;
                    cannonPivot.transform.LookAt(trackedController.transform.position); // the cannon orientation will now follow the controller
                }
            }
        }

    }

    // tells us we have a controller that entered the cannon trigger collider area
    void OnTriggerEnter(Collider other)
    {
        trackedController = other.GetComponent<SteamVR_TrackedObject>(); // looks to see if there's an object in our collider
        if (trackedController != null)
        {
            controllerInCannonHandle = true;
            Debug.Log("Controller entered cannon handle collider");
        }
    }

    // tells us we have a controller that exited the cannon trigger collider area
    void OnTriggerExit(Collider other)
    {
        trackedController = other.GetComponent<SteamVR_TrackedObject>(); // looks to see if there's an object in our collider
        if (trackedController != null)
        {
            controllerInCannonHandle = false;
            Debug.Log("Controller exited cannon handle collider");
        }
    }
}
