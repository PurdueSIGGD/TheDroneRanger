using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour {

    public float health = 100;
    public float maxHealth = 100;

    //accessor
    public float getHealth()
    {
        return health;
    }
    public float getMaxHealth()
    {
        return maxHealth;
    }

    public bool isDead()
    {
        return health <= 0;
    }

    //mutators
    public int takeDamage(float damage) //returns -1 if player is dead, returns 1 otherwise
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            return -1;
        }
        return 1;
    }

    public void setHealth(float h)
    {
        health = h;
    }
    public void setMaxHealth(float h)
    {
        maxHealth = h;
    }
}
