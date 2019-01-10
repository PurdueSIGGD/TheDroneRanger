using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public string label;
	public int xdir = 1;
	public int ydir = 1;
	public bool flipSprite = true;
	//public bool flipOnSwitch = false;

	// Update is called once per frame
	public virtual void Update () {
		//Debug.Log ("still");
		gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, gameObject.GetComponent<Rigidbody2D> ().velocity.y);
	}

	public void xFlip()
	{
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
	}
}
