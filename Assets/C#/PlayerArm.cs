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
        weaponAngle = Mathf.Atan2(weaponDir.y, weaponDir.x) * Mathf.Rad2Deg / 2.0f;

	}
	
	void Update () {

        if (doesRotate)
        {

            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Vector2 mouseDir = (mouseWorldPos - (Vector2)this.transform.position).normalized;
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
