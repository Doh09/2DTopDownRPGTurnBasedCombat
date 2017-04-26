using UnityEngine;
using System.Collections;

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
        var abilityFired = Instantiate(projectile.gameObject, user, true); //Remember to put script on spawned abilities that kills them after x seconds, in case they miss target due to some an error.
        //Control the spawned projectile.
        abilityFired.transform.position = user.position;
        var heading = target.position - user.position; //calculate direction
        abilityFired.GetComponent<Rigidbody2D>().AddForce(heading*projectileForce); //apply force in direction
        makeDmgText(damage, user.GetComponent<CharacterScript>(), target.GetComponent<CharacterScript>());
        Debug.Log("Firing projectile ability at "+target.gameObject.name+" with force "+projectileForce);
    }

}