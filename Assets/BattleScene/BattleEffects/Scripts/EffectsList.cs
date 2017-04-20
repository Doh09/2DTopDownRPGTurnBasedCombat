using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsList : MonoBehaviour
{

    public List<EffectData> EffectsToUse = new List<EffectData>(); //A list of effects used by this BattleEffect.

    public void UseEffects(Transform user, Transform target)
    {
        Debug.Log("Began using effects");
        foreach (var effect in EffectsToUse)
        {
         effect.UseEffect(user, target);   //should maybe be called Async so it doesnt have to wait for effect parts to finish and they start synchronously?
        }
    }
}
