using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonWaterScript : MonoBehaviour {
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(0.05f, 0.05f);
    public string textureName = "ToonWater";    
    public Renderer myRenderer;

    Vector2 uvOffset = Vector2.zero;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        if (myRenderer.enabled)
        {
            myRenderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
    }
}
