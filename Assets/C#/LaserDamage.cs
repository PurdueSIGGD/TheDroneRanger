using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
	private float damage;
	private bool hurtEnemies;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (damage <= 0)
		{
			Debug.Log("Laser damage value is <= 0");
		} else
		{
			if (collision.gameObject.name.Equals("Player") || (hurtEnemies && collision.gameObject.name.Contains("Enemy")))
			{
				collision.gameObject.GetComponent<PlayerAttributes>().takeDamage(damage);
			} 
		}
	}

	public void setValues(float dmg, bool emy)
	{
		damage = dmg;
		hurtEnemies = emy;
	}
}
