using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shamlessly stolen from Andrew Lonsway - edited by Clayton Detke
public class Projectile : MonoBehaviour {
    [HideInInspector]
    public GameObject sourcePlayer;

    public float damage = 1;
    public float lifetime = 10;
    public bool dieOnHit = true;

    private bool hasHit;

    // Use this for initialization
    void Start () {
        Invoke("DestroyMe", lifetime);
    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        /* CHECKS FOR HIT VALIDIDTY */
        if (hasHit && dieOnHit) return; // We only want to hit one object... for some reason it collides multiple times before destroying itself
        if (col.isTrigger) return; // Only want our own trigger effects
        if (!sourcePlayer) return;
        if (col == sourcePlayer.GetComponent<Collider2D>()) return;
        Attributes attr;
        Prop p;
        if ((attr = col.GetComponentInParent<Attributes>()))
        {
            if (attr.gameObject == sourcePlayer.gameObject) return;
        }
        else if ((p = col.GetComponentInParent<Prop>()))
        {
            if (!p.isDestroyed())
            {
                p.decreaseDurability(damage);
                destroyThis();
            }
            else { return; }
        }

        /* ACTIONS TO TAKE POST-HIT */
        hasHit = true;
        //TODO: check to see if we can hit the thing that we collided with
        if (attr != null)
        {
            attr.takeDamage(damage);
        }

        destroyThis();
    }

    private void destroyThis()
    {
        if (dieOnHit)
        {
            //TODO: should I blow up?
            Destroy(this.gameObject);
        }
    }
}
