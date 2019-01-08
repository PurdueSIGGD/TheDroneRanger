using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour {

    public int clipSize = 6;
    public float fireDelay = 0.2f;

    public float projectileSpeed = 30;
    public GameObject projectile = null;

    private int ammoCount;

    private ProjectileSpawner projectileSpawner;

    void Start () {

        projectileSpawner = this.gameObject.GetComponentInParent<ProjectileSpawner>();
        if (projectileSpawner == null)
        {
            projectileSpawner = this.gameObject.AddComponent<ProjectileSpawner>();
        }

        projectileSpawner.thrust = projectileSpeed;
        projectileSpawner.hurtPlayer = false;
        projectileSpawner.projectile = projectile;
        projectileSpawner.cooldown = fireDelay;

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
        if (ammoCount <= 0)
        {
            return false;
        }

        if (projectileSpawner.use())
        {
            ammoCount--;
            return true;
        }

        return false;
    }

    public void reload()
    {

        ammoCount = clipSize;

    }

    public ProjectileSpawner getProjectileSpawner()
    {
        return projectileSpawner;
    }

}
