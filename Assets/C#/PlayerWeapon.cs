using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public bool doesRotate = true;
    public Vector2 centerOffset = new Vector2(0, 0);
    public Vector2 pivot = new Vector2(0, 0);
    public float recoilForce = 10.0f;

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
        Vector2 plCenter = new Vector2(player.transform.position.x, player.transform.position.y);

        Vector2 mouseDir = new Vector2(mouseWorldPos.x - plCenter.x, mouseWorldPos.y - plCenter.y);
        mouseDir.Normalize();

        Vector2 weapDir = new Vector2(transform.position.x - plCenter.x, transform.position.y - plCenter.y);
        float angle = Vector2.Angle(weapDir, mouseDir);

        Vector2 delta = Quaternion.Euler(0.0f, 0.0f, Vector2.Angle(mouseDir, new Vector2(1, 0))) * centerOffset;

        Vector3 pos = new Vector3(plCenter.x, plCenter.y, transform.position.z);
        pos.x += delta.x;
        pos.y += delta.y;
        transform.position = pos;

        /*
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
        */
    }
}
