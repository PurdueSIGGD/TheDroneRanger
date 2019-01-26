using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementAcceleration = 2000;
    public float maxHorizontalVelocity = 10;
    public float slopeVelocity = 9;
    public float jumpImpulse = 10.0f;
    public int maxJumps = 1;
    private Vector2 platformSpeed;

    //public float crouchMultiplier = 0.5f; //Value collider height is multiplied by
    //public Sprite crouchSprite;
    //private Sprite defaultSprite;

    private Rigidbody2D myRigid;
    //private BoxCollider2D myBox;
    //private SpriteRenderer myRenderer;

    //private bool isCrouching = false;

    private bool canJump;
    private bool jumping; // The player has hit the jump button and not yet returned to the ground
    private int jumpCounter;
    private float gravity;

    private bool nearLadder = false;
    private bool onLadder = false;
    private bool onSlope = false;
    private bool grounded = false;
    private int contactAmount = 0;
    private ContactPoint2D point;

    // Use this for initialization
    void Start()
    {
        myRigid = this.GetComponent<Rigidbody2D>();
        //myBox = this.GetComponent<BoxCollider2D>();
        //myRenderer = this.GetComponent<SpriteRenderer>();
        //defaultSprite = myRenderer.sprite;

        gravity = myRigid.gravityScale;
        platformSpeed = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float verticalMovement = Input.GetAxis("Vertical");
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
        } // Done with ladders

        if (onLadder || (grounded && onSlope)) // Prevent slipping off of slopes
        {
            myRigid.gravityScale = 0;
        }
        else
        {
            myRigid.gravityScale = gravity;
        }

        bool goingLeft = Input.GetAxisRaw("Horizontal") < 0;
        bool goingRight = Input.GetAxisRaw("Horizontal") > 0;
        //Movement (Left / Right)
        if(contactAmount <= 1 && point.point.y < transform.position.y) // on a slope
        {
            if (!grounded && contactAmount == 0)
            {
                print(point.normal);
                if (goingLeft)
                {
                    myRigid.velocity = new Vector2(-slopeVelocity, myRigid.velocity.y);
                }
                else if (goingRight)
                {
                    myRigid.velocity = new Vector2(slopeVelocity, myRigid.velocity.y);
                }
                else { myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
            }
            else if(!goingRight && !goingLeft) // standing still
            {
                if (!grounded) { myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
                else { myRigid.velocity = new Vector2(0, 0); }
            }
            else if (point.point.x < transform.position.x) // on a slope like \
            {
                if (goingLeft)
                {
                    if (Mathf.Abs(point.normal.x) > Mathf.Abs(point.normal.y))
                    {
                        myRigid.velocity = new Vector2(0, 0);
                    }
                    else
                    {
                        myRigid.velocity = new Vector2(-point.normal.y * slopeVelocity, point.normal.x * slopeVelocity);
                    }
                }
                else if (goingRight)
                {
                    myRigid.velocity = new Vector2(point.normal.y * slopeVelocity, -point.normal.x * slopeVelocity);
                }
            }
            else if (point.point.x > transform.position.x) // on a slope like /
            {
                if (goingRight)
                {
                    if (Mathf.Abs(point.normal.x) + .2f > Mathf.Abs(point.normal.y))
                    {
                        myRigid.velocity = new Vector2(0, 0);
                    }
                    else
                    {
                        myRigid.velocity = new Vector2(point.normal.y * slopeVelocity, -point.normal.x * slopeVelocity);
                    }
                }
                else if (goingLeft)
                {
                    myRigid.velocity = new Vector2(-point.normal.y * slopeVelocity, point.normal.x * slopeVelocity);
                }
            }
            if (contactAmount == 1) { onSlope = true; }
            else { onSlope = false; }
        }
        else if (!grounded) // in air
        {
            if (goingLeft)
            {
                myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y);
            }
            else if (goingRight)
            {
                myRigid.velocity = new Vector2(maxHorizontalVelocity, myRigid.velocity.y);
            }
            else{ myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
        }
        // on ground
        else
        {
            if (goingLeft)
            {
                if (!jumping) { myRigid.velocity = new Vector2(-maxHorizontalVelocity, 0) + platformSpeed; }
                else { myRigid.velocity = new Vector2(-maxHorizontalVelocity + platformSpeed.x, myRigid.velocity.y); }
            }
            else if (goingRight)
            {
                if (!jumping) { myRigid.velocity = new Vector2(maxHorizontalVelocity, 0) + platformSpeed; }
                else { myRigid.velocity = new Vector2(maxHorizontalVelocity + platformSpeed.x, myRigid.velocity.y); }
            }
            else{
                if(!jumping) { myRigid.velocity = platformSpeed; }
                myRigid.velocity = new Vector2(platformSpeed.x, myRigid.velocity.y);
            }
        }

        // Trying to jump
        if (verticalMovement > 0)
        {
            /*if (isCrouching)
            {
                isCrouching = false;
                myBox.size = new Vector2(myBox.size.x, myBox.size.y / crouchMultiplier);

                myBox.offset = new Vector2(0, 0);
                myRenderer.sprite = defaultSprite;
            }*/
            if (verticalMovement > 0 && canJump && jumpCounter < maxJumps)
            {
                jump(1.0f);
            }

        }
        // Trying to crouch
        /*else if (verticalMovement < 0 && grounded && (!onLadder && !nearLadder))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                float offset = myBox.size.y * (1.0f - crouchMultiplier) / 2.0f; //How much the bottom boundary is displaced
                myBox.size = new Vector2(myBox.size.x, myBox.size.y * crouchMultiplier);

                myBox.offset = new Vector2(0, -offset);
                myRenderer.sprite = crouchSprite;
            }
        }*/
        canJump = verticalMovement <= 0;
    }

    void jump(float jumpFactor)
    {
        myRigid.velocity = new Vector2(myRigid.velocity.x, 0);
        myRigid.AddForce(new Vector2(0, myRigid.mass * jumpImpulse), ForceMode2D.Impulse);
        jumping = true;
        jumpCounter++;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        contactAmount = col.contactCount;
        point = col.GetContact(0);
        jumping = false;
    }
    void OnCollisionStay2D(Collision2D col)
    {
        contactAmount = col.contactCount;
        point = col.GetContact(0);
        if(point.point.y < transform.position.y) {
            grounded = true;
            jumpCounter = 0;
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        contactAmount = 0;
        grounded = false;
    }
    public void setNearLadder(bool near)
    {
        nearLadder = near;
    }

    public void setPlatformSpeed(float xVelocity, float yVelocity)
    {
        platformSpeed.x = xVelocity;
        platformSpeed.y = yVelocity;
    }

    public void exitPlatform()
    {
        platformSpeed.x = 0;
        platformSpeed.y = 0;
    }
}
