using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour {

    public int clipSize = 6;
    public float fireDelay = 0.2f;
    public float reloadDelay = 0.5f;

    public bool rapidFire = false;//Variable used by parent PlayerWeapon class
    public bool oneReload = true;
    public bool fireCancelReload = true;

    public float projectileSpeed = 3;
    public GameObject projectile = null;
    public AudioClip fireSound = null;
    public AudioClip reloadSound = null;

    private int ammoCount = 0;
    private bool reloading = false;

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

    public void Update()
    {
        if (reloading && reloadAbility.canUse())
        {
            if (ammoCount < clipSize)
            {
                ammoCount++;
                reloadAbility.use();
                audioSource.PlayOneShot(reloadSound);
            }
            else
            {
                reloading = false;
            }
        }
    }

    public bool fire()
    {
        //Don't allow fire while reloading
        if (!reloadAbility.canUse() || reloading)
        {
            if (!oneReload && fireCancelReload)
            {
                reloading = false;
            }
            else
            {
                return false;
            }
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
        if (reloading)
            return;

        if (ammoCount < clipSize) {
            reloadAbility.cooldown = reloadDelay;
            if (reloadAbility.use())
            {
                if (oneReload)
                {
                    ammoCount = clipSize;
                }
                else
                {
                    ammoCount++;
                    reloading = true;//Continue reloading after this
                }
                audioSource.PlayOneShot(reloadSound);
            }
       }
    }

    public ProjectileSpawner getProjectileSpawner()
    {
        return projectileSpawner;
    }

}
