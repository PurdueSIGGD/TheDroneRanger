using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveChase : EnemyMovement {

	public float speed;
	private EnemyAttributes enemyStats;

	public override void sStart()
	{
		enemyStats = GetComponent<EnemyAttributes> ();

	}

	public override void sSwitchTo() {
		master.setAnimState ("Walking", true);
	}

	// Update is called once per frame
	public override void sUpdate () {
		//Debug.Log ("chase");
		Rigidbody2D aggroRigid;

		if (aggroRigid = enemyStats.getAggro ()) {
			//Debug.Log((aggroRigid.position.x - transform.position.x) + " " + xdir);
			if (Mathf.Sign (aggroRigid.position.x - transform.position.x) != xdir) {
				
				xFlip ();
			}
		}

		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xdir * speed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
	}
}
