using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveChase : EnemyMovement {

	public float speed;
	private EnemyAttributes enemyStats;
	private Rigidbody2D enemyRigid;

	void Start()
	{
		enemyStats = GetComponent<EnemyAttributes> ();
		enemyRigid = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	public override void Update () {
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
