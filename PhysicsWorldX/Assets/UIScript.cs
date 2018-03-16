using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScript : MonoBehaviour {

    private TMP_Text myText;

    private float shootVelocity;
    private float shootAngle;
    private float xDistance;
    private float yDistance;

	// Use this for initialization
	void Start () {
        myText = gameObject.GetComponent<TMP_Text>();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
