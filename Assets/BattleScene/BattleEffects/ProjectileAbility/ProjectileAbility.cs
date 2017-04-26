using UnityEngine;
using System.Collections;

/// <summary>
/// This script fires a projectile, it can be a fireball, a bomb, a throwing knife, an arrow or similar.
/// It does NOT handle contact with the target, this is to be handle by the projectile itself.
/// The target will be set as the parent object of the instantiated projectile.
/// </summary>
[CreateAssetMenu(menuName = "Abilities/ProjectileAbility")]
public class ProjectileAbility : Ability
{
    public float projectileForce = 50f;
    public int damage = 50;
    public Rigidbody2D projectile; //Projectile MUST have a rigidbody2D

    public override void Initialize(GameObject obj)
    {

    }

    public override void TriggerAbility(Transform user, Transform target, BattleManager.MakeDamageText makeDmgText)
    {
        //Spawn projectile and set its caster/user to be the parent.
        var abilityFired = Instantiate(projectile.gameObject, target, true);
        //Control the spawned projectile.
        abilityFired.transform.position = user.position;
        var heading = target.position - user.position; //calculate direction
        abilityFired.GetComponent<Rigidbody2D>().AddForce(heading*projectileForce); //apply force in direction
        makeDmgText(damage, user.GetComponent<CharacterScript>(), target.GetComponent<CharacterScript>());
        Debug.Log("Firing projectile ability at "+target.gameObject.name+" with force "+projectileForce);
        //Fired projectile has to handle what happens from here.
    }

}