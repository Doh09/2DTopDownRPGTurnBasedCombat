using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCCollide : MonoBehaviour
{

    private static GameManager gameManager;
    private CharacterScript[] _characterScripts;
    

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
            _characterScripts = col.gameObject.GetComponentsInChildren<CharacterScript>();

            var player = transform.GetComponent<CharacterScript>();
            Debug.Log("Player -- " + "Armor: " + player.armorType.ToString()  +" SkinType: " + player.skinType.ToString());
            testmethod();
            GameManager.instance.ChangeToNewScene("BattleScene");
        }
    }

    void testmethod()
    {
        foreach (var bitches in _characterScripts)
        {
            Debug.Log("Enemy Name: " + bitches.characterName);
        }
    }

}
