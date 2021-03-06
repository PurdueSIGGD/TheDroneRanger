﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxHorizontalVelocity = 10;
    public float diagVelocity = 10;
    public float diagVelocityOffset = .98f;
    public float jumpImpulse = 10.0f;
    public int maxJumps = 1;
    public AudioClip walkLeftSound = null;
    public AudioClip walkRightSound = null;
    public AudioClip jumpSound = null;
    public bool isDead = false;

    private Vector2 platformSpeed;

    private PlayerAttributes player;
    private Rigidbody2D myRigid;
    private AudioSource audioSource = null;
    private Animator anim;
    
    private bool jumping; // The player has hit the jump button and not yet returned to the ground
    private int jumpCounter;
    public float gravity;

    private bool nearLadder = false;
    private bool respawning = false;
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
        audioSource = this.GetComponent<AudioSource>();
        anim = this.GetComponent<Animator>();
        player = this.GetComponent<PlayerAttributes>();

        gravity = myRigid.gravityScale;
        platformSpeed = new Vector2(0, 0);
        slope = new Vector2(.7f, .7f);
    }

    // Update is called once per frame
    void Update()
    {
        if(onSlope && sinceOnSlope <= 0)
        {
            onSlope = false;
            grounded = false;
        }
        if (isDead)
        {
            myRigid.velocity = new Vector2(0, 0);
            return;
        }
        if (!player.canInput()) //Don't allow input
        {
            return;
        }
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

        if ((onSlope && !jumping && !hitCeiling) || onLadder) // Prevent slipping off of slopes
        {
            myRigid.gravityScale = 0;
        }
        else
        {
            myRigid.gravityScale = gravity;
        }

        bool goingLeft = Input.GetAxisRaw("Horizontal") < 0;
        bool goingRight = Input.GetAxisRaw("Horizontal") > 0;
        anim.SetBool("Walking", goingLeft || goingRight);
        bool goingUp = myRigid.velocity.y > 0.1f;
        bool goingDown = myRigid.velocity.y < -0.1f;
        if(!audioSource.isPlaying)
        {
            if (goingRight) audioSource.PlayOneShot(walkRightSound);
            else if (goingLeft) audioSource.PlayOneShot(walkLeftSound);
        }
        //Movement (Left / Right)
        if (!grounded) // in air
        {
            if (goingLeft)
            {
                if (hitWallLeft || hitCeilLeft)
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
                else
                { myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y); }
            }
            else if (goingRight)
            {
                if (hitWallRight || hitCeilRight)
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
                else
                { myRigid.velocity = new Vector2(maxHorizontalVelocity, myRigid.velocity.y); }
            }
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
                { myRigid.velocity = new Vector2(-maxHorizontalVelocity, 0) + platformSpeed; }
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
                { myRigid.velocity = new Vector2(maxHorizontalVelocity, 0) + platformSpeed; }
                else
                { myRigid.velocity = new Vector2(maxHorizontalVelocity, myRigid.velocity.y); }
            }
            else if (jumping || hitCeiling) { myRigid.velocity = new Vector2(0, myRigid.velocity.y); }
            else if (onSlope) { myRigid.velocity = new Vector2(0, 0) + platformSpeed; }
            else { myRigid.velocity = new Vector2(0, myRigid.velocity.y) + platformSpeed; }
        }

        // Trying to jump
        if (verticalMovement > 0)
        {
            if (contactAmount > 0 || onLadder)
            {
                if (verticalMovement > 0 && jumpCounter < maxJumps)
                {
                    jump(1.0f);
                }
            }

        }
        if(sinceOnSlope > 0) { sinceOnSlope--; }
    }

    public void respawn()
    {
        respawning = true;
        grounded = false;
        myRigid.velocity = new Vector2(0, 0);
    }

    void jump(float jumpFactor)
    {
        myRigid.velocity = new Vector2(myRigid.velocity.x, 0);
        myRigid.AddForce(new Vector2(0, myRigid.mass * jumpImpulse), ForceMode2D.Impulse);
        jumping = true;
        jumpCounter++;
        audioSource.PlayOneShot(jumpSound);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        contactAmount = col.contactCount;
        hitCeiling = false;
        hitCeilLeft = false;
        hitCeilRight = false;
        hitWallLeft = false;
        hitWallRight = false;
        ContactPoint2D point = col.GetContact(0);
        if (point.normal.x == 0.0f && point.normal.y == 1.0f) // on flat ground
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
            sinceOnSlope = 1;
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
        if (respawning) { grounded = false; respawning = false; }
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
            if (point.normal.x == 0.0f && point.normal.y == 1.0f) // on flat ground
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
                sinceOnSlope = 1;
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
        if (respawning) { grounded = false; respawning = false; }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        contactAmount = 0;
        grounded = false;
        hitWallLeft = false;
        hitWallRight = false;
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