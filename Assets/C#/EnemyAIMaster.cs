using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMaster : MonoBehaviour {
	//public EnemyMovement moveScript;
	public EnemyAttack attackScript;
	//public EnemyProjectileSpawner projSpawner;
	//public GameObject[] weaponList;
	public string[] spawnSeq;
	public string[] moveSeq;
	private EnemyProjectileSpawner[] spawnList;
	private EnemyMovement[] moveList;
	public string aggromove;
	private EnemyMovement aMove;
	public float[] moveDur;
	public int currSpawn = 0;
	public int currMove = 0;
	//public int currWeap = 0;
	//public GameObject PrimaryWeap;
	//public GameObject SecondWeap;
	public float cycleTime = -1;
	private float timeCounter = 0;
	private float moveTimer = 0;
	private float jumpTimer = 0;
	private bool grounded = false;
	private EnemyAttributes EnemyStats;
	private bool aggroed = false;


	// Use this for initialization
	void Start () {
		EnemyStats = GetComponent<EnemyAttributes> ();
		//projSpawner.setProjectile (weaponList[currWeap]);

		spawnList = new EnemyProjectileSpawner[spawnSeq.Length];
		EnemyProjectileSpawner[] allSpawners = GetComponents<EnemyProjectileSpawner> ();
		for (int i = 0; i < allSpawners.Length; i++) {
			allSpawners [i].enabled = false;
		}

		for (int i = 0; i < spawnList.Length; i++) {
			for (int x = 0; x < allSpawners.Length; x++) {
				if (spawnSeq[i].Equals(allSpawners[x].label))
				{
					spawnList [i] = allSpawners [x];
					//spawnList [i].enabled = true;
					Debug.Log (allSpawners [x].label);
					break;
				}
			}
		}
		spawnList [0].enabled = true;

		moveList = new EnemyMovement[moveSeq.Length];
		EnemyMovement[] allMoves = GetComponents<EnemyMovement> ();

		for (int i = 0; i < allMoves.Length; i++) {
			allMoves [i].enabled = false;
		}

		for (int i = 0; i < moveList.Length; i++) {
			for (int x = 0; x < allMoves.Length; x++) {
				if (moveSeq[i].Equals(allMoves[x].label))
				{
					moveList [i] = allMoves [x];
					//moveList [i].enabled = true;
					Debug.Log (allMoves [x].label);
					break;
				}
			}
		}
		moveList [0].enabled = true;
		for (int x = 0; x < allMoves.Length; x++) {
			if (aggromove.Equals(allMoves[x].label))
			{
				aMove = allMoves [x];
				//moveList [i].enabled = true;
				Debug.Log (allMoves [x].label);
				break;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (cycleTime > 0) {
			timeCounter += Time.deltaTime;
			if (timeCounter >= cycleTime) {
				timeCounter -= cycleTime;
				changePattern ();
				//changeMovement ();
			}

		}
		if (!aggroed && moveList.Length > 1) {
			moveTimer += Time.deltaTime;
			if (moveTimer >= Mathf.Abs (moveDur [currMove])) {
				moveTimer -= Mathf.Abs (moveDur [currMove]);
				changeMovement ();
			}
		}
		jumpTimer = Mathf.Max (0, jumpTimer - Time.deltaTime);

		if (EnemyStats.getAggro () && EnemyStats.canHit()) {
			if (!aggroed) {
				aggroed = true;
				getCurrMove ().enabled = false;
				aMove.enabled = true;
			}

		} else {
			if (aggroed) {
				aggroed = false;
				aMove.enabled = false;
				getCurrMove().enabled = true;
			}


		}


	}

	public bool canJump()
	{
		return jumpTimer <= 0 && grounded;
	}

	public void resetJumpCool()
	{
		jumpTimer = EnemyStats.getJumpCool();
	}

	public EnemyProjectileSpawner getCurrSpawner()
	{
		return spawnList [currSpawn];
	}

	public EnemyMovement getCurrMove()
	{
		return moveList [currMove];
	}

	public virtual void changePattern()
	{
		/*
		currWeap++;
		if (currWeap >= weaponList.Length) {
			currWeap = 0;
		}
		projSpawner.setProjectile (weaponList[currWeap]);
		*/
		spawnList [currSpawn].enabled = false;
		currSpawn++;
		if (currSpawn >= spawnList.Length) {
			currSpawn = 0;
		}
		spawnList [currSpawn].enabled = true;
	}

	public virtual void changeMovement()
	{
		int tempxdir = moveList [currMove].xdir;
		moveList [currMove].enabled = false;
		currMove++;

		if (currMove >= moveList.Length) {
			currMove = 0;
		}

		if (moveDur[currMove] < 0) {
			if (moveList [currMove].xdir == tempxdir) {
				moveList [currMove].xFlip ();
			} else {
				//moveList [currMove].xdir = tempxdir;
				moveList [currMove].xFlip ();
				moveList [currMove].xFlip ();
			}

		} else {
			moveList [currMove].xdir = tempxdir;
		} 
		moveList [currMove].enabled = true;
	}

	void OnCollisionStay2D(Collision2D other)
	{
		int i = 0;
		while (i < other.contacts.Length) {
			if (other.contacts [i].point.y < transform.position.y) {
				grounded = true;
				break;
			}
			i++;
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		grounded = false;
	}

}

/*
 * ford = 0
 * 
 * statd = 0
 * 
 * curr = 1
 * 
 * temp = 0
 * 
 * 
 * 
*/