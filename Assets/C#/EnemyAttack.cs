using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
	private EnemyAttributes EnemyStats;
	private Rigidbody2D EnemyRigid;
	private EnemyMovement EnemyMove;
	public float range;
	public float visionStart; //visionStart and visionEnd define between what angles the enemy can see
	public float visionEnd;


	// Use this for initialization
	void Start () {
		EnemyStats = GetComponent<EnemyAttributes> ();
		EnemyMove = GetComponent<EnemyMovement> ();
		EnemyRigid = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		seekTarget ();

		if (EnemyStats.getAggro () != null) {
			attack ();

		}
	}

	public virtual void attack()
	{
		//Debug.Log ("I see you at " + EnemyStats.getAggro().position);
	}

	public void seekTarget() 
	{
		Collider2D[] hits;
		hits = Physics2D.OverlapCircleAll (EnemyRigid.position, range); //get all colliders within range

		PlayerMovement playerMove = null;

		for (int i = 0; i < hits.Length; i++) {							//iterate through colliders and see if any classify as player
			
			if ((playerMove = hits [i].GetComponentInParent<PlayerMovement> ())) {
				Rigidbody2D playerRigid = playerMove.GetComponentInParent<Rigidbody2D> ();
				float theta = Vector2.SignedAngle (transform.right * EnemyMove.xdir, playerRigid.position - EnemyRigid.position);
				//Debug.Log (EnemyMove.xdir);
				if (theta < visionStart || theta > visionEnd) {			//check if player is within acceptable angle range for field of vision
					EnemyStats.setAggro (null);
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
