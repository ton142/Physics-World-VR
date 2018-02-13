using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour {

    // the plane should take off (rise) and then stay at steady velocity

    public float groundSpeed = 5f;
    public float speed = 10f;
    public float angleTakeoff = 45f;
    public float timeforTakeoff = 3f;
       

    private bool liftingOff;
    private float timer;
    private Rigidbody myPlane;
 

	// Use this for initialization
	void Start () {
        // reset beginning transform to the ground
        transform.position = new Vector3(0, 1.5f, 0);
        myPlane = GetComponent<Rigidbody>();
        
	}
	
	void FixedUpdate () {
        timer += Time.deltaTime;
        myPlane.velocity = -(transform.right * groundSpeed);

        if (timer > timeforTakeoff && transform.position.y < 20f)
        {
            Debug.Log("Takeoff time!");
            myPlane.velocity = (transform.up * speed);
            //Takeoff();
        }
        
    }

    void Takeoff ()
    {
            myPlane.velocity = (transform.up * speed);
    }
}
