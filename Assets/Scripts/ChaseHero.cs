using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseHero : MonoBehaviour
{
    public float speed;

    private Vector3 Player;
    private Vector2 PlayerDirection;
    private Rigidbody2D rb;
    private float xdif;
    private float ydif;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Player = GameObject.Find("Player").transform.position;

	    xdif = Player.x - transform.position.x;
	    ydif = Player.y - transform.position.y;

        PlayerDirection = new Vector2(xdif, ydif);

       // rb.AddForce(PlayerDirection.normalized * speed);
        transform.Translate(PlayerDirection.normalized * speed);

	}
}
