using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArm : MonoBehaviour {

    public bool doesRotate = true;
    
    private float weaponAngle;
    private GameObject player;
    private SpriteRenderer playerBody;
    private SpriteRenderer arm;
    private Animator anim;

	void Start () {
        
        player = transform.parent.gameObject;
        anim = this.GetComponentInParent<Animator>();
        playerBody = player.GetComponent<SpriteRenderer>();
        arm = this.GetComponent<SpriteRenderer>();

        Vector2 weaponDir = transform.GetChild(0).position - transform.position;
        weaponAngle = Mathf.Atan2(weaponDir.y, weaponDir.x) * Mathf.Rad2Deg / 2.0f;

	}
	
	void Update () {

        if (doesRotate)
        {

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Vector2 mouseDir = (mouseWorldPos - (Vector2)this.transform.position).normalized;
            float yRotation = 0.0f;

            if (mouseWorldPos.x < player.transform.position.x)
            {
                anim.SetBool("LookingRight", false);
                playerBody.flipX = true;
                arm.sortingOrder = -2;
                playerBody.sortingOrder = 2;
                yRotation = 180.0f;
                mouseDir.x *= -1;
            }
            else
            {
                anim.SetBool("LookingRight", true);
                playerBody.flipX = false;
                arm.sortingOrder = 2;
                playerBody.sortingOrder = 0;
            }

            player.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            transform.rotation = Quaternion.Euler(0, yRotation, Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg - weaponAngle);

        }

    }
}
