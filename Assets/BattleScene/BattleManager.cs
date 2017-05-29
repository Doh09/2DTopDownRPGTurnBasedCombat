using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    public GameObject pointerHand;
    public Vector3 pointerHandOffset;

    [Header("Turn handling")]
    public bool PlayerTurn = false;
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
    [HideInInspector]
    public List<CharacterScript> DeadFighters = new List<CharacterScript>();
    public CanvasGroup ActionMenu;
    public bool enemyTurnInitialized = false;
    public TextMeshProUGUI bigInfoText;

    [Header("Abilities")]
    public Ability CurrentAbility;
    public Dropdown abilityDropDownMenu;
    private AbilityCoolDown abilityCoolDown;
    [HideInInspector]
    public delegate void MakeDamageText(int damage, CharacterScript turnTaker, CharacterScript targetToAttack);
    public MakeDamageText dmgCallback;

    public List<CharacterScript> _CharacterScriptsFromMap;
    private Transform enemyParent;
    private Transform playerParent;

    private GameObject[] friendlyBattlePositions;
    private GameObject[] enemyBattlePositions;
    private int friendlybpIndex = 0;
    private int enemybpIndex = 0;

    void Awake()
    {
        friendlyBattlePositions = GameObject.FindGameObjectsWithTag("FriendlyBattlePosition");
        enemyBattlePositions = GameObject.FindGameObjectsWithTag("EnemyBattlePosition");
        if (bigInfoText == null)
            GameObject.Find("bigInfoText").GetComponent<TextMeshProUGUI>();
        bigInfoText.text = "";
        //Load in all fighters
        var fighterTransforms = GameManager.instance.GetCharactersTransforms();
        Debug.Log("battleManager");

        foreach (var ft in fighterTransforms)
        {
            if (ft.tag == "BattleParticipant")
            {
                AllFighters.Add(ft.GetComponent<CharacterScript>());
                ft.gameObject.SetActive(true);
                ft.parent = null;
                if (ft.GetComponent<CharacterScript>().hostility == CharacterScript.HostilityToPlayer.Friendly)
                {
                    //Give friendly battle position
                    ft.position = friendlyBattlePositions[friendlybpIndex].transform.position;
                    ft.localRotation = Quaternion.Euler(0, 180, 0); //turn right
                    friendlybpIndex++;
                    friendlybpIndex = friendlybpIndex % friendlyBattlePositions.Length;
                }
                else if (ft.GetComponent<CharacterScript>().hostility == CharacterScript.HostilityToPlayer.Enemy)
                {
                    //Give enemy battle position
                    ft.position = enemyBattlePositions[enemybpIndex].transform.position;
                    ft.localRotation = Quaternion.Euler(0, 0, 0); //turn left
                    enemybpIndex++;
                    enemybpIndex = enemybpIndex % enemyBattlePositions.Length;
                }
            }
            else if (ft.CompareTag("Enemy"))
            {
                enemyParent = ft;
                
            }
            else if (ft.CompareTag("Player"))
            {
                playerParent = ft;
            }
        }
        //deactivate parent objects during combat.
        enemyParent.gameObject.SetActive(false);
        playerParent.gameObject.SetActive(false);
        //Sort fighters into friendlies and enemies respectively, for AI purposes.
        foreach (var fighter in AllFighters) 
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
        currentTurnTaker = AllFighters[0];
        PanelToShowTurns.GetComponent<Image>().sprite = currentTurnTaker.portrait;
        if (pointerHand == null)
            pointerHand = GameObject.FindGameObjectWithTag("PointerHand");
        dmgCallback = new MakeDamageText(DamageTarget); //Use a delegate to let the ability make a callback to the damage method in BattleManager.

   //     var gameManager = GameObject.Find("GameManager");
    //    var gameManagerScript = gameManager.GetComponent<GameManager>();


        
        //foreach (var battleParticipant in gameManagerScript.GetCharactersTransforms())
        //{
        //    GameObject character = Instantiate(battleParticipant.gameObject, transform, true);
        //    character.transform.parent = transform;
        //    AllFighters.Add(battleParticipant.GetComponent<CharacterScript>());
        //}
        // AllFighters = ;

    }

    // Use this for initialization
    void Start()
    {
        SetupAbilitiesDropDownMenu();
        refreshDropDown();
        setAbility();
        checkPlayerTurn();
        pointerHand.transform.position = currentTurnTaker.transform.position + pointerHandOffset;
    }

    private void refreshDropDown(int newValue = 0)
    {
        if (abilityCoolDown == null)
            abilityCoolDown = GameObject.Find("AbilityDropDownMenu").GetComponent<AbilityCoolDown>();
        abilityCoolDown = GetComponent<AbilityCoolDown>();
        abilityDropDownMenu.value = newValue;
        abilityDropDownMenu.RefreshShownValue();
    }

    public void setAbility()
    {
        int abilityNumber = abilityDropDownMenu.value;
        CurrentAbility = currentTurnTaker.abilities[abilityNumber];
        abilityCoolDown.ability = CurrentAbility;
    }
    
    // Update is called once per frame
    void Update()
    {
        pointerHand.transform.position = currentTurnTaker.transform.position + pointerHandOffset;
        ActionMenu.interactable = PlayerTurn;
        if (Input.GetMouseButtonDown(0)) //Test activate next turn
        {
            SelectTarget();
        }
        if (!PlayerTurn && !enemyTurnInitialized && abilityCoolDown.CooldownIsComplete)
            takeEnemyTurn();
    }

    private void takeEnemyTurn()
    {
        if (AllFriendlies.Count == 0)
        {
            Debug.Log("No players detected");
            return;
        }
        //Initialize enemy turn
        enemyTurnInitialized = true;
        //Select ability
        int abilityInt = Random.Range(0, currentTurnTaker.abilities.Count);
        CurrentAbility = currentTurnTaker.abilities[abilityInt];
        //Find target
        if (CurrentAbility.canTarget.Contains(CharacterScript.HostilityToPlayer.Friendly) &&
            CurrentAbility.canTarget.Contains(CharacterScript.HostilityToPlayer.Enemy))
        {
            //Find a random target among battleparticipants
            int targetInt = Random.Range(0, AllFighters.Count);
            selectedTargetToAttack = AllFighters[targetInt];
        }
        else if (CurrentAbility.canTarget.Contains(CharacterScript.HostilityToPlayer.Friendly))
        {
            //Find an NPC as target
            int targetInt = Random.Range(0, AllEnemies.Count);
            selectedTargetToAttack = AllEnemies[targetInt];
        }
        else if (CurrentAbility.canTarget.Contains(CharacterScript.HostilityToPlayer.Enemy))
        {
            //Find a player as target
            int targetInt = Random.Range(0, AllFriendlies.Count);
            selectedTargetToAttack = AllFriendlies[targetInt];
        }
        //Trigger ability
        TriggerCurrentAbility();
    }

    private void checkPlayerTurn()
    {
        //Player turn if a friendly characters turn.
        PlayerTurn = currentTurnTaker.hostility == CharacterScript.HostilityToPlayer.Friendly;
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

    public void DamageTarget(int damage, CharacterScript turnTaker, CharacterScript targetToAttack)
    {      
        Debug.Log("Button was clicked");
        if (targetToAttack != null)
        {
            Debug.Log("Target hp before attack: " + targetToAttack.hp);
            // int damage = currentTurnTaker.damage;
            targetToAttack.hp -= damage;
            if (damage >= 0)
            //if damage taken
            MakeFloatingTextAboveTarget(targetToAttack.transform, damage + " health lost!", HostileTxtColor);
            else
            { //if healed
            MakeFloatingTextAboveTarget(targetToAttack.transform, (Mathf.Abs(damage)) + " health healed!", FriendlyTxtColor);
            }
            if (targetToAttack.hp <= 0f)
            {
                //Target died
                SetPlayerDead(targetToAttack);
            }
            Debug.Log("Target hp after attack: " + targetToAttack.hp);
            NextTurn();
        }
        else
        {
            MakeFloatingTextAboveTarget(currentTurnTaker.transform, "No target selected", NeutralTxtColor);
            Debug.Log("No target selected");
        }
    }

    public void TriggerCurrentAbility()
    {
        if (checkBattleSelections())
        {
            Animator anim = null;
            if (currentTurnTaker.GetComponent<Animator>() != null)
            {
                anim = currentTurnTaker.GetComponent<Animator>();
            }
            if (anim != null)
                anim.SetTrigger("UseAbility");
            if (!CurrentAbility.canTarget.Contains(selectedTargetToAttack.hostility) && PlayerTurn) //only do check on playerturn, as enemy turn has check when selecting target.
            {
                MakeFloatingTextAboveTarget(selectedTargetToAttack.transform, "Invalid target!", NeutralTxtColor);
                return;
            }
            abilityCoolDown.ability = CurrentAbility;
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
        Destroy(flTxtCanvas, FloatingTxtFadeTime);
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
        //Set abilities in menu
        SetupAbilitiesDropDownMenu();
        setAbility();
        checkPlayerTurn();
        enemyTurnInitialized = false;
        //AddSpeedToTurns();
        //WhosTurnIsIt.Dequeue(); //remove a fighter from queue
        //WhosTurnIsItPortraits.Dequeue(); //remove a portrait from queue
    }

    void SetPlayerDead(CharacterScript fighter)
    {
        fighter.GetComponent<Animator>().SetTrigger("die");
        if (fighter.hostility == CharacterScript.HostilityToPlayer.Friendly)
        {
            //Remove from friendly queue
            AllFriendlies.RemoveAll(x => x.GetComponent<CharacterScript>().Equals(fighter));
        }
        else if (fighter.hostility == CharacterScript.HostilityToPlayer.Enemy)
        {
            //Remove from enemy queue
            AllEnemies.RemoveAll(x => x.GetComponent<CharacterScript>().Equals(fighter));
        }
        AllFighters.RemoveAll(x => x.GetComponent<CharacterScript>().Equals(fighter)); //Remove from combat
        //Add to queue of dead guys
        DeadFighters.Add(fighter);
        //If allEnemies.length or allFriendlies.length == 0
        if (AllEnemies.Count == 0)
        {
            //Win
            Debug.Log("You won, well done!");
            bigInfoText.gameObject.SetActive(true);
            bigInfoText.text = "You won, well done!";
            StartCoroutine(changeScene(GameManager.instance.previousSceneName, 3));
        }
        else if (AllFriendlies.Count == 0)
        {
            //Lose
            Debug.Log("You lost, better luck next time!");
            bigInfoText.gameObject.SetActive(true);
            bigInfoText.text = "You lost, better luck next time!";
            StartCoroutine("Menu", 3);
        } 
    }

    IEnumerator changeScene(string sceneToChangeTo, int waitForSeconds = 3)
    {
        yield return new WaitForSeconds(waitForSeconds);
        foreach (var characterScript in AllFriendlies)
        {
            if (characterScript.transform.CompareTag("BattleParticipant"))
                characterScript.transform.parent = playerParent; //reattach players to their map parent object
            
        }
        foreach (var characterScript in DeadFighters)
        {
            if (characterScript.transform.CompareTag("BattleParticipant") 
                && characterScript.hostility == CharacterScript.HostilityToPlayer.Friendly)
                characterScript.transform.parent = playerParent; //reattach players to their map parent object

        }

        GameObject.Destroy(enemyParent.root.gameObject);
        GameManager.instance.ChangeToNewScene(sceneToChangeTo);
    }

    void SetupAbilitiesDropDownMenu()
    {
        abilityDropDownMenu.value = 0;
        List<Dropdown.OptionData> newOptions = new List<Dropdown.OptionData>();
        foreach (var ability in currentTurnTaker.abilities)
        {
            Dropdown.OptionData newOption = new Dropdown.OptionData(ability.aName);
            if (ability.aSprite != null)
                newOption.image = ability.aSprite;
            newOptions.Add(newOption);
        }
        abilityDropDownMenu.options = newOptions;
        abilityDropDownMenu.RefreshShownValue();
    }

    float getSpeedValue(CharacterScript fighter)
    {
        return fighter.speed / AllFighters.Count; //Fighter speed divided by amount of fighters
    }
}
