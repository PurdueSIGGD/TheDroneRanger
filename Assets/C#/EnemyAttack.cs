using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
	private EnemyAttributes EnemyStats;
	private Rigidbody2D EnemyRigid;
	public float range;
	public float visionStart; //visionStart and visionEnd define between what angles the enemy can see
	public float visionEnd;
	public Collider2D attackZone;


	// Use this for initialization
	void Start () {
		EnemyStats = GetComponent<EnemyAttributes> ();
		EnemyRigid = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		seekTarget ();

		if (EnemyStats.getAggro () != null) {
			attack ();

		}
	}

	public void attack()
	{
		Debug.Log ("I see you at " + EnemyStats.getAggro().position);
	}

	public void seekTarget()
	{
		Collider2D[] hits = new Collider2D[10];
		hits = Physics2D.OverlapCircleAll (EnemyRigid.position, range);

		PlayerMovement playerMove = null;
		for (int i = 0; i < hits.Length; i++) {
			if ((playerMove = hits [i].GetComponentInParent<PlayerMovement> ())) {
				Rigidbody2D playerRigid = playerMove.GetComponentInParent<Rigidbody2D> ();
				float theta = Vector2.SignedAngle (transform.right, playerRigid.position - EnemyRigid.position);
				Debug.Log (theta);
				if (theta < visionStart || theta > visionEnd) {
					continue;

				}
				if (EnemyStats.getAggro () == null) {
					EnemyStats.setAggro (playerRigid);
				}
				//Debug.Log ("I see you");
				break;
			}

		}
		if (playerMove == null) {
			EnemyStats.setAggro (null);
			//Debug.Log ("I see nobody");
		}


	}
}
