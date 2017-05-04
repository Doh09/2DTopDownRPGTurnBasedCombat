using UnityEngine;
using System.Collections;

public abstract class Ability : ScriptableObject
{

    public string aName = "New Ability";
    public Sprite aSprite;
    public AudioClip aSound;
    public float aBaseCoolDown = 1f;
    public bool canTargetAllies;
    public bool canTargetEnemies;
    
    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility(Transform user, Transform target, BattleManager.MakeDamageText makeDmgText);// Transform abilityEffect,
}