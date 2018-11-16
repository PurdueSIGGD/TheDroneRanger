using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileSpawner : ProjectileSpawner {
	private EnemyAttributes enemyStats;
	private bool usesGravity;
	private float gravScale;
	public float arc = .5f; //value from 0 to 1 describing how direct the path of thrown object should follow

	public void setProjectile(GameObject proj)
	{
		projectile = proj;
		gravScale = projectile.GetComponent<Rigidbody2D> ().gravityScale;
		usesGravity = (gravScale != 0);
	}


	public override void Start()
	{
		enemyStats = GetComponent<EnemyAttributes> ();
		setProjectile (projectile);
	}

	public Vector2 CalcTrajectory()
	{
		//float g = projectile.GetComponent<Rigidbody2D> ().gravityScale;
		Vector2 SpawnPosition = transform.position;
		Vector2 TargetPos = enemyStats.getAggro ().position;
		Vector2 Direction;
		if (!usesGravity) {
			
			Direction = TargetPos - SpawnPosition;
			Direction.Normalize ();
			return Direction * thrust;
		}

		float yf = TargetPos.y - SpawnPosition.y;
		float xf = TargetPos.x - SpawnPosition.x;
		float g = -Physics2D.gravity.y * gravScale;

		/*
		float theta = Mathf.Pow(thrust, 4) + 2 * g * yf * thrust * thrust - g * g * xf * xf;
		theta = -xf * Mathf.Sqrt( -Mathf.Sqrt(theta) + g * yf + thrust * thrust);
		theta = Mathf.Acos(theta / (thrust * Mathf.Sqrt(2*yf * yf + 2*xf * xf)));

		float vx = Mathf.Cos(theta);
		float vyo = Mathf.Sin(theta);
*/
		//float vx = thrust / Mathf.Sqrt (Mathf.Pow ((yf - Physics2D.gravity.y * xf * xf / 2) / xf, 2) + 1);
		//float vyo = Mathf.Sqrt (thrust * thrust - vx * vx); 
		float b = thrust * thrust - yf * g;
		float discr = b * b - g * g * (xf * xf + yf * yf);

		//

		float discRoot = Mathf.Sqrt (discr);
		float tMax = Mathf.Sqrt ((b + discRoot) * 2 / (g * g));
		float tMin = Mathf.Sqrt ((b - discRoot) * 2 / (g * g));
		float t = tMin + (tMax - tMin) * arc;
		float vx = xf / t;
		float vyo = yf / t + t * g / 2;

		Direction = new Vector2 (vx, vyo);
		//Debug.Log (Direction.magnitude);
		//Direction.Normalize ();

		return Direction;
	}



	public override void use_UseAbility()
	{
		Vector2 SpawnPosition = transform.position;
		Vector2 Direction = CalcTrajectory ();
		if (float.IsNaN(Direction.x)) {
			enemyStats.setInRange (false);
			return;
		}
		enemyStats.setInRange (true);
		//TODO add targeting and spawn the bullet
		GameObject Bullet = GameObject.Instantiate(projectile);
		Bullet.GetComponent<Projectile>().sourcePlayer = this.gameObject;
		Bullet.GetComponent<Transform>().position = SpawnPosition;
		Bullet.GetComponent<Rigidbody2D>().AddForce(Bullet.GetComponent<Rigidbody2D>().mass * Direction, ForceMode2D.Impulse);
	}
		
	public override void Update()//triggering attacks will be handled in an AI script
	{

	}


}
