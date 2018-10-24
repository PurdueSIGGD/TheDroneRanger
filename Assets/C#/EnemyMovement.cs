using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public int xdir = 1;
	public int ydir = 1;
	public float radius;
	public float speed;
	public bool flipSprite;

	void Start() {
		StartCoroutine(Move());
	}

	void Update () {
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xdir, 0) * speed;
	}

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
}
