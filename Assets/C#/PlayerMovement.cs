using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementAcceleration = 2000;
    public float maxHorizontalVelocity = 10;
    public float upDiagonalVelocity = 7;
    public float downDiagonalVelocity = 6;
    public float diagVelocityOffset = 0.1f;
    public float jumpImpulse = 10.0f;
    public int maxJumps = 1;

    public float crouchMultiplier = 0.5f; //Value collider height is multiplied by
    public Sprite crouchSprite;
    private Sprite defaultSprite;

    private Rigidbody2D myRigid;
    private BoxCollider2D myBox;
    private SpriteRenderer myRenderer;

    private bool isCrouching = false;

    private bool canJump;
    private bool jumping; // The player has hit the jump button and not yet returned to the ground
    private int jumpCounter;
    private float gravity;

    private bool nearLadder = false;
    private bool onLadder = false;
    private bool grounded = false;

    // Use this for initialization
    void Start()
    {
        myRigid = this.GetComponent<Rigidbody2D>();
        myBox = this.GetComponent<BoxCollider2D>();
        myRenderer = this.GetComponent<SpriteRenderer>();

        gravity = myRigid.gravityScale;

        defaultSprite = myRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");
        if (nearLadder && !onLadder && verticalMovement != 0) // Handle getting on ladders
        {
            onLadder = true;
            jumpCounter = 0;
            myRigid.velocity = new Vector2(0, 0);
            myRigid.gravityScale = 0;
        }
        if (onLadder) // Handle being on ladders
        {
            if (Input.GetAxisRaw("Horizontal") == 0 && nearLadder)
            {
                myRigid.velocity = new Vector2(0, Input.GetAxisRaw("Vertical") * 10);
                return;
            }
            else if (!grounded)
            {
                onLadder = false;
                myRigid.gravityScale = gravity;
                myRigid.velocity = new Vector2(0, 0);
                //Jump
                jump(0.5f);
            }
        }

        if (grounded || onLadder) // Prevent slipping off of slopes
        {
            myRigid.gravityScale = 0;
        }
        else
        {
            myRigid.gravityScale = gravity;
        }

        bool goingLeft = Input.GetAxisRaw("Horizontal") < 0;
        bool goingRight = Input.GetAxisRaw("Horizontal") > 0;
        bool goingUp = myRigid.velocity.y > 0.02f;
        bool goingDown = myRigid.velocity.y < -0.01f;
        //Movement (Left / Right)
        if (!grounded) // in air
        {
            if (goingLeft)
            {
                if (goingUp && !jumping) { myRigid.velocity = new Vector2(-maxHorizontalVelocity, diagVelocityOffset); }
                else if (!goingUp && !jumping) { myRigid.velocity = new Vector2(-downDiagonalVelocity, -downDiagonalVelocity-diagVelocityOffset); }
                else { myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y); }
            }
            else if (goingRight)
            {
                if (goingUp && !jumping) { myRigid.velocity = new Vector2(maxHorizontalVelocity, diagVelocityOffset); }
                else if (!goingUp && !jumping) { myRigid.velocity = new Vector2(downDiagonalVelocity, -downDiagonalVelocity-diagVelocityOffset); }
                else { myRigid.velocity = new Vector2(maxHorizontalVelocity, myRigid.velocity.y); }
            }
            else if (!jumping) { myRigid.velocity = new Vector2(0, -downDiagonalVelocity); }
            else{ myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
        }
        else // on ground
        {
            if (goingLeft)
            {
                if (goingUp && !jumping) { myRigid.velocity = new Vector2(-upDiagonalVelocity, upDiagonalVelocity); }
                else if (goingDown && !jumping) { myRigid.velocity = new Vector2(-downDiagonalVelocity, -downDiagonalVelocity); }
                else if (!jumping) { myRigid.velocity = new Vector2(-maxHorizontalVelocity, 0); }
                else { myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y); }
            }
            else if (goingRight)
            {
                if (goingUp && !jumping) { myRigid.velocity = new Vector2(upDiagonalVelocity, upDiagonalVelocity); }
                else if (goingDown && !jumping) { myRigid.velocity = new Vector2(downDiagonalVelocity, -downDiagonalVelocity); }
                else if (!jumping) { myRigid.velocity = new Vector2(maxHorizontalVelocity, 0); }
                else { myRigid.velocity = new Vector2(maxHorizontalVelocity, myRigid.velocity.y); }
            }
            else if (jumping) { myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
            else { myRigid.velocity = new Vector2(0, 0); }
        }

        // Trying to jump
        if (verticalMovement > 0)
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
                jump(1.0f);
            }

        }
        // Trying to crouch
        else if (verticalMovement < 0 && grounded && (!onLadder && !nearLadder))
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

    void jump(float jumpFactor)
    {
        if(!jumping)
        {
            myRigid.velocity = new Vector2(myRigid.velocity.x, 0);
        }
        myRigid.AddForce(new Vector2(0, myRigid.mass * jumpImpulse), ForceMode2D.Impulse);
        jumping = true;
        jumpCounter++;
    }

    void OnCollisionEnter2D(Collision2D col) // Bottom Collider
    {
        jumping = false;
    }
    void OnCollisionStay2D(Collision2D col) // Bottom Collider
    {
        Vector2 bottom = (Vector2)transform.position + new Vector2(0, -myBox.size.y / 2.0f);
        if (Physics2D.Raycast(bottom, -Vector2.up, 0.01f).collider != null && !jumping)
        {
            jumpCounter = 0;
        }
        grounded = true;
    }
    void OnCollisionExit2D(Collision2D col) // Bottom Collider
    {
        grounded = false;
    }
    void OnTriggerStay2D(Collider2D col) // Main Collider
    {
        if (col.GetComponent<Projectile>()) { return; }
        if (col.GetComponent<Ladder>()) { return; }
    }
    void OnTriggerExit2D(Collider2D col) // Main Collider
    {
        if (col.GetComponent<Projectile>()) { return; }
        if (col.GetComponent<Ladder>()) { return; }
    }
    public void setNearLadder(bool near)
    {
        nearLadder = near;
    }
}
