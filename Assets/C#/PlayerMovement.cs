using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public int movementSpeed = 3000;
    public int maxHorizontalVelocity = 200;
    public int jumpHeight = 400;

    private Rigidbody2D myRigid;
    private BoxCollider2D myBox;

    private bool isCrouching = false;

    private bool canJump;
    private int jumpCounter, maxJumps = 2;

	// Use this for initialization
	void Start () {
        myRigid = this.GetComponent<Rigidbody2D>();
        myBox = this.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontalMovement = movementSpeed * Input.GetAxis("Horizontal");
        if((horizontalMovement > 0 && myRigid.velocity.x < maxHorizontalVelocity)
            || (horizontalMovement < 0 && myRigid.velocity.x > -maxHorizontalVelocity))
        {
            myRigid.AddForce(new Vector2(horizontalMovement * Time.deltaTime, 0));
        }
        if (Input.GetAxisRaw("Vertical") > 0 && canJump && jumpCounter < maxJumps)
        {
            myRigid.AddForce(new Vector2(0, jumpHeight));
            jumpCounter++;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            //TODO: add crouching functionality
            if (!isCrouching)
            {

                

            }

        }
        canJump = Input.GetAxis("Vertical") <= 0;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        jumpCounter = 0;
    }
}
