using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCCollide : MonoBehaviour
{

    private static GameManager gameManager;
    private List<Transform> _characterTransforms;
    private GameObject enemy;
    

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
       
        if (!col.CompareTag("Enemy"))
        {
            return;
        }
        else
        {

            for (int i = 0; i < col.transform.childCount; i++)
            {
                col.transform.GetChild(i).gameObject.SetActive(true);
            }
            DontDestroyOnLoad(gameObject.transform);    
            DontDestroyOnLoad(col.transform.parent.gameObject);
            
            _characterTransforms = new List<Transform>(col.gameObject.GetComponentsInChildren<Transform>());
            _characterTransforms.Add(transform.GetComponent<Transform>());

            GameManager.instance.StoreFightersInGameManager(_characterTransforms);


            col.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);

            GameManager.instance.ChangeToNewScene("BattleScene");

        }
    }


}
