﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    private Camera camera;
    private bool sceneChangeInitialized = false;
    private SimpleBlit simpleBlit;
    private GameObject player;
    public string previousSceneName;

    private List<Transform> _charactersTransforms;

    public string sceneToLoadTo = "BattleScene";


    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
        {
            SceneManager.sceneLoaded += onSceneLoaded;
            instance = this;
        }
        else if (instance != null)
        {
            if (instance != this)
                Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
        GetPlayer();
    }

    public GameObject GetPlayer()
    {
        if (player == null)
            player = GameObject.Find("Player");
        return player;
    }

    // Update is called once per frame
    void Update()
    {

        if (sceneChangeInitialized && simpleBlit.currentValue >= 1f)
        {
            previousSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneToLoadTo);
            sceneChangeInitialized = false;

        }
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!SceneManager.GetActiveScene().Equals("BattleScene"))
        {
            if (player != null)
            player.SetActive(true);
        }
    }

    public void StoreFightersInGameManager(List<Transform> characterTransforms)
    {
        _charactersTransforms = characterTransforms;
        foreach (var ct in _charactersTransforms)
        {
            Debug.Log(ct.gameObject);
        }
    }


    public void ChangeToNewScene(string sceneToChangeTo = "BattleScene")
    {
        sceneToLoadTo = sceneToChangeTo;
        if (camera == null)
            camera = Camera.main;
        if (camera == null)
            camera = Camera.current;

        simpleBlit = camera.GetComponent<SimpleBlit>();
        simpleBlit.Activated = true;
        sceneChangeInitialized = true;
        Debug.Log("LoadMe");
        camera = null;
    }

    public List<Transform> GetCharactersTransforms()
    {
        return _charactersTransforms;
    }

}
