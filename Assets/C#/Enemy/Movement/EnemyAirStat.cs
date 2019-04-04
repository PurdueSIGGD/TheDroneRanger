using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirStat : EnemyMovement
{
    // Start is called before the first frame update
	private float oldGrav;

	public override void sSwitchTo() {
		
		if (enemyRigid.gravityScale != 0) {
			oldGrav = enemyRigid.gravityScale;
			enemyRigid.gravityScale = 0;
		}
		master.setAnimState ("Walking", false);
	}

	public override void sSwitchFrom() {
		
		enemyRigid.gravityScale = oldGrav;
	}

    // Update is called once per frame
	public override void sUpdate()
	{
		enemyRigid.velocity = new Vector2 (0, 0);
	}
}
