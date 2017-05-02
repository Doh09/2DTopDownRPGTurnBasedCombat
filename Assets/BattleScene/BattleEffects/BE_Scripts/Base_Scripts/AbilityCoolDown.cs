using UnityEngine;  
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AbilityCoolDown : MonoBehaviour
{
    public bool CooldownIsComplete = true;
    public string abilityButtonAxisName = "Fire1";
    public Image darkMask;
    public Text coolDownTextDisplay;

    [SerializeField]
    public Ability ability;
    [SerializeField]
    private GameObject weaponHolder;
    private Image myButtonImage;
    private AudioSource abilitySource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;


    void Start()
    {
        Initialize(ability, weaponHolder);
    }

    public void Initialize(Ability selectedAbility, GameObject weaponHolder)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();
        myButtonImage.sprite = ability.aSprite;
        darkMask.sprite = ability.aSprite;
        coolDownDuration = ability.aBaseCoolDown;
        ability.Initialize(weaponHolder);
        AbilityReady();
    }

    // Update is called once per frame
    void Update()
    {
        CooldownIsComplete = (Time.time > nextReadyTime);
        if (CooldownIsComplete)
        {
            AbilityReady(); //set the UI, maybe use a boolean parameter and set it opposite of the boolean parameter?

        }
        else
        {
            CoolDown();
        }
    }

    //set the UI
    private void AbilityReady()
    {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }

    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCd = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCd.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    public void FireAbility(Transform user, Transform target, BattleManager.MakeDamageText dmgText)
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;

        abilitySource.clip = ability.aSound;
        abilitySource.Play();
        ability.TriggerAbility(user, target, dmgText);
    }
}