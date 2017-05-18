using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetIdleAnimation : MonoBehaviour
{

    public float maxOffset = 0.5f;
	// Use this for initialization
	void Start ()
	{
	    Animator animator = GetComponent<Animator>();
	    animator.recorderStartTime = Random.Range(0f, maxOffset);
	    //animation["Idle"].time = Random.Range(0.0, animation["Idle"].length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
