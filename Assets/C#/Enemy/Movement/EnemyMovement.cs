using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public string label;
	public int xdir = 1;
	public int ydir = 1;
	public bool flipSprite = true;
	public float jumpHeight = 3;
	private EnemyAIMaster master;
	[HideInInspector]
	public Rigidbody2D enemyRigid;
	//public bool flipOnSwitch = false;

	void Start()
	{
		enemyRigid = gameObject.GetComponent<Rigidbody2D> ();
		master = GetComponent<EnemyAIMaster> ();
		sStart ();
	}

	public virtual void sStart()
	{

	}

	// Update is called once per frame
	public virtual void Update () {
		//Debug.Log ("still");

		Vector2 bottom = (Vector2)transform.position + new Vector2 (0, -gameObject.GetComponent<Collider2D> ().bounds.size.y / 2.0f - .01f);

		//if (master.canJump() && Physics2D.Raycast(bottom, -Vector2.up, .01f))
		//{
			//Debug.Log ("ground");
			//jumpCounter = 0;
		//}
		sUpdate ();
	}

	public virtual void sUpdate()
	{
		enemyRigid.velocity = new Vector2 (0, gameObject.GetComponent<Rigidbody2D> ().velocity.y);
	}

	public void jump()
	{
		

		if (master.canJump ()) {
			//Debug.Log ("jumped");
			master.resetJumpCool ();
			enemyRigid.AddForce (Vector2.up * Mathf.Sqrt (2 * jumpHeight * Physics2D.gravity.magnitude) * enemyRigid.mass, ForceMode2D.Impulse);
		}
		//jumpCounter++;
					
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
