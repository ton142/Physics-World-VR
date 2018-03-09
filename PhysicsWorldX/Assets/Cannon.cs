using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    public GameObject ballPrefab; // the prefab of the ball spawned everytime
    public GameObject ballSpawnPoint; // the parent of the ball object where the ball will spawn
    public Vector3 endOfCannon; // the world space position the ball leaves the cannon

    public GameObject cannonPivot; // the point that acts as the pivot for the cannon

    //private Vector3 cannonStartPosition; // the position the slingshot object needs to return to
    private GameObject currentBall; // the current ball in the slingshot
    private bool readyforReload = true; // true if there is no ball currently in the cannon 

    private SteamVR_TrackedObject trackedController;
    private bool controllerInCannonHandle = false; // looks to see if there's a controller in the cannon handle area or not

    // variables to keep track of the initial velocity and angle of the ball
    private float BASE_VELOCITY = 5f;
    private Vector3 shootDirection;

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
        //slingshotStartPosition = slingshotGameObject.transform.position; // records the position of the slingshot when the game first starts
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
        currentBall.transform.LookAt(endOfCannon);

        // changing how the slingshot behaves by making different inputs on the controller 
        if (trackedController != null) // checks to make sure we have a controller that has entered the cannon handle collider area
        {
            var device = SteamVR_Controller.Input((int)trackedController.index);
            if (controllerInCannonHandle)
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
                else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
                {
                    // *** REWRITE THIS CODE ***
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
        }
    }

    // tells us we have a controller that exited the cannon trigger collider area
    void OnTriggerExit(Collider other)
    {
        trackedController = other.GetComponent<SteamVR_TrackedObject>(); // looks to see if there's an object in our collider
        if (trackedController != null)
        {
            controllerInCannonHandle = false;
        }
    }
}
