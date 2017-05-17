using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Warp : MonoBehaviour {

	public Transform player;
	public string scene;
	public float posX;
	public float posY;
	//public float posZ;

	void OnTriggerEnter2D(Collider2D other){
        SceneManager.LoadScene (scene);
		PlayerPrefs.SetFloat ("x", posX);
		PlayerPrefs.SetFloat ("y", posY);
		PlayerPrefs.SetFloat ("z", 0);
	}
}
