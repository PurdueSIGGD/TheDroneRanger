using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public int EnemySpeed;
	public int XMoveDirection;

	void Update () {
		//make sure to Ignore Raycast on the enemy Layer so it doesn't detect itself, do this for anything it doesn't mind running into
		RaycastHit2D hit = Physics2D.Raycast (transform.position, new Vector2 (XMoveDirection, 0));
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XMoveDirection, 0) * EnemySpeed;
		if (hit.distance < 2f) {
			Debug.Log("uh oh");
			Flip();
		}
	}
	void Flip() {
		if (XMoveDirection > 0) {
			XMoveDirection = -1;
		} else {
			XMoveDirection = 1;
		}
	}
}
