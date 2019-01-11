using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour {

    public int clipSize = 6;
    public float fireDelay = 0.2f;
    public float reloadDelay = 0.5f;

    public bool rapidFire = false;//Variable used by parent PlayerWeapon class

    public float projectileSpeed = 30;
    public GameObject projectile = null;
    public AudioClip fireSound = null;
    public AudioClip reloadSound = null;

    private int ammoCount;

    private ProjectileSpawner projectileSpawner;
    private CooldownAbility reloadAbility = null;
    private AudioSource audioSource = null;

    void Start () {

        projectileSpawner = this.gameObject.GetComponentInParent<ProjectileSpawner>();
        if (projectileSpawner == null)
        {
            projectileSpawner = this.gameObject.AddComponent<ProjectileSpawner>();
        }

        audioSource = this.GetComponent<AudioSource>();

        projectileSpawner.thrust = projectileSpeed;
        projectileSpawner.hurtPlayer = false;
        projectileSpawner.projectile = projectile;
        projectileSpawner.cooldown = fireDelay;

        reloadAbility = this.gameObject.AddComponent<GenericAbility>();
        reloadAbility.cooldown = reloadDelay;

        ammoCount = clipSize;//Start full

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

    public bool fire()
    {
        //Don't allow fire while reloading
        if (!reloadAbility.canUse())
        {
            return false;
        }

        if (ammoCount <= 0)
        {
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
        if (ammoCount < clipSize) {
            reloadAbility.cooldown = reloadDelay;
            if (reloadAbility.use())
            {
                ammoCount = clipSize;
                audioSource.PlayOneShot(reloadSound);
            }
       }

    }

    public ProjectileSpawner getProjectileSpawner()
    {
        return projectileSpawner;
    }

}
