using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttributes {
    private double health;
    //public Weapon currentWeapon;

    //high noon at 100, should not be > 100
    private double highNoonPercent;
    
    //may not be needed, depending on what gets put in Weapon class
    private int ammoCount;
    private int maxAmmo;

    //default constructor
    public PlayerAttributes() {
        health = 100.0;
        highNoonPercent = 0.0;
        ammoCount = 6;
        maxAmmo = 6;
    }

    //constructor
    public PlayerAttributes(double health, double highNoonPercent, int maxAmmo) {
        this.health = health;
        this.highNoonPercent = highNoonPercent;
        this.ammoCount = maxAmmo;
        this.maxAmmo = maxAmmo;
    }

    //accessors
    public double getHealth() {
        return health;
    }

    public double getHighNoonPercent() {
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

    //returns -1 if player is dead, 1 otherwise
    public int takeDamage(double damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            return -1;
        }
        return 1;
    }

    //adds percent to the current high noon percent
    public void addHighNoonPercent(double percent) {
        highNoonPercent += percent;
    }

    //returns true if highNoonPercent >= 100
    public bool isHighNoon() {
        return (highNoonPercent >= 100.0);
    }
}