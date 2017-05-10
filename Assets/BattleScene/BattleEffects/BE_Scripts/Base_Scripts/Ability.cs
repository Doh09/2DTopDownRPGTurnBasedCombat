using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class Ability : ScriptableObject
{

    public string aName = "New Ability";
    public Sprite aSprite;
    public AudioClip aSound;
    public float aBaseCoolDown = 1f;
    public List<CharacterScript.HostilityToPlayer> canTarget;
    
    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility(Transform user, Transform target, BattleManager.MakeDamageText makeDmgText);// Transform abilityEffect,
}