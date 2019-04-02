﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveForward : EnemyMovement {

	public float speed;

	public override void sSwitchTo() {
		master.setAnimState ("Walking", true);
	}

	// Update is called once per frame
	public override void sUpdate () {
		//Debug.Log ("forward");
		enemyRigid.velocity = new Vector2(xdir * speed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

	}
}
