using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour {
    public float durability = 1;
    public Sprite destroyedSprite;
    protected ParticleSystem ps;
    public AudioClip breakSound = null;

    private SpriteRenderer myRenderer;
    private AudioSource audioSource = null;
    // Use this for initialization
    void Start ()
    {
        myRenderer = this.GetComponent<SpriteRenderer>();
        audioSource = this.GetComponent<AudioSource>();
        ps = this.GetComponentInChildren<ParticleSystem>();
        var emission = ps.emission;
        emission.enabled = false;
    }

    // called to decrease the durability of a prop by amount
    public void decreaseDurability(float amount)
    {
        this.durability -= durability;
        destroy();
    }

    //called to change durability of a prop
    public void changeDurability(float durability) {
        this.durability = durability;
        destroy();
    }

    // This should be called when the prop gets shot
    public virtual void destroy() {
        if (isDestroyed())
        {
            if(audioSource != null)
            {
                audioSource.PlayOneShot(breakSound);
            }
            myRenderer.sprite = destroyedSprite;
            BoxCollider2D box = this.GetComponent<BoxCollider2D>();
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            Destroy(box, 0);
            var emission = ps.emission;
            emission.enabled = true;
            StartCoroutine(stopAnimation());
        }
    }

    // This should be called to get whether or not this has been destroyed
    public bool isDestroyed()
    {
        return this.durability <= 0;
    }

    //runs a timer to stop destruction animation after a certain amount of time
    protected IEnumerator stopAnimation() {
        yield return new WaitForSeconds(.1f);
        var emission = ps.emission;
        emission.enabled = false;
    }
}
