using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public bool doesRotate = true;

	void Start () {
		


	}
	
	void Update () {

        if (doesRotate)
        {

            //mousePos values -1.0 to 1.0
            Vector2 mousePos = new Vector2(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f);
            mousePos *= 2.0f;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, (int)angle);

        }

    }
}
