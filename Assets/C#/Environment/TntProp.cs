using System;
using System.Collections.Generic;
using UnityEngine;

class TntProp : Prop
{
    public float range = 10;
    public float damage = 10;

    public override void destroy(){
        if (isDestroyed())
        {
            explode();
            this.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
            BoxCollider2D box = this.GetComponent<BoxCollider2D>();
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            Destroy(box, 0);
            print("dead");
        }
    }

    private void explode()
    {
        CircleCollider2D explosion = new CircleCollider2D();
        explosion.isTrigger = true;
        explosion.radius = range;
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        Collider2D[] results = new Collider2D[] { };

        print("dead");

        explosion.OverlapCollider(filter, results);

        foreach (Collider2D col in results)
        {
            print("Collision");
            if (col.GetComponent<Attributes>() == null) continue;
            col.GetComponent<Attributes>().health -= damage;
        }
    }

    void Delete()
    {
        DestroyImmediate(this);
    }
}

