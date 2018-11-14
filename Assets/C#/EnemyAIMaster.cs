using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMaster : MonoBehaviour {
	private EnemyMovement moveScript;
	private EnemyAttack attackScript;
	private EnemyAttributes EnemyStats;


	// Use this for initialization
	void Start () {
		moveScript = GetComponent<EnemyMovement> ();
		attackScript = GetComponent<EnemyAttack> ();
		EnemyStats = GetComponent<EnemyAttributes> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (EnemyStats.getAggro ()) {
			//moveScript.enabled = false;
		} else {
			//moveScript.enabled = true;
		}


	}
}
