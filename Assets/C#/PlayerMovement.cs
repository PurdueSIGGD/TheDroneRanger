using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementAcceleration = 2000;
    public float maxHorizontalVelocity = 10;
    public float downSlopeVelocity = 10;
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
        if (nearLadder && !onLadder && Input.GetAxisRaw("Vertical") != 0)
        {
            onLadder = true;
            myRigid.velocity = new Vector2(0, 0);
            myRigid.gravityScale = 0;
        }

        if (onLadder)
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

        // Deal with slopes
        if (grounded || onLadder)
        {
            myRigid.gravityScale = 0;
        }
        else if (!onLadder && !nearLadder)
        {
            myRigid.gravityScale = gravity;
        }

        float verticalMovement = Input.GetAxis("Vertical");

        /*
        float horizontalMovement = movementAcceleration * Input.GetAxis("Horizontal");
        // Add force movement option
        
        if ((horizontalMovement > 0 && myRigid.velocity.x < maxHorizontalVelocity)
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
        */

        //Instant run movement option
        if (!grounded)
        {
            if (Input.GetAxisRaw("Horizontal") < 0) myRigid.velocity = new Vector2(-maxHorizontalVelocity, myRigid.velocity.y);
            else if (Input.GetAxisRaw("Horizontal") > 0) myRigid.velocity = new Vector2(maxHorizontalVelocity, myRigid.velocity.y);
            else myRigid.velocity = new Vector2(0, myRigid.velocity.y);
            if (jumpCounter == 0)
            {
                myRigid.velocity = new Vector2(myRigid.velocity.x, -downSlopeVelocity);
            }
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") < 0) myRigid.velocity = new Vector2(-maxHorizontalVelocity, 0);
            else if (Input.GetAxisRaw("Horizontal") > 0) myRigid.velocity = new Vector2(maxHorizontalVelocity, 0);
            else myRigid.velocity = new Vector2(0, 0);
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
                jump(1.0f);
            }

        }
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

    private void jump(float jumpFactor)
    {
        myRigid.AddForce(new Vector2(0, myRigid.mass * jumpImpulse), ForceMode2D.Impulse);
        jumpCounter++;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 bottom = (Vector2)transform.position + new Vector2(0, -myBox.size.y / 2.0f);
        if (Physics2D.Raycast(bottom, -Vector2.up, 0.01f).collider != null)
        {
            jumpCounter = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Projectile>()) { return; }
        if (col.GetComponent<Ladder>()) { return; }
        grounded = true;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.GetComponent<Projectile>()) { return; }
        if (col.GetComponent<Ladder>()) { return; }
        grounded = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<Projectile>()) { return; }
        if (col.GetComponent<Ladder>()) { return; }
        grounded = false;
    }
    public void setNearLadder(bool near)
    {
        nearLadder = near;
    }
}
