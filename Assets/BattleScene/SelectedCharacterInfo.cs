using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCharacterInfo : MonoBehaviour
{

    public BattleManager bm;

    public GameObject display_Name;
    public GameObject display_Str;
    public GameObject display_Int;
    public GameObject display_Spd;
    public GameObject display_portrait;
    public GameObject display_Healthbar;

    // Use this for initialization
    void Awake () {

    }
	
	// Update is called once per frame
    void LateUpdate()
    {
        if (bm.selectedTargetToAttack != null)
        {
            //Display target info
            //Name
            display_Name.GetComponent<Text>().text = bm.selectedTargetToAttack.characterName;
            //Strength
            display_Str.GetComponentInChildren<Text>().text = bm.selectedTargetToAttack.strength+"";
            //Intelligence
            display_Int.GetComponentInChildren<Text>().text = bm.selectedTargetToAttack.intelligence + "";
            //Speed
            display_Spd.GetComponentInChildren<Text>().text = bm.selectedTargetToAttack.speed + "";
            //Portrait
            display_portrait.GetComponentInChildren<Image>().sprite = bm.selectedTargetToAttack.portrait;
            //Healthbar
        }
    }
}
