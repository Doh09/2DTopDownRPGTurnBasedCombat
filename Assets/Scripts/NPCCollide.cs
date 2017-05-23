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
    void Start()
    {

        if (gameManager == null)
            gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (!col.CompareTag("Enemy"))
        {
            return;
        }

        for (int i = 0; i < col.transform.childCount; i++)
        {
            col.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        DontDestroyOnLoad(gameObject.transform);
        DontDestroyOnLoad(col.transform.parent.gameObject);

        _characterTransforms = new List<Transform>();
        _characterTransforms.Add(transform); //Add player parent
        var playerChildren = gameObject.GetComponentsInChildren<Transform>();
        _characterTransforms.AddRange(playerChildren);
        var enemyChildren = col.gameObject.GetComponentsInChildren<Transform>();
        _characterTransforms.AddRange(enemyChildren);

        GameManager.instance.StoreFightersInGameManager(_characterTransforms);

        GameManager.instance.ChangeToNewScene("BattleScene");

    }


}
