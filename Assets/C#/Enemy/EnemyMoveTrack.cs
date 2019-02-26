using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTrack : EnemyMovement
{
	private EnemyAttributes enemyStats;

	public override void sStart()
	{
		enemyStats = GetComponent<EnemyAttributes> ();
	}
		
	public override void sUpdate()
    {
		Rigidbody2D aggroRigid;
		if (aggroRigid = enemyStats.getAggro ()) {
			//Debug.Log((aggroRigid.position.x - transform.position.x) + " " + xdir);
			if (Mathf.Sign (aggroRigid.position.x - transform.position.x) != xdir) {

				xFlip ();
			}
		}
		enemyRigid.velocity = new Vector2 (0, gameObject.GetComponent<Rigidbody2D> ().velocity.y);
    }
}
