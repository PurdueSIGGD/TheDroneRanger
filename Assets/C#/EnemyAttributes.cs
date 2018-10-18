using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttributes : MonoBehaviour {
	public float health = 100;
	//public Weapon currentWeapon;
	public float attackRate = 1;
	public float moveSpeed = 1;
	public Rigidbody2D aggroRigid = null;

	//accessors
	public float getHealth() {
		return health;
	}

	public float getAttackRate() {
		return attackRate;
	}

	public float getMoveSpeed() {
		return moveSpeed;
	}

	public Rigidbody2D getAggro()
	{
		return aggroRigid;
	}

	//mutators

	//returns -1 if player is dead, 1 otherwise
	public int takeDamage(float damage) {
		health -= damage;
		if (health <= 0) {
			health = 0;
			return -1;
		}
		return 1;
	}

	public bool isDead()
	{
		return health <= 0;
	}

	public void setHealth(float h)
	{
		health = h;
	}

	//adds percent to the current high noon percent
	public void setAttackRate(float rate) {
		attackRate += rate;
	}

	//returns true if highNoonPercent >= 100
	public void setMoveSpeed(float speed)
	{
		moveSpeed = speed;
	}

	public void setAggro(Rigidbody2D playerRigid)
	{
		aggroRigid = playerRigid;
	}
}