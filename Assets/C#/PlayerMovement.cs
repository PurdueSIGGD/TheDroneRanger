using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementAcceleration = 2000;
    public float maxHorizontalVelocity = 10;
    public float jumpImpulse = 7.0f;
    public int maxJumps = 1;

    public float crouchMultiplier = 0.5f; //Value collider height is multiplied by
    public Sprite crouchSprite;
    private Sprite defaultSprite;

    private Rigidbody2D myRigid;
    private BoxCollider2D myBox;
    private SpriteRenderer myRenderer;

    private bool isCrouching = false;

    private bool canJump;
    private int jumpCounter;

	// Use this for initialization
	void Start () {

        myRigid = this.GetComponent<Rigidbody2D>();
        myBox = this.GetComponent<BoxCollider2D>();
        myRenderer = this.GetComponent<SpriteRenderer>();

        defaultSprite = myRenderer.sprite;

	}
	
	// Update is called once per frame
	void Update () {

        float horizontalMovement = movementAcceleration * Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        if((horizontalMovement > 0 && myRigid.velocity.x < maxHorizontalVelocity)
            || (horizontalMovement < 0 && myRigid.velocity.x > -maxHorizontalVelocity))
        {

            myRigid.AddForce(new Vector2(myRigid.mass * horizontalMovement * Time.deltaTime, 0));

        }
        
        if (Mathf.Abs(myRigid.velocity.x) > maxHorizontalVelocity)
        {
            Vector3 velocity = myRigid.velocity;
            int sign = (velocity.x > 0 ? 1 : -1);
            velocity.x = sign * maxHorizontalVelocity;
            myRigid.velocity = velocity;
        }

        if (verticalMovement >= 0)
        {

            if (isCrouching)
            {

                isCrouching = false;
                myBox.size = new Vector2(myBox.size.x, myBox.size.y / crouchMultiplier);

                myBox.offset = new Vector2(0, 0);
                myRenderer.sprite = defaultSprite;

            }

            if (verticalMovement > 0 && canJump && jumpCounter < maxJumps)
            {
                myRigid.AddForce(new Vector2(0, myRigid.mass * jumpImpulse), ForceMode2D.Impulse);
                jumpCounter++;
            }

        }
        else if (verticalMovement < 0)
        {
            
            if (!isCrouching)
            {

                isCrouching = true;
                float offset = myBox.size.y * (1.0f - crouchMultiplier) / 2.0f; //How much the bottom boundary is displaced
                myBox.size = new Vector2(myBox.size.x, myBox.size.y * crouchMultiplier);

                myBox.offset = new Vector2(0, -offset);
                myRenderer.sprite = crouchSprite;

            }

        }

        canJump = verticalMovement <= 0;

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 bottom = (Vector2)transform.position + new Vector2(0, -myBox.size.y / 2.0f);
        if (Physics2D.Raycast(bottom, -Vector2.up, 0.01f).collider != null)
        {
            
            jumpCounter = 0;

        }

    }

}
