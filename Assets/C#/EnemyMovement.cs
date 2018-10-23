using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public int xdir = 1;
	public int ydir = 1;
	public int radius;
	public int speed;

	private float timer = 0;

	void Update () {
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xdir, 0) * speed;
		timer += Time.deltaTime;
		if (timer >= radius / speed) {
			timer = 0;
			Debug.Log("Nice");
			FlipX();
		}
	}
	void FlipX() {
		if (xdir > 0) {
			xdir = -1;
			gameObject.GetComponent<SpriteRenderer>().flipX = true;
		} else {
			xdir = 1;
			gameObject.GetComponent<SpriteRenderer>().flipX = false;
		}
	}
}
