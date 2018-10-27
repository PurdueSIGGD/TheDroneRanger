using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public int movementSpeed = 3000;
    public int maxHorizontalVelocity = 200;
    public float jumpHeight = 7.0f;
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

        float horizontalMovement = movementSpeed * Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        if((horizontalMovement > 0 && myRigid.velocity.x < maxHorizontalVelocity)
            || (horizontalMovement < 0 && myRigid.velocity.x > -maxHorizontalVelocity))
        {

            myRigid.AddForce(new Vector2(horizontalMovement * Time.deltaTime, 0));

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
                myRigid.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
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

        jumpCounter = 0;

    }

}
