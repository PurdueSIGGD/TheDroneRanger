using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttributes : Attributes {
	//public Weapon currentWeapon;
	public float attackRate = 1;
	public float moveSpeed = 1;
	public Rigidbody2D aggroRigid = null;

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

	//mutators

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