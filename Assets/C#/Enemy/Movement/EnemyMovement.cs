using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public string label;
	public int xdir = 1;
	public int ydir = 1;
	public bool flipSprite = true;
	public float jumpHeight = 3;
	public bool hasCustom = false;
	public float customRange;
	public float customVStart;
	public float customVEnd;
	[HideInInspector]
	public EnemyAIMaster master;
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

		//Vector2 bottom = (Vector2)transform.position + new Vector2 (0, -gameObject.GetComponent<Collider2D> ().bounds.size.y / 2.0f - .01f);

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

	public void switchTo() {
		master.setAnimState ("LookingLeft", (xdir == -1));
		//Debug.Log ("switch");
		if (hasCustom) {
			//Debug.Log("hasCustom" + customRange + ", " + customVStart + ", " + customVEnd);
			master.attackScript.range = customRange;
			master.attackScript.visionStart = customVStart;
			master.attackScript.visionEnd = customVEnd;


		} else {
			//Debug.Log ("default");
			master.attackScript.setDefaultBounds ();
		}

		sSwitchTo ();
	}

	public virtual void sSwitchTo() {
		master.setAnimState ("Walking", false);
	}

	public void switchFrom() {
		if (hasCustom) {
			
		}
		sSwitchFrom ();
	}

	public virtual void sSwitchFrom() {
		
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
		//Debug.Log ("flip");
		if (xdir > 0) {
			xdir = -1;
			if (flipSprite) {
				//gameObject.GetComponent<SpriteRenderer>().flipX = true;
				master.setAnimState ("LookingLeft", true);
			}
		} else {
			xdir = 1;
			if (flipSprite) {
				//gameObject.GetComponent<SpriteRenderer>().flipX = false;
				master.setAnimState ("LookingLeft", false);
			}
		}
	}
}
