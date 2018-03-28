using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // to use the plugin text editor TextMeshPro (it renders text much nicer than default unity text)

public class Cannon : MonoBehaviour
{
    //////////////////// BALL AND CANNON VARIABLES //////////////////
    public GameObject ballPrefab; // the prefab of the ball spawned everytime
    public GameObject ballSpawnPoint; // the parent of the ball object where the ball will spawn
    public GameObject ballExitPoint; // the end of the cannon

    private GameObject currentBall; // the current ball in the slingshot
    private Rigidbody ballRigidbody; // the rigidbody of our current ball
    private TrailRenderer ballTrail; // the trail the ball leaves


    public GameObject cannonHandle;
    private Vector3 cannonHandleStartPosition; // the position the handle needs to return to after releasing it 
    public GameObject cannonPivot; // the point that acts as the pivot for the cannon
         
    private bool readyforReload = true; // true if there is no ball currently in the cannon 
    
    // variables to keep track of the initial velocity and angle of the ball
    private float BASE_VELOCITY = 30f;
    private Vector3 shootDirection;
    private float extraVelocity; // extra velocity to the ball based on how far the handle is pulled back

    //////////////////// CONTROLLER VARIABLES //////////////////

    private SteamVR_TrackedObject trackedController;
    private bool controllerInCannonHandle = false; // looks to see if there's a controller in the cannon handle area or not

    //////////////////// TUTORIAL VARIABLES //////////////////

    private bool firstTutorial;
    private bool secondTutorial;


    //////////////////// UI VARIABLES //////////////////

    public TMP_Text shootUI; // to display the initial velocity and angles
    
    // to display the current variables of the physics forumlas
    public TMP_Text xVelocity;
    public TMP_Text yVelocity;
    public TMP_Text initialVelocity;
    public TMP_Text initialAngle;
    public float velocityTimer;

    // add a timer that starts when the ball is released
    // velocityTimer = 0;


    // in fixedupdate:
    // velocityTimer += Time.deltatime;

    public TMP_Text messageField;   //text for tutorial
    public TMP_Text messageField2;  //text for 2nd tutorial

    // private variables for the UI display, use helper functions below to get the values
    private float shootAngle;
    private float shootVelocity;



    //////////////////// OUR FUNCTIONS //////////////////

