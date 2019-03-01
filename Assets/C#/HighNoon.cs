using System;
using System.Collections.Generic;
using UnityEngine;

public class HighNoon : MonoBehaviour
{
    private PlayerAttributes player = null;
    private WeaponAttributes weapon = null;
    private WeaponAttributes prevWep = null;
    public float activeTime = 6;
    public float HNTimeScale = 0.02f; // How fast time moves in high noon
    public AudioClip tickSound = null;

    public float charge = 100;
    private bool active = false;
    private float startTime;
    private AudioSource audioSource = null;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        player = this.GetComponent<PlayerAttributes>();
    }
    
    private void startHighNoon()
    {
        if (charge < 100)
        {
            return;
        }
        prevWep = player.getActiveWeapon();
        if (weapon == null)
        {
            weapon = player.giveWeapon(WEAPONS.REVOLVER_GOLDEN, false);
        }
        player.setActiveWeapon(weapon);
        audioSource.pitch = .5f;
        weapon.setAmmo(weapon.getClipSize());//Force reload
        startTime = Time.realtimeSinceStartup;
        Time.timeScale = HNTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        active = true;
    }

    private void endHighNoon()
    {
        active = false;
        audioSource.pitch = 1;
        charge = 0;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        player.setActiveWeapon(prevWep);
    }

    public bool isActive()
    {
        return active;
    }

    void Update()
    {
        if(Input.GetButtonDown("Ability") || active)
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
                if(weapon.getAmmo() == 0)
                {
                    endHighNoon();
                }
            }
        }
    }
}
