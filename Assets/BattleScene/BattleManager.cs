using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class has the responsibility of handling all battle interactions between characters in a battle.
/// It will handle displaying the battle effects, know which is the current turn taker and the currently selected target.
/// Any battle effect activated is the responsibility of this class.
/// The custom information about the battle effect is specified in the turn taker and/or target, for example damage taken.
/// </summary>
public class BattleManager : MonoBehaviour
{
    [Header("Floating text")]
    public GameObject FloatingTxtCanvas;
    public float FloatingTxtFadeTime = 2f;
    public Color FriendlyTxtColor;
    public Color HostileTxtColor;
    public Color NeutralTxtColor;

    [Header("Target handling")]
    public CharacterScript selectedTargetToAttack;

    [Header("Turn handling")]
    public CharacterScript currentTurnTaker;
    private int currentTurnTakerIndex = 1;
    public GameObject PanelToShowTurns;
    //public Queue<CharacterScript> WhosTurnIsIt = new Queue<CharacterScript>();
    //public Queue<GameObject> WhosTurnIsItPortraits = new Queue<GameObject>();
    public List<CharacterScript> AllFighters = new List<CharacterScript>();
    [HideInInspector]
    public List<CharacterScript> AllFriendlies = new List<CharacterScript>();
    [HideInInspector]
    public List<CharacterScript> AllEnemies = new List<CharacterScript>();

    [Header("Abilities")]
    public Ability CurrentAbility;
    public Dropdown abilityDropDownMenu;
    private AbilityCoolDown abilityCoolDown;

    void Awake()
    {
        abilityCoolDown = GetComponent<AbilityCoolDown>();
    }

    // Use this for initialization
    void Start()
    {
        foreach (var fighter in AllFighters) //Sort fighters into friendlies and enemies respectively.
        {
            if (fighter.hostility == CharacterScript.HostilityToPlayer.Enemy)
            {
                AllEnemies.Add(fighter);
            }
            else
            {
                AllFriendlies.Add(fighter);
            }
        }
        currentTurnTaker = AllFighters[currentTurnTakerIndex];
        PanelToShowTurns.GetComponent<Image>().sprite = currentTurnTaker.portrait;
        setAbility();
    }

    public delegate void MakeDamageText(int damage, CharacterScript turnTaker, CharacterScript targetToAttack);

    public MakeDamageText dmgCallback;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1)) //Test activate next turn
        {
            TriggerCurrentAbility();
        }
        if (Input.GetMouseButtonDown(0)) //Test activate next turn
        {
            SelectTarget();
        }

    }

    private void SelectTarget()
    {
        //Debug.Log("Mouse 0 is down");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.transform.CompareTag("BattleParticipant"))
        {//If target has a collider and is a battle participant, note that battle participants MUST have a CharacterScript.
            selectedTargetToAttack = hit.collider.gameObject.GetComponent<CharacterScript>();
            Debug.Log("Selected target: " + selectedTargetToAttack.characterName);
        }
        else
        {
            Debug.Log("No hit/target found");

        }
    }

    public void setAbility()
    {
        int abilityNumber = abilityDropDownMenu.value;
        CurrentAbility = currentTurnTaker.abilities[abilityNumber];
        abilityCoolDown.ability = CurrentAbility;
    }


    public void DamageTarget(int damage, CharacterScript turnTaker, CharacterScript targetToAttack)
    {
        Debug.Log("Button was clicked");
        if (targetToAttack != null)
        {
            if (targetToAttack.hostility == turnTaker.hostility)
            {
                MakeFloatingTextAboveTarget(targetToAttack.transform, "Cannot attack an ally!", FriendlyTxtColor);
                Debug.Log("Cannot attack an ally!");
                return;
            }
            Debug.Log("Target hp before attack: " + targetToAttack.hp);
            // int damage = currentTurnTaker.damage;
            targetToAttack.hp -= damage;
            MakeFloatingTextAboveTarget(targetToAttack.transform, damage + " damage taken!", HostileTxtColor);
            Debug.Log("Target hp after attack: " + targetToAttack.hp);
            NextTurn();
        }
        else
        {
            MakeFloatingTextAboveTarget(Camera.main.transform, "No target selected", NeutralTxtColor);
            Debug.Log("No target selected");
        }
    }

    public void TriggerCurrentAbility()
    {
        if (checkBattleSelections())
        {
            dmgCallback = new MakeDamageText(DamageTarget); //Use a delegate to let the ability make a callback to the damage method in BattleManager.
            if (abilityCoolDown.CooldownIsComplete)
                abilityCoolDown.FireAbility(currentTurnTaker.transform, selectedTargetToAttack.transform, dmgCallback);
        }
    }

    private bool checkBattleSelections()
    {
        if (currentTurnTaker == null)
        {
            MakeFloatingTextAboveTarget(currentTurnTaker.transform, "No turnTaker found", NeutralTxtColor);
            return false;
        }
        if (selectedTargetToAttack == null)
        {
            MakeFloatingTextAboveTarget(currentTurnTaker.transform, "No target selected", NeutralTxtColor);
            return false;
        }
        if (CurrentAbility == null)
        {
            MakeFloatingTextAboveTarget(currentTurnTaker.transform, "No ability selected", NeutralTxtColor);
            return false;
        }
        return true;
    }

    void MakeFloatingTextAboveTarget(Transform target, string floatingText, Color txtColor)
    {
        //Create text canvas
        GameObject flTxtCanvas = Instantiate(FloatingTxtCanvas, target);
        //Set position
        flTxtCanvas.transform.position = new Vector3(target.position.x, target.position.y, flTxtCanvas.transform.position.z);
        //Get text from child object of the canvas and set it
        Text flText = flTxtCanvas.GetComponentInChildren<Text>();
        flText.text = floatingText;
        flText.color = txtColor;
        StartCoroutine(DestroyObjectAfterTime(flTxtCanvas, FloatingTxtFadeTime));
    }

    private IEnumerator DestroyObjectAfterTime(GameObject toDestroy, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // Now do your thing here
        Destroy(toDestroy);
    }

    void NextTurn()
    {
        currentTurnTaker = AllFighters[currentTurnTakerIndex];
        currentTurnTakerIndex++;
        currentTurnTakerIndex = currentTurnTakerIndex % AllFighters.Count;
        Image i = PanelToShowTurns.GetComponent<Image>();
        i.sprite = currentTurnTaker.portrait;
        //panel.transform.SetParent(PanelToShowTurns.transform, false);
        i.color = Color.yellow;
        Debug.Log("currentTurnTakerIndex: " + currentTurnTakerIndex);
        Debug.Log("currentTurnTaker: " + currentTurnTaker.characterName);
        abilityDropDownMenu.value = 0;
        setAbility();
        //AddSpeedToTurns();
        //WhosTurnIsIt.Dequeue(); //remove a fighter from queue
        //WhosTurnIsItPortraits.Dequeue(); //remove a portrait from queue
    }

    float getSpeedValue(CharacterScript fighter)
    {
        return fighter.speed / AllFighters.Count; //Fighter speed divided by amount of fighters
    }
}
