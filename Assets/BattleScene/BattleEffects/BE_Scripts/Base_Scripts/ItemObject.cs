using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ItemObject : MonoBehaviour
{
    public EffectData EffectData;

    void Start()
    {
        Sprite aSprite = EffectData.itemSprite;
        GetComponent<SpriteRenderer>().sprite = aSprite;

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(aSprite.bounds.size.x, aSprite.bounds.size.y);
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player") return; //Do something?
    }
}
