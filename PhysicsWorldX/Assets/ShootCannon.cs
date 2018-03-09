using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCannon : MonoBehaviour
{
    private SteamVR_TrackedObject trackedController;
    private bool controllerInCollider = false; // looks to see if there's a controller in the cannon handle area or not

    private Transform cannonPivot;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // changing how the slingshot behaves by making different inputs on the controller 
        if (trackedController != null) // checks to make sure we have a controller that has entered the cannon handle collider area
        {
            var device = SteamVR_Controller.Input((int)trackedController.index);
            if (controllerInCollider)
            {
                /*
                shootDirection = (endOfCannon - currentBall.transform.position); // the direction the ball flies out at
                shootVelocity = (BASE_VELOCITY * shootDirection.magnitude); // the velocity the ball will fly out at
                
                // if we release the trigger, the slingshot will reset to its original position
                if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    // this code resets the "trigger variables" so the ball can be reloaded etc
                    readyforReload = true;
                    controllerInCannonHandle = false;

                    // Vector3 ballPosition = currentBall.transform.position; 
                    //slingshotGameObject.transform.position = slingshotStartPosition; // slingshot is reset into start position
                    currentBall.transform.parent = null; // "frees" the ball from the slingshot

                    // this code adds the velocity to the ball to send it flying
                    Rigidbody rigidbody = currentBall.GetComponent<Rigidbody>();
                    rigidbody.velocity = shootDirection * shootVelocity;
                    rigidbody.useGravity = true;

                } */

                // if we press the controller trigger, the cannon will pivot around our controller
                if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
                {
                    cannonPivot.transform.LookAt(trackedController.transform.position); // the cannon orientation will now follow the controller
                }
            }
        }
    }
        // tells us we have a controller that entered the collider area
        void OnTriggerEnter(Collider other)
        {
            trackedController = other.GetComponent<SteamVR_TrackedObject>(); // looks to see if there's an object in our collider
            if (trackedController != null)
            {
                controllerInCollider = true;
            }
        }

        // tells us we have a controller that exited the collider area
        void OnTriggerExit(Collider other)
        {
            trackedController = other.GetComponent<SteamVR_TrackedObject>(); // looks to see if there's an object in our collider
            if (trackedController != null)
            {
                controllerInCollider = false;
            }
        }
    }
