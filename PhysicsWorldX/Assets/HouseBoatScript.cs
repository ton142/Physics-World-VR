using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBoatScript : MonoBehaviour
{

    public float speed = 0.02f;
    private Transform myTransform;
    private Vector3 myForward;


    // Use this for initialization
    void Start()
    {
        myTransform = GetComponent<Transform>();
        myForward = -(transform.right);
    }

    void FixedUpdate()
    {
        myTransform.position += (myForward * speed);
        
    }
}
