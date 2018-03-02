using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour {

    [SerializeField]
    private Text projectileInfo = null;

    public Slingshot slingshot;

	// Use this for initialization
	void Start () {
        projectileInfo.text = "Velocity: " + slingshot.GetComponent<Slingshot>().getShootVelocity();
	}
	
}
