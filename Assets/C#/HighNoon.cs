using System;
using System.Collections.Generic;
using UnityEngine;

public class HighNoon : MonoBehaviour
{
    public GameObject Player;
    public ProjectileSpawner gun;
    public float activeTime;

    private float charge = 100;
    private int shotsLeft = 6;
    private bool active = false;
    private float startTime;

    private void startHighNoon()
    {
        print("active");
        //if (charge < 100) return;
        charge = 0;
        startTime = Time.realtimeSinceStartup;
        Time.timeScale = 0.02f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        active = true;
    }

    private void endHighNoon()
    {
        shotsLeft = 6;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        active = false;
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
                //TODO add highnoon implimentation
                if (Input.GetButtonDown("Fire1"))
                {
                    gun.use_UseAbility();
                    shotsLeft--;
                    if(shotsLeft == 0)
                    {
                        endHighNoon();
                    }
                }
            }
        }
    }
}
