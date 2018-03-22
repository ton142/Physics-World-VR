using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {

    public GameObject explosionEffect;
    public float explosionRadius = 5f;
    public float explosionForce = 50f; // the magnitude of the force the explosion generates on the hit items

    private bool hasExploded; //boolean to let us know when to call the Explode function

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (hasExploded) {
            Explode();
            hasExploded = true;
        }
	}

    void Explode()
    {
        Debug.Log("Cannon exploded");

        // show explosion effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Find objects that are hit by creating a list of objects with colliders touching a sphere of the specified radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        // Add force/damage to all objects hit by the explosion
        foreach (Collider hitObject in colliders)
        {
            Destructible dest = hitObject.GetComponent<Destructible>();
            if (dest != null)
            {
                dest.Destroy();
            }

            Rigidbody rb = hitObject.GetComponent<Rigidbody>();
            if (rb != null) // need to check that the object actually has a rigidbody attached to it
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }


        }


        // destroy our current gameObject, which is the current cannonball
        Destroy(gameObject);
    }
}
