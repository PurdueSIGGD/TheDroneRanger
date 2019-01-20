﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class HighNoon : MonoBehaviour
{
    public GameObject Player;
    public WeaponAttributes weapon;
    private ProjectileSpawner gun;
    public float activeTime = 6;
    public float HNTimeScale = 0.02f; // How fast time moves in high noon

    public float charge = 100;
    private int shotsLeft = 6;
    private bool active = false;
    private float startTime;

    private void startHighNoon()
    {
        gun = weapon.getProjectileSpawner();
        if (charge < 100) return;
        weapon.setAmmo(shotsLeft);
        gun.ability_Start();
        startTime = Time.realtimeSinceStartup;
        Time.timeScale = HNTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        active = true;
    }

    private void endHighNoon()
    {
        charge = 0;
        shotsLeft = 6;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        active = false;
    }

    public bool isActive()
    {
        return active;
    }

    void Update()
    {
        if(Input.GetButton("Ability") || active)
        {
            if (!active)
            {
                startHighNoon();
            }
            else if(Time.realtimeSinceStartup - startTime >= activeTime)
            {
                endHighNoon();
            }
            else
            {
                charge = 100 * ((activeTime + startTime - Time.realtimeSinceStartup) / activeTime);
                //TODO add highnoon implimentation
                if (Input.GetButtonDown("Fire1"))
                {
                    gun.use_UseAbility();
                    shotsLeft--;
                    weapon.setAmmo(shotsLeft);
                    if(shotsLeft == 0)
                    {
                        endHighNoon();
                    }
                }
            }
        }
    }
}