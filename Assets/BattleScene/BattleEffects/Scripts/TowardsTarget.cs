using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A SO that can be applied to a battle effect to give it life.
/// </summary>
[CreateAssetMenu]
public class TowardsTarget : EffectData
{
    public Transform battleEffect;
    private Transform target;
    private Transform user;
    private BattleManager battleManager;
    public float MovementSpeed = 3f;
    public Vector3 offsetTarget;
    public float minDistanceToTarget = 0.1f;
    private Vector3 actualTarget;
    private bool started = false;

    public override void UseEffect(Transform User, Transform Target)
    {
        target = Target;
        user = User;
        actualTarget = target.position + offsetTarget;
        MoveToTarget();
    }

    void MoveToTarget()
    {
            Debug.Log("BattleEffect - Towards Target started");
            if (Mathf.Abs(battleEffect.position.x) - Mathf.Abs(actualTarget.x) > minDistanceToTarget
                            && Mathf.Abs(battleEffect.position.y) - Mathf.Abs(actualTarget.y) > minDistanceToTarget)
            {  //Move towards target if not at target.
                battleEffect.transform.position = Vector2.MoveTowards(new Vector2(battleEffect.position.x, battleEffect.position.y), target.position, MovementSpeed * Time.deltaTime);
            }
            else if (Mathf.Abs(battleEffect.position.x) - Mathf.Abs(actualTarget.x) < minDistanceToTarget
                && Mathf.Abs(battleEffect.position.y) - Mathf.Abs(actualTarget.y) < minDistanceToTarget)
            {
                HitTarget(); //Hit target if correct location reached.
            }
    }

    void HitTarget()
    {
        Debug.Log("Hitting target");
        Destroy(battleEffect.gameObject);
        Destroy(this);
    }
}
