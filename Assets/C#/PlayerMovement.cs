using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementAcceleration = 2000;
    public float maxHorizontalVelocity = 10;
    public float diagVelocity = 10;
    public float diagVelocityOffset = .98f;
    public float jumpImpulse = 10.0f;
    public int maxJumps = 1;
    private Vector2 platformSpeed;

    //public float crouchMultiplier = 0.5f; //Value collider height is multiplied by
    //public Sprite crouchSprite;
    //private Sprite defaultSprite;

    private Rigidbody2D myRigid;
    private BoxCollider2D myBox;
    //private SpriteRenderer myRenderer;

    //private bool isCrouching = false;

    private bool canJump;
    private bool jumping; // The player has hit the jump button and not yet returned to the ground
    private int jumpCounter;
    private float gravity;

    private bool nearLadder = false;
    private bool onLadder = false;
    private bool grounded = false;
    private bool onSlope = false;
    private int sinceOnSlope = 0;
    private bool hitCeiling = false;
    private bool hitCeilLeft = false;
    private bool hitCeilRight = false;
    private bool hitWallLeft = false;
    private bool hitWallRight = false;
    private int contactAmount = 0;
    private Vector2 slope; // The normal vector of the slope that we are on.

    // Use this for initialization
    void Start()
    {
        myRigid = this.GetComponent<Rigidbody2D>();
        myBox = this.GetComponent<BoxCollider2D>();
        //myRenderer = this.GetComponent<SpriteRenderer>();
        //defaultSprite = myRenderer.sprite;

        gravity = myRigid.gravityScale;
        platformSpeed = new Vector2(0, 0);
        slope = new Vector2(.7f, .7f);
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
        }

        if ((grounded && !hitCeiling && !hitWallLeft && !hitWallRight) || onLadder) // Prevent slipping off of slopes
        {
            myRigid.gravityScale = 0;
        }
        else
        {
            myRigid.gravityScale = gravity;
        }

        bool goingLeft = Input.GetAxisRaw("Horizontal") < 0;
        bool goingRight = Input.GetAxisRaw("Horizontal") > 0;
        bool goingUp = myRigid.velocity.y > 0.1f;
        bool goingDown = myRigid.velocity.y < -0.1f;
        //Movement (Left / Right)
        if (!grounded) // in air
        {
            if (goingLeft)
            {
                if (hitWallLeft)
                { myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
                else if (jumping)
                { myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y); }
                else if (goingUp && sinceOnSlope > 0 && !hitCeiling && contactAmount < 2 && slope.x != 0) // up a \ slope
                {
                    if (slope.x > slope.y)
                    { myRigid.velocity = new Vector2(0, 0); }
                    else
                    { myRigid.velocity = new Vector2(-slope.y * diagVelocity, slope.x * diagVelocity * diagVelocityOffset); }
                }
                else if (contactAmount >= 2)
                { myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y); }
                else
                { myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y - (diagVelocity / 5)); }
            }
            else if (goingRight)
            {
                if (hitWallRight)
                { myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
                else if (jumping)
                { myRigid.velocity = new Vector2(maxHorizontalVelocity, myRigid.velocity.y); }
                else if (goingUp && sinceOnSlope > 0 && !hitCeiling && contactAmount < 2 && slope.x != 0) // up a / slope
                {
                    if (-slope.x > slope.y)
                    { myRigid.velocity = new Vector2(0, 0); }
                    else
                    { myRigid.velocity = new Vector2(slope.y * diagVelocity, -slope.x * diagVelocity * diagVelocityOffset); }
                }
                else if (contactAmount >= 2)
                { myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y); }
                else
                { myRigid.velocity = new Vector2(maxHorizontalVelocity, myRigid.velocity.y - (diagVelocity / 5)); }
            }
            else if (!jumping)
            { myRigid.velocity = new Vector2(0, myRigid.velocity.y - (diagVelocity / 5)); }
            else
            { myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
        }
        else // on ground
        {
            if (goingLeft)
            {
                if (hitCeilLeft) { myRigid.velocity = new Vector2(0, 0); }
                else if (hitWallLeft) { myRigid.velocity = new Vector2(-myRigid.velocity.x, 0); }
                else if ((goingUp || goingDown || onSlope) && !jumping && !hitCeiling && contactAmount < 2)
                {
                    if (slope.x > slope.y)
                    { myRigid.velocity = new Vector2(0, 0); }
                    else
                    { myRigid.velocity = new Vector2(-slope.y * diagVelocity, slope.x * diagVelocity); }
                }
                else if (!jumping)
                { myRigid.velocity = new Vector2(-maxHorizontalVelocity, 0); }
                else
                { myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y); }
            }
            else if (goingRight)
            {
                if (hitCeilRight) { myRigid.velocity = new Vector2(0, 0); }
                else if (hitWallRight) { myRigid.velocity = new Vector2(-myRigid.velocity.x, 0); }
                else if ((goingUp || goingDown || onSlope) && !jumping && !hitCeiling && contactAmount < 2)
                {
                    if (-slope.x > slope.y)
                    { myRigid.velocity = new Vector2(0, 0); }
                    else
                    { myRigid.velocity = new Vector2(slope.y * diagVelocity, -slope.x * diagVelocity); }
                }
                else if (!jumping)
                { myRigid.velocity = new Vector2(maxHorizontalVelocity, 0); }
                else
                { myRigid.velocity = new Vector2(maxHorizontalVelocity, myRigid.velocity.y); }
            }
            else if (jumping || hitCeiling) { myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
            else { myRigid.velocity = new Vector2(0, 0); }
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
        sinceOnSlope--;
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
        hitCeiling = false;
        hitCeilLeft = false;
        hitCeilRight = false;
        hitWallLeft = false;
        hitWallRight = false;
        ContactPoint2D point = col.GetContact(0);
        if (point.normal.x == 0 && point.normal.y == 1) // on flat ground
        {
            grounded = true;
            jumpCounter = 0;
            jumping = false;
        }
        else if(point.normal.x > .9f)
        {
            hitWallLeft = true;
        }
        else if (point.normal.x < -.9f)
        {
            hitWallRight = true;
        }
        else if (point.normal.y > 0) // on a slope
        {
            jumping = false;
            jumpCounter = 0;
            grounded = true;
            onSlope = true;
            sinceOnSlope = 5;
            slope = point.normal;
        }
        else if (point.normal.y < 0) // hit a ceiling
        {
            hitCeiling = true;
            jumping = false;
            if (point.normal.x > 0)
            {
                hitCeilLeft = true;
            }
            else if (point.normal.x < 0)
            {
                hitCeilRight = true;
            }
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
        grounded = false;
        contactAmount = col.contactCount;
        ContactPoint2D point;
        onSlope = false;
        hitCeiling = false;
        hitCeilLeft = false;
        hitCeilRight = false;
        hitWallLeft = false;
        hitWallRight = false;
        for (int i = 0; i < contactAmount; i++)
        {
            point = col.GetContact(i);
            if (point.normal.x == 0 && point.normal.y == 1) // on flat ground
            {
                grounded = true;
            }
            else if (point.normal.x > .9f)
            {
                hitWallLeft = true;
            }
            else if (point.normal.x < -.9f)
            {
                hitWallRight = true;
            }
            else if (point.normal.y > 0) // on a slope
            {
                grounded = true;
                onSlope = true;
                sinceOnSlope = 5;
                slope = point.normal;
                jumpCounter = 0;
            }
            else if (point.normal.y < 0) // hit a ceiling
            {
                hitCeiling = true;
                jumping = false;
                if (point.normal.x > 0)
                {
                    hitCeilLeft = true;
                }
                else if (point.normal.x < 0)
                {
                    hitCeilRight = true;
                }
            }
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
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