    // Use this for initialization
    void Start()
    {
        // logging the original transforms of the cannon so we can snap back to these positions 
        cannonHandleStartPosition = cannonHandle.transform.position;


        setTextFormula();

        firstTutorial = true;
        secondTutorial = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // reloads the ball in the slingshot if there is not one already
        if (readyforReload)
        {
            currentBall = Instantiate(ballPrefab); //creates a ball
            ballRigidbody = currentBall.GetComponent<Rigidbody>(); // reference to the rigidbody of the current ball
            ballTrail = currentBall.GetComponent<TrailRenderer>(); // the trail the current ball leaves

            currentBall.transform.parent = ballSpawnPoint.transform; // parents the ball to the spawn point
            currentBall.transform.localPosition = Vector3.zero; // puts the position of the ball to the position of the slingshot (the parent) 
            readyforReload = false; // now there is already a ball in the slingshot so we turn the boolean false 
         
        }
        if (firstTutorial == true)
        {
            messageField.enabled = true;
            messageField2.enabled = false;
        }
        else if(secondTutorial == true)
        {
            messageField.enabled = false;
            messageField2.enabled = true;
        }
        else
        {
            messageField.enabled = false;
            messageField2.enabled = false;
        }

        //points the forward direction of the ball towards the cannon exit point
        currentBall.transform.LookAt(ballExitPoint.transform.position);


        // record how far the handle has been pulled back and square it
        float handleDistance = (cannonHandle.transform.position - cannonHandleStartPosition).magnitude;
        extraVelocity = Mathf.Pow(handleDistance * 10, 2f);

        //Debug.Log("Distance pulled: " + handleDistance + ", extraVelocity: " + extraVelocity);

        // changing how the slingshot behaves by making different inputs on the controller 
        if (trackedController != null) // checks to make sure we have a controller that has entered the cannon handle collider area
        {
            var device = SteamVR_Controller.Input((int)trackedController.index);
            if (controllerInCannonHandle)
            {
                
                shootDirection = (ballExitPoint.transform.position - ballSpawnPoint.transform.position); // the direction the ball flies out at
                shootVelocity = (BASE_VELOCITY * shootDirection.magnitude); // the velocity the ball will fly out at

                // shooting script
                // if we release the trigger, the slingshot will reset to its original position
                if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
                {

                    // resets the "trigger variables" so the ball can be reloaded etc
                    readyforReload = true;
                    controllerInCannonHandle = false;
                    ballTrail.enabled = true;
                    setTextFormula();

                    // Vector3 ballPosition = currentBall.transform.position; 
                    cannonHandle.transform.position = cannonHandleStartPosition; // handle is reset into start position
                    cannonPivot.transform.LookAt(cannonHandleStartPosition); // rotation of cannon is reset into start position
                    currentBall.transform.parent = null; // "frees" the ball from the cannon
                    
                    //puts the ball at the end of cannon and add the velocity to the ball to send it flying
                    currentBall.transform.position = ballExitPoint.transform.position;
                    ballRigidbody.useGravity = true;
                    ballRigidbody.velocity = shootDirection * (shootVelocity + extraVelocity);

                    if (secondTutorial == true)
                    {
                        firstTutorial = false;
                        secondTutorial = false;
                    }

                } 

                // if we press the controller trigger, the handle will follow our controller
                if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
                {
                    // *** Add code to make the cannon pivot and move ***
                    cannonHandle.transform.position = trackedController.transform.position;
                    cannonPivot.transform.LookAt(trackedController.transform.position); // the cannon orientation will now follow the controller
                    setTextDynamic();

                    if (firstTutorial == true)
                    {
                        firstTutorial = false;
                        secondTutorial = true;
                    }
            
                }
            }
        }

        SetShootUI();
        
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

    ////////////////// UI FUNCTIONS ///////////////// 

    void SetShootUI ()
    {
       // shootUI.text = "initial velocity: " + getShootVelocity() + " m/s" + "\n" + "initial angle: " + getShootAngle() + " °";
    } 

    // helper functions to get private variables for the UI display
    double getShootAngleRadians()
    {
        float myAngle = (Vector3.Angle(shootDirection, Vector3.ProjectOnPlane(shootDirection, Vector3.up))) * Mathf.Deg2Rad; // the angle parallel to the ground the ball flies out at
        return System.Math.Round(myAngle, 2);
    }

    double getShootAngleDegrees()
    {
        float myAngle = Vector3.Angle(shootDirection, Vector3.ProjectOnPlane(shootDirection, Vector3.up)); // the angle parallel to the ground the ball flies out at
        return System.Math.Round(myAngle, 2);
    }

    double getShootVelocity()
    {      
        return System.Math.Round(shootVelocity + extraVelocity, 2); // return the velocity rounded to 2 integers
    }

    float getHorizontalPosition()
    {
        // returns length of vector after it is projected onto the ground plane
        return (Vector3.ProjectOnPlane((currentBall.transform.position - ballExitPoint.transform.position), Vector3.up)).magnitude;
    }

    float getVerticalPosition()
    {
        return currentBall.transform.position.y;
    }

    void setTextFormula()
    {
        xVelocity.text = "<#FF0000>v<sub>x</sub></color> = <#FF00FF>v<sub>0</sub></color>cos<#FFFFFF>(angle*pi/180)</color>";
        yVelocity.text = "<#0000FF>v<sub>y</sub></color> = <#FF00FF>v<sub>0</sub></color>sin<#FFFFFF>(angle*pi/180)</color>";

        initialVelocity.text = "<#FF0000>v<sub>0</sub></color> =" + 0 + " m /s";
        initialAngle.text = "angle =" + 0 + " °";
    }
    //xDisplacement.text = "<#FF0000>x</color> = <#FF00FF>v<sub>0</sub></color>cos<#FFFFFF>θ</color>t";
    //yDisplacement.text = "<#0000FF>y</color> = <#FF00FF>v<sub>0</sub></color>sin<#FFFFFF>θ</color>t - ½gt²";


    void setTextDynamic()
    {
        xVelocity.text = "<#FF0000>v<sub>x</sub></color>" + "= <#FF00FF>" + getShootVelocity() + "</color>" + "*<#FFFFFF>" + Mathf.Cos((float)getShootAngleRadians()) + "</color>";
        yVelocity.text = "<#0000FF>v<sub>y</sub></color>" + "= <#FF00FF>" + getShootVelocity() + "</color>" + "*<#FFFFFF>" + Mathf.Sin((float)getShootAngleRadians()) + "</color>";
        initialVelocity.text = "<#FF0000>v<sub>0</sub></color> =" + getShootVelocity() + " m /s";
        initialAngle.text = "angle =" + getShootAngleDegrees() + " °";

    //xDisplacement.text = "<#FF0000>" + "x" + "</color> = <#FF00FF>" + getShootVelocity() + "</color>cos" + "<#FFFFFF>" + getShootAngle() + "</color>" + "t";
    //yDisplacement.text = "<#0000FF>" + "y" + "</color> = <#FF00FF>" + getShootVelocity() + "</color>sin" + "<#FFFFFF>" + getShootAngle() + "</color>" + "t - ½gt²";
    }

    double getRigidbodyInitialVelocity()
    {
        return System.Math.Round(ballRigidbody.velocity.magnitude, 2); // return the velocity rounded to 2 integers
    }

    double getHorizontalVelocity()
    {
        return System.Math.Round(ballRigidbody.velocity.x, 2); // return the velocity rounded to 2 integers
    }

    double getVerticalVelocity()
    {
        return System.Math.Round(ballRigidbody.velocity.y, 2); // return the velocity rounded to 2 integers
    }

    double getTimeInSeconds()
    {
        return System.Math.Round(velocityTimer, 2); // return the time rounded to 2 integers
    }

    
}
