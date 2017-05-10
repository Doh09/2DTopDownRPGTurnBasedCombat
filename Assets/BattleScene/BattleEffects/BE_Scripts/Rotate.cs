using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float angle = 1f;
    public bool reverseRotation = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!reverseRotation)
		transform.Rotate(Vector3.forward, angle);
        else
        {
            transform.Rotate(Vector3.back, angle);
        }
	}
}
