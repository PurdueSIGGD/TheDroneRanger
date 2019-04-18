using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovePace : EnemyMovement {
	
	public float radius;
	public float speed;

	private float clock;

	void Start() {
		clock = 0;
		//StartCoroutine(Move());
	}

	public override void Update () {
		clock += Time.deltaTime;
		if (clock >= radius / speed) {
			clock -= radius/speed;
			xFlip ();
		}
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xdir * speed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
		Debug.Log ("forward");
	}

	public override void sSwitchTo() {
		master.setAnimState ("Walking", true);
	}
	/*
	IEnumerator Move() {
		while (true) {
			if (xdir > 0) {
				xdir = -1;
				if (flipSprite) {
					gameObject.GetComponent<SpriteRenderer>().flipX = true;
				}
			} else {
				xdir = 1;
				if (flipSprite) {
					gameObject.GetComponent<SpriteRenderer>().flipX = false;
				}
			}
			yield return new WaitForSeconds(radius / speed);
		}
	}
	*/
}
