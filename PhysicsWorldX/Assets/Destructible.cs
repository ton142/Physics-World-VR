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

    //////////////////// OUR FUNCTIONS //////////////////

    private void FixedUpdate()
    {
        xDistance = (transform.position.x - cannonBallInitialPoint.transform.position.x);
        yDistance = (transform.position.y - cannonBallInitialPoint.transform.position.y);
    }

    public void Destroy()
    {
        // create a shattered version 
        Instantiate(destroyedVersion, transform.position, transform.rotation);

        // destroy our game object
        Destroy(gameObject);
    }

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
