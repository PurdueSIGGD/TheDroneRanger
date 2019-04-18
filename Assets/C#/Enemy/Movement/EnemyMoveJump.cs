using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveJump : EnemyMovement {
	public bool xLocked = false;

	public override void sSwitchTo() {
		master.setAnimState ("Walking", false);
	}

	// Update is called once per frame
	public override void sUpdate () {
		//Debug.Log ("jump");
		jump();
		if (xLocked) {
			enemyRigid.velocity = new Vector2 (0, gameObject.GetComponent<Rigidbody2D> ().velocity.y);
		}
	}
}
