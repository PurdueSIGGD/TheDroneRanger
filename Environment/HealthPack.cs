using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int Health;
    public bool Max;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        PlayerAttributes attr = collision.GetComponent<PlayerAttributes>();
        if (attr == null) return;
        if (attr.health == attr.maxHealth) return;
        if (Max) attr.health = attr.maxHealth;
        else if (attr.health + Health > attr.maxHealth) attr.health = attr.maxHealth;
        else attr.health += Health;
        Destroy(this.gameObject);
    }
}
