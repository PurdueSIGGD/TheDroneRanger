using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    private ProjectileSpawner spawner;

	void Start () {

        spawner = this.GetComponent<ProjectileSpawner>();

    }

    void Update () {

        if (Input.GetButton("Fire1"))
        {
            spawner.use();
        }

    }
}
