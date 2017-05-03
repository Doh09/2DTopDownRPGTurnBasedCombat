using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseHero : MonoBehaviour
{
    public float speed;
    private Vector3 PlayerPosition;
    private Vector2 PlayerDirection;
    private float xdif;
    private float ydif;
    private bool chase = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (chase)
	    {
	        ChasePlayer();
	    }
	    chase = false;
	}

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("TRIIIIIIGGGERED!");
        if (!other.CompareTag("Player"))
        { 
            return;
            Debug.Log("return");
        }
        else
        {
            Debug.Log("true");
            chase = true;
        }
    }

    void ChasePlayer()
    {
        PlayerPosition = GameObject.Find("Player").transform.position;

        xdif = PlayerPosition.x - transform.position.x;
        ydif = PlayerPosition.y - transform.position.y;

        PlayerDirection = new Vector2(xdif, ydif);

        transform.Translate(PlayerDirection.normalized * speed);
    }
}
