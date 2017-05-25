using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("is triigger");
        GameManager.instance.ChangeToNewScene("WinningScene");
    }
}
