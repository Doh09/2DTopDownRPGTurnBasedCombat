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
    public GameObject display_Portrait;
    public GameObject display_Healthbar;

    // Use this for initialization
    void Awake () {

    }
	
	// Update is called once per frame
    void LateUpdate()
    {
        if (bm != null)
        {
            if (bm.selectedTargetToAttack != null)
            {
                //Display target info
                //Name
                if (display_Name == null)
                    display_Name = GameObject.Find("display_Name");
                display_Name.GetComponent<Text>().text = bm.selectedTargetToAttack.characterName;
                //Strength
                if (display_Str == null)
                    display_Str = GameObject.Find("display_Str");
                display_Str.GetComponentInChildren<Text>().text = bm.selectedTargetToAttack.strength + "";
                //Intelligence
                if (display_Int == null)
                    display_Int = GameObject.Find("display_Int");
                display_Int.GetComponentInChildren<Text>().text = bm.selectedTargetToAttack.intelligence + "";
                //Speed
                if (display_Spd == null)
                    display_Spd = GameObject.Find("display_Spd");
                display_Spd.GetComponentInChildren<Text>().text = bm.selectedTargetToAttack.speed + "";
                //Portrait
                if (display_Portrait == null)
                    display_Portrait = GameObject.Find("display_Portrait");
                display_Portrait.GetComponentInChildren<Image>().sprite = bm.selectedTargetToAttack.portrait;
                //Healthbar
            }
        }
    }
}
