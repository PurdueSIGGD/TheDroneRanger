using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public bool doesRotate = true;
    public Vector2 centerOffset = new Vector2(0, 0);
    public float recoilForce = 150.0f;

    private Camera cam;
    private GameObject player;
    private Rigidbody2D playerRigid;

	void Start () {

        cam = Camera.main;
        player = transform.parent.gameObject;
        playerRigid = player.GetComponent<Rigidbody2D>();

        Vector3 newPos = player.transform.position;
        newPos.x += centerOffset.x;
        newPos.y += centerOffset.y;
        transform.position = newPos;

    }
	
	void Update () {

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        Vector3 center = new Vector3(transform.position.x, transform.position.y, cam.nearClipPlane);

        Vector2 direction = new Vector2(mouseWorldPos.x - center.x, mouseWorldPos.y - center.y);
        direction.Normalize();

        if (doesRotate)
        {

            int angle = (int)(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);


            //Flip sprite when on the other side of player
            if (player.transform.position.x > mouseWorldPos.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 180 - angle);
                player.transform.rotation = Quaternion.Euler(0, 180, 0);

            }
            else
            {

                transform.rotation = Quaternion.Euler(0, 0, angle);
                player.transform.rotation = Quaternion.Euler(0, 0, 0);

            }

        }

        //Left click
        if (Input.GetMouseButtonDown(0))
        {

            playerRigid.AddForce(-direction * recoilForce);

        }

    }
}
