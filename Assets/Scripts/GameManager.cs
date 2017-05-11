using System.Collections;
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
    private List<CharacterScript> _charactersStats;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
        {
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
	void Start ()
	{
	    camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        if (sceneChangeInitialized && simpleBlit.currentValue >= 1f)
        {
            SceneManager.LoadScene("BattleScene");
            sceneChangeInitialized = false;
        }
    }

    public void ChangeToBattleScene(List<CharacterScript> cStats)
    {
        _charactersStats = cStats;
        simpleBlit = camera.GetComponent<SimpleBlit>();
        simpleBlit.Activated = true;
        sceneChangeInitialized = true;
        Debug.Log("LoadMe");
    }

    public List<CharacterScript> GetCharactersScripts()
    {
        
        return _charactersStats;    
    }

}
