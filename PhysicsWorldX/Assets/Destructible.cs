using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Destructible : MonoBehaviour {

    public GameObject destroyedVersion; // if we want to make a "shattered" version of the item
    public GameObject cannonBallInitialPoint;
    
    
    //////////////////// UI VARIABLES //////////////////

    public TMP_Text distanceUI; // to display how far away our item is from the cannon
    private float xDistance;
    private float yDistance;

    //////////////////// START OF FUNCTIONS //////////////////

    private void FixedUpdate()
    {
        xDistance = Vector3.Magnitude(Vector3.ProjectOnPlane(transform.position - cannonBallInitialPoint.transform.position, Vector3.up)); // the length of the distance vector after it is projected onto the ground plane
        yDistance = (transform.position.y - cannonBallInitialPoint.transform.position.y);

        SetUI();
    }

    public void Destroy()
    {
        // create a shattered version 
        //Instantiate(destroyedVersion, transform.position, transform.rotation);

        // destroy our game object
        Destroy(gameObject);
    }

    // records when something is in our collider
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Our " + gameObject.name + " has been hit by a object of type" + col.gameObject.tag);
        if (col.gameObject.tag == "CannonBall") // if the other object in our collider is a cannonball
        {
            // make cannonball explode
            col.gameObject.GetComponent<CannonBall>().Explode();

            // destroy ourself
            Destroy();

            Debug.Log(gameObject.name + "has been exploded!");
        }
        
    }

    //////////////////// UI FUNCTIONS //////////////////

    void SetUI()
    {
        distanceUI.text = "horizontal distance: " + getxDistance() + "m" + "\n" + 
            "vertical distance: " + getyDistance() + "m";
    }

    double getxDistance()
    {
        return System.Math.Round(xDistance, 2); // return the horizontal distance rounded to 2 integers
    }

    double getyDistance()
    {
        return System.Math.Round(yDistance, 2); // return the horizontal distance rounded to 2 integers
    }

}
