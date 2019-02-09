using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributes : Attributes {
	//public Weapon currentWeapon;
	public float attackRate = 1;
	public float moveSpeed = 1;
	public float jumpCool = .5f;
	public Rigidbody2D aggroRigid = null;
	private bool inRange;

	//accessors
    
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

	public bool hasAggroRigid ()
	{
		Debug.Log ("Checking" + (aggroRigid != null));
		return (aggroRigid != null);
	}

	public float getJumpCool()
	{
		return jumpCool;
	}

	public bool canHit()
	{
		return inRange;
	}

	//mutators
    
	public void setAttackRate(float rate) {
		attackRate += rate;
	}

	public void setMoveSpeed(float speed)
	{
		moveSpeed = speed;
	}

	public void setAggro(Rigidbody2D playerRigid)
	{
		aggroRigid = playerRigid;
	}

	public void setJumpCool(float j)
	{
		jumpCool = j;
	}

	public void setInRange(bool x)
	{
		inRange = x;
	}

	public override bool takeDamage(float damage) //returns true if alive, false if dead
	{
		health -= damage;
		//Debug.Log ("Enemy Damage taken");
		if (health <= 0)
		{
			health = 0;
			death ();
			return false;
		}
		return true;
	}

	public void death()
	{
		Destroy (this.gameObject);

	}
}