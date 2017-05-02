using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public string characterName = "NoNameSet";
    public Sprite portrait = new Sprite();
    public int level;
    public float hp = 100f;
    public HostilityToPlayer hostility = HostilityToPlayer.Friendly;
    public ArmorType armorType = ArmorType.Unarmored;
    public SkinType skinType = SkinType.Flesh;
    public int availableStatPoints = 0;
    public int armorRating = 0;
    public float speed = 1f;
    public float progressToTurnInCombat = 1f;
    public int intelligence = 10;
    public int strength = 10;
    public int damage = 20;
    public List<Ability> abilities;
    public List<DamageAffector> damageAffectors = new List<DamageAffector>(); //A list of objects inhering from DamageAffector, this can be buffs, curses, armorrating etc.

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public enum HostilityToPlayer
    {
        Friendly, Neutral, Enemy
    }

    public enum ArmorType
    {
        Magic, Unarmored, Light, Normal, Heavy, Building
    }

    public enum SkinType
    {
        Flesh, Stone, Wood, ThickFlesh, ThinFlesh, Bone 
    }
}
