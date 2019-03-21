using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
	public float damage;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (damage <= 0)
		{
			Debug.Log("Laser damage value is <= 0");
		} else
		{
			if (collision.gameObject.name.Equals("Player"))
			{
				collision.gameObject.GetComponent<PlayerAttributes>().takeDamage(damage);
			}
		}
	}
}
