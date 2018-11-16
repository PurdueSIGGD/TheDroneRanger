using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttributes : Attributes {
    
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
    //returns true if shot, false if no ammo
    public bool shoot() {
        if(ammoCount <= 0)
        {
            return false;
        }
        ammoCount--;
        return true;
    }

    public void reload()
    {
        ammoCount = maxAmmo;
    }
    
    //adds percent to the current high noon percent
    public void addHighNoonPercent(float percent) {
        highNoonPercent += percent;
    }

    //returns true if highNoonPercent >= 100
    public bool isHighNoon() {
        return (highNoonPercent >= 100.0);
    }
}