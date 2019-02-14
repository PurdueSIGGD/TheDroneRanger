using System;
using System.Collections.Generic;
using UnityEngine;

public class HighNoon : MonoBehaviour
{
    public GameObject Player;
    public WeaponAttributes weapon;
    private ProjectileSpawner gun;
    public float activeTime = 6;
    public float HNTimeScale = 0.02f; // How fast time moves in high noon
    public AudioClip tickSound = null;

    public float charge = 100;
    private int shotsLeft = 6;
    private bool active = false;
    private float startTime;
    private AudioSource audioSource = null;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void startHighNoon()
    {
        audioSource.pitch = .5f;
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
        audioSource.pitch = 1;
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
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(tickSound);
                }
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
