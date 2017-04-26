using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{

    public int damage;

    public void OnContact(CharacterScript turnTaker, CharacterScript target, BattleManager.MakeDamageText dmgText)
    {
        dmgText(damage, turnTaker, target);
    }
}
