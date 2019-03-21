using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjMelee : EnemyProjectileSpawner
{
	public float offsetDist;
	public float offsetAngle;
	public float atkRange;

    // Update is called once per frame
	public override void sAbility()
	{
		
		//Debug.Log ("Using");
		Vector2 Direction = master.getCurrMove().xdir * offsetDist * new Vector2(Mathf.Cos(offsetAngle), Mathf.Sin(offsetAngle));
		Vector2 SpawnPosition = (Vector2)transform.position + Direction;
		if (Vector2.Distance(transform.position, enemyStats.getAggro().position) > atkRange) {
			enemyStats.setInRange (false);
			return;
		}
		enemyStats.setInRange (true);

		//TODO add targeting and spawn the bullet
		GameObject Bullet = GameObject.Instantiate(projectile);
		Bullet.GetComponent<Projectile>().sourceObj = this.gameObject;
		Bullet.GetComponent<Projectile> ().hurtPlayer = true;
		Bullet.GetComponent<Transform>().position = SpawnPosition;
	}
}
