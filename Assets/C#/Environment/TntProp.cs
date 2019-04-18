using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TntProp : Prop
{
    public float damage = 10;
    public CircleCollider2D explosionCol;
    [SerializeField]
    protected ParticleSystem explosionEffect;

    public override void destroy(){
        if (isDestroyed())
        {
            if (this.GetComponent<AudioSource>() != null)
            {
                this.GetComponent<AudioSource>().PlayOneShot(breakSound);
            }
            this.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
            BoxCollider2D box = this.GetComponent<BoxCollider2D>();
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            var emission = ps.emission;
            emission.enabled = true;
            Destroy(box, 0);
            StartCoroutine(stopAnimation());
            explode();
            explosionEffect.Play();
            StartCoroutine("StopExplosion");
        }
    }

    private void explode()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        Collider2D[] results = new Collider2D[100];

        explosionCol.OverlapCollider(filter, results);

        foreach (Collider2D col in results)
        {
            if (col == null) continue;
            if (col.isTrigger) continue;
            if (col.GetComponent<Attributes>() != null) col.GetComponent<Attributes>().takeDamage(damage);
            if (col.GetComponent<Prop>() != null) col.GetComponent<Prop>().decreaseDurability(damage);
        }
    }

    protected IEnumerator StopExplosion()
    {
        yield return new WaitForSeconds(.25f);
        explosionEffect.Clear();
    }
}

