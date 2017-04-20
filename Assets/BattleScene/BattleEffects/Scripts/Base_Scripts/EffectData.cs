using UnityEngine;

public abstract class EffectData : ScriptableObject
{
    public Sprite itemSprite;
    public abstract void UseEffect(Transform User, Transform Target); //Insert Player/Character script as parameter.
}
