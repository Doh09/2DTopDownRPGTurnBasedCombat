using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFindsPlayer : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    transform.parent = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
