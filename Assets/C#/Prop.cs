using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour {
    public float durability = 1;
    public Sprite destroyedSprite;
    private ParticleSystem ps;

    private SpriteRenderer myRenderer;
    private Sprite defaultSprite;
    // Use this for initialization
    void Start ()
    {
        myRenderer = this.GetComponent<SpriteRenderer>();
        defaultSprite = myRenderer.sprite;
        ps = GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.enabled = false;
    }

    // called to decrease the durability of a prop by amount
    public void decreaseDurability(float amount)
    {
        print("hello!");
        this.durability -= durability;
        destroy();
    }

    //called to change durability of a prop
    public void changeDurability(float durability) {
        this.durability = durability;
        destroy();
    }

    // This should be called when the prop gets shot
    void destroy() {
        if(isDestroyed())
        {
            myRenderer.sprite = destroyedSprite;
            SpriteRenderer destroyed1;
            var emission = ps.emission;
            emission.enabled = true;
        }
    }

    // This should be called to get whether or not this has been destroyed
    public bool isDestroyed()
    {
        return this.durability <= 0;
    }
}
