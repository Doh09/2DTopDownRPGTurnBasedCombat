using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExtend : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CallGameManagerToChangeToMenu()
    {
        GameManager.instance.ChangeToNewScene("Menu");
    }
}
