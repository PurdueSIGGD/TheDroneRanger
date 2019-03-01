﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum WEAPONS
{
    REVOLVER,
    REVOLVER_FUTURE,
    REVOLVER_GOLDEN,
    BLADE,
    SHOTGUN,
    PLASMA,
    SNIPER,
    MAX_WEAPONS
};

public class WeaponAttributes : MonoBehaviour {

    //Order is important here
    private static string[] WEAPON_PATHS =
    {
        "Weapons/Revolver",
        "Weapons/Revolver_Future",
        "Weapons/Revolver_Golden",
        "Weapons/Revolver_Blade",
        "Weapons/Shotgun",
        "Weapons/Plasma_Cannon",
        "Weapons/Sniper"
    };

    public int clipSize = 6;
    public float fireDelay = 0.2f;
    public float reloadDelay = 0.5f;

    public bool rapidFire = false;//Variable used by parent PlayerWeapon class
    public bool oneReload = true;
    public bool autoReload = false;
    public bool fireCancelReload = true;
    public bool canZoom = false;

    public float projectileSpeed = 3;
    public GameObject projectile = null;
    public Texture ammoUI = null;
    public AudioClip fireSound = null;
    public AudioClip reloadSound = null;
    public AudioClip emptySound = null;

    private int ammoCount = 0;
    private bool reloading = false;

    protected WEAPONS type = 0;
    private ProjectileSpawner projectileSpawner = null;
    private CooldownAbility reloadAbility = null;
    private AudioSource audioSource = null;
    private AudioSource emptySoundSource = null;
    private AlternateCamera altCam = null;
    private FollowingCamera followCam = null;

    public static WeaponAttributes create(WEAPONS wep)
    {
        if (wep < 0 || wep >= WEAPONS.MAX_WEAPONS)
        {
            return null;
        }

        WeaponAttributes prefab = (Instantiate(Resources.Load(WEAPON_PATHS[(int)wep], typeof(GameObject))) as GameObject).GetComponent<WeaponAttributes>();
        if (!prefab)
        {
            return null;
        }

        prefab.type = wep;
        return prefab;
    }

    void Start () {

        GameObject cam = Camera.main.gameObject;
        followCam = cam.GetComponentInChildren<FollowingCamera>();
        if (!followCam)
        {
            followCam = cam.AddComponent<FollowingCamera>();
            followCam.enabled = false;
        }
        altCam = cam.GetComponentInChildren<AlternateCamera>();
        if (!altCam)
        {
            altCam = cam.AddComponent<AlternateCamera>();
            altCam.enabled = false;
        }

        projectileSpawner = this.gameObject.GetComponentInParent<ProjectileSpawner>();
        if (projectileSpawner == null)
        {
            projectileSpawner = this.gameObject.AddComponent<ProjectileSpawner>();
        }

        audioSource = this.GetComponentInParent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }
        emptySoundSource = this.gameObject.AddComponent<AudioSource>();
        emptySoundSource.playOnAwake = false;

        projectileSpawner.thrust = projectileSpeed;
        projectileSpawner.projectile = projectile;
        projectileSpawner.cooldown = fireDelay;
        projectileSpawner.mouseAim = true;
        projectileSpawner.onlyForward = true;

        reloadAbility = this.gameObject.AddComponent<GenericAbility>();
        reloadAbility.cooldown = reloadDelay;

        ammoCount = clipSize;//Start full

	}

    void OnDestroy()
    {
        try
        {
            altCam.enabled = false;
            followCam.enabled = true;
        }
        catch (Exception){
            //Prevent errors if destroyed together
        }
    }

    public void setCooldown(float delay)
    {
        projectileSpawner.cooldown = delay;
        fireDelay = delay;
    }

    public int getAmmo()
    {

        return ammoCount;

    }

    public void setAmmo(int amount)
    {
        ammoCount = Mathf.Min(clipSize, amount);
    }

    public int getClipSize()
    {
        return clipSize;
    }

    public void setClipSize(int amount)
    {
        clipSize = amount;
        ammoCount = Mathf.Min(clipSize, ammoCount);
    }

    public WEAPONS getType()
    {
        return type;
    }

    public void Update()
    {
        if (reloading && reloadAbility.canUse())
        {
            if (oneReload)
            {
                ammoCount = clipSize;
            }
            else
            {
                ammoCount++;
            }
            if (ammoCount < clipSize)
            {
                reloadAbility.use();
                audioSource.PlayOneShot(reloadSound);
            }
            else
            {
                reloading = false;
            }
        }

        if (autoReload && projectileSpawner.canUse() && ammoCount < clipSize)
        {
            reload();
        }

        if (canZoom && (Input.GetButtonDown("Fire2") || Input.GetButtonUp("Fire2")))//On press or release
        {
            bool pressed = Input.GetButton("Fire2");
            altCam.enabled = pressed;
            followCam.enabled = !pressed;
        }

    }

    public bool fire()
    {
        if (fireCancelReload)
        {
            reloading = false;
        }
        else if (!reloadAbility.canUse())
        {
            return false;
        }

        if (ammoCount <= 0)
        {
            emptySoundSource.clip = emptySound;
            if (!emptySoundSource.isPlaying)
            {
                emptySoundSource.Play();
            }
            return false;
        }

        if (projectileSpawner.use())
        {
            ammoCount--;
            audioSource.PlayOneShot(fireSound);

            return true;
        }

        return false;
    }

    public void reload()
    {
        if (reloading || !projectileSpawner.canUse())
            return;

        if (ammoCount < clipSize) {
            reloadAbility.cooldown = reloadDelay;
            if (reloadAbility.use())
            {
                reloading = true;
                audioSource.PlayOneShot(reloadSound);
            }
       }
    }

    public ProjectileSpawner getProjectileSpawner()
    {
        return projectileSpawner;
    }

}
