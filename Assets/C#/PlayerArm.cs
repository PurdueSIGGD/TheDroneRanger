using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArm : MonoBehaviour {

    public bool doesRotate = true;

    private Camera cam;
    private float weaponAngle;
    private GameObject player;
    private SpriteRenderer playerBody;
    private SpriteRenderer arm;
    private Animator anim;

	void Start () {

        cam = Camera.main;
        player = transform.parent.gameObject;
        anim = this.GetComponentInParent<Animator>();
        playerBody = player.GetComponent<SpriteRenderer>();
        arm = this.GetComponent<SpriteRenderer>();

        Vector2 weaponDir = transform.GetChild(0).position - transform.position;
        weaponAngle = Mathf.Atan2(weaponDir.y, weaponDir.x) * Mathf.Rad2Deg;

	}
	
	void Update () {

        if (doesRotate)
        {

            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            Vector2 mouseDir = new Vector2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y);
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
