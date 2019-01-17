using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public KeyCode reloadKey = KeyCode.R;

    private WeaponAttributes weapon;

	void Start () {

        weapon = this.GetComponent<WeaponAttributes>();

    }

    void Update () {

        if ((weapon.rapidFire && Input.GetButton("Fire1")) ||
            (!weapon.rapidFire && Input.GetButtonDown("Fire1")))
        {
            weapon.fire();
        }
        else if (Input.GetKeyDown(reloadKey))
        {
            weapon.reload();
        }

    }
}
