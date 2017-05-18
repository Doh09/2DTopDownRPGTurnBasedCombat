using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

	public Transform canvas;
	public Transform pauseMenu;
	public Transform soundsMenu;
	SaveGame saveGame;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Pause ();
		}
	}

	public void Pause ()
	{
		if (pauseMenu.gameObject.activeInHierarchy == false) {
			pauseMenu.gameObject.SetActive (true);
			soundsMenu.gameObject.SetActive (false);
		}

		if (canvas.gameObject.activeInHierarchy == false) {
			canvas.gameObject.SetActive (true);
			Time.timeScale = 0;
			saveGame = gameObject.GetComponent<SaveGame> ();
			saveGame.SaveGameSettings (false);
		} else {
			canvas.gameObject.SetActive (false);
			Time.timeScale = 1;
		}
	}

	public void Sounds (bool open)
	{
		if (open) {
			soundsMenu.gameObject.SetActive (true);
			pauseMenu.gameObject.SetActive (false);
		}
		if (!open) {
			soundsMenu.gameObject.SetActive (false);
			pauseMenu.gameObject.SetActive (true);
		}
	}
}
