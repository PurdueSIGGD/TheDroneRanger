using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public bool doesRotate = true;
    public Vector2 centerOffset = new Vector2(0, 0);

    private Camera cam;
    private GameObject player;

	void Start () {

        cam = Camera.main;
        player = transform.parent.gameObject;

	}
	
	void Update () {

        if (doesRotate)
        {
            
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            Vector3 center = new Vector3(transform.position.x , transform.position.y, cam.nearClipPlane);
            center.x += centerOffset.x;
            center.y += centerOffset.y;

            int angle = (int)(Mathf.Atan2(mouseWorldPos.y - center.y, mouseWorldPos.x - center.x) * Mathf.Rad2Deg);

            if (Mathf.Abs(angle) > 90)
            {
                transform.rotation = Quaternion.Euler(0, 180, 180 - angle);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

        }

        //Left click
        if (Input.GetMouseButtonDown(0))
        {

            print("Click");

        }

    }
}
