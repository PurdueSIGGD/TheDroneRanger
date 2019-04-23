using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallDamage : MonoBehaviour
{
    public float damage = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerAttributes>() != null)
        {
            collision.GetComponent<PlayerAttributes>().takeDamage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerAttributes>() != null)
        {
            collision.GetComponent<PlayerAttributes>().takeDamage(damage);
        }
    }
}
