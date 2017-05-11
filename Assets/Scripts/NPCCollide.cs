using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCCollide : MonoBehaviour
{

    private static GameManager gameManager;
    private List<CharacterScript> _characterScripts;
    

    // Use this for initialization
    void Start ()
    {

        if (gameManager == null)
      gameManager = GameManager.instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("trigger");
        if (!col.CompareTag("Enemy"))
        {
            return;
        }
        else
        {
            Debug.Log("true");
            _characterScripts = new List<CharacterScript>(col.gameObject.GetComponentsInChildren<CharacterScript>());
            _characterScripts.Add(transform.GetComponent<CharacterScript>());
            gameManager.ChangeToBattleScene(_characterScripts);
        }
    }


}
