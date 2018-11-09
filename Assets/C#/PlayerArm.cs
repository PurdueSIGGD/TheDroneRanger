using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArm : MonoBehaviour {

    public bool doesRotate = true;

    private Camera cam;
    private float weaponAngle;
    private GameObject player;

	void Start () {

        cam = Camera.main;
        player = transform.parent.gameObject;

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
                yRotation = 180.0f;
                mouseDir.x *= -1;
            }

            player.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            transform.rotation = Quaternion.Euler(0, yRotation, Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg - weaponAngle);

        }

    }
}
