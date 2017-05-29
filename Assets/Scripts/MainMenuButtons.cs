using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {

	public Button startGame;
	//public Button exitGame;

	// Use this for initialization
	void Start () {
		startGame = startGame.GetComponent<Button> ();
		//exitGame = exitGame.GetComponent<exitGame> ();
	}
	
	public void startLevel(){
        if (PlayerPrefs.HasKey("map"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("map")); //Should be changed to use GameManagers scene load also.

        }
        else
        {
            Debug.Log("Play was clicked");
            GameObject.Find("Player").transform.position = new Vector3(0,0,0);
		    GameManager.instance.ChangeToNewScene("Main");
		}
	}

}
