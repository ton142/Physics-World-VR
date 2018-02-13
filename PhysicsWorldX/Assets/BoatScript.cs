using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour
{

    public float speed = 0.02f;
    private Transform myTransform;


    // Use this for initialization
    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        myTransform.position += (myTransform.forward * speed);
        
    }
}
