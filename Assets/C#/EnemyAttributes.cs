using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttributes {
	private double health;
	//public Weapon currentWeapon;
	private double attackRate;
	private double moveSpeed;
	private Rigidbody2D aggroRigid;

	//default constructor
	public EnemyAttributes() {
		health = 100.0;
		attackRate = 1;
		moveSpeed = 1;
		aggroRigid = null;
	}

	//constructor
	public EnemyAttributes(double health, double rate, double speed) {
		this.health = health;
		attackRate = rate;
		moveSpeed = speed;
		aggroRigid = null;

	}

	//accessors
	public double getHealth() {
		return health;
	}

	public double getAttackRate() {
		return attackRate;
	}

	public double getMoveSpeed() {
		return moveSpeed;
	}

	public Rigidbody2D getAggro()
	{
		return aggroRigid;
	}

	//mutators

	//returns -1 if player is dead, 1 otherwise
	public int takeDamage(double damage) {
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

	public void setHealth(double h)
	{
		health = h;
	}

	//adds percent to the current high noon percent
	public void setAttackRate(double rate) {
		attackRate += rate;
	}

	//returns true if highNoonPercent >= 100
	public void setMoveSpeed(double speed)
	{
		moveSpeed = speed;
	}

	public void setAggro(Rigidbody2D playerRigid)
	{
		aggroRigid = playerRigid;
	}
}