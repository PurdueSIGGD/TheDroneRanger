﻿using System;
using System.Collections.Generic;
using UnityEngine;

class TntProp : Prop
{
    public float damage = 10;
    public CircleCollider2D explosionCol;

    public override void destroy(){
        if (isDestroyed())
        {
            explode();
            this.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
            BoxCollider2D box = this.GetComponent<BoxCollider2D>();
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            var emission = ps.emission;
            emission.enabled = true;
            Destroy(box, 0);
            StartCoroutine(stopAnimation());
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
            if (col.GetComponent<Attributes>() == null) continue;
            col.GetComponent<Attributes>().health -= damage;
        }
    }
}
