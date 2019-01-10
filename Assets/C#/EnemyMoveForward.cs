using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveForward : EnemyMovement {

	public float speed;

	// Update is called once per frame
	public override void Update () {
		//Debug.Log ("forward");
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xdir * speed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
	}
}
