using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Warp : MonoBehaviour {

	public Transform player;
	public string scene;
	public float posX;
	public float posY;
    public Vector3 offSet;
	//public float posZ;

    void OnEnable()
    {
        player = GameManager.instance.GetPlayer().transform;
    }

    void OnTriggerEnter2D(Collider2D other){
        DontDestroyOnLoad(player);
        player.position = new Vector3(posX, posY, 0) + offSet;
        SceneManager.LoadScene (scene);
		PlayerPrefs.SetFloat ("x", posX);
		PlayerPrefs.SetFloat ("y", posY);
		PlayerPrefs.SetFloat ("z", 0);
	}
}
