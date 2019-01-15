using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttributes : Attributes {

    [SerializeField]
    private PlayerHealthBar healthBar;
    //public Weapon currentWeapon;

    //high noon at 100, should not be > 100
    public float highNoonPercent = 0;
    
    //may not be needed, depending on what gets put in Weapon class
    public int ammoCount = 6;
    public int maxAmmo = 6;

    //accessors
    
    public float getHighNoonPercent() {
        return highNoonPercent;
    }

    public int getAmmoCount() {
        return ammoCount;
    }
    
    //mutators
    //returns -1 if reload needed, 1 otherwise
    public int shoot() {
        if (ammoCount != 0) {
            ammoCount--;
            if (ammoCount == 0)
                return -1;
            return 1;
        }
        else {
            ammoCount = maxAmmo;
            return -1;
        }
    }
    
    //adds percent to the current high noon percent
    public void addHighNoonPercent(float percent) {
        highNoonPercent += percent;
    }

    //returns true if highNoonPercent >= 100
    public bool isHighNoon() {
        return (highNoonPercent >= 100.0);
    }

    private void Update()
    {
        this.healthBar.updateHealth(this.health, 0, this.maxHealth);
    }
}