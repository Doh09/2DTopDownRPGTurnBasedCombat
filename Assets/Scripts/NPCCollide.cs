using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCCollide : MonoBehaviour
{

    private static GameManager gameManager;
    private List<Transform> _characterObjects;
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
                transform.GetChild(i).gameObject.SetActive(true);
                Debug.Log(i);
            }
            //DontDestroyOnLoad(gameObject.transform);    
            //DontDestroyOnLoad(col.transform.parent.gameObject);
            
            //_characterObjects = new List<Transform>(col.gameObject.GetComponentsInChildren<Transform>());
            //_characterObjects.Add(transform.GetComponent<Transform>());
            //gameObject.SetActive(false);
            //col.transform.parent.gameObject.SetActive(false);
            //col.gameObject.SetActive(false);
            //gameManager.ChangeToBattleScene(_characterObjects);
        }
    }


}
