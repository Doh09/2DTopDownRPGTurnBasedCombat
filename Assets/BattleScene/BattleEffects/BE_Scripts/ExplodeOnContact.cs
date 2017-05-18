using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DamageOnContact))]
public class ExplodeOnContact : MonoBehaviour
{
    private Transform _user;
    private Transform _target;
    private BattleManager.MakeDamageText _makeDmgText;
    public float _contactDistanceToTarget = 1f;
    public GameObject Explosion;

    void initialize(Transform user, Transform target, BattleManager.MakeDamageText makeDmgText)
    {
        _user = user;
        _target = target;
        _makeDmgText = makeDmgText;
    }

    void Start()
    {
        //the parent of this object MUST be set when it is instantiated, and the parent MUST be the target.
        _target = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            //Check if within distance of target
            if (Vector2.Distance(_target.position, transform.position) < _contactDistanceToTarget)
            {
                //Explode
                Animator anim = null;
                if (_target.GetComponent<Animator>() != null)
                {
                    anim = _target.GetComponent<Animator>();
                    anim.SetTrigger("WasHit");
                }
                
                var explosion = Instantiate(Explosion, transform, true);
                explosion.transform.position = transform.position;
                explosion.transform.SetParent(null);
                Destroy(gameObject);
            }
        }
    }
}
