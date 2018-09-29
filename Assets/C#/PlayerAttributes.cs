using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttributes {
    private double health;
    //public Weapon currentWeapon;

    //high noon at 1, should not be > 1
    private double highNoonPercent;
    
    //may not be needed, depending on what gets put in Weapon class
    private int ammoCount;

    //default constructor
    public PlayerAttributes() {
        health = 100.0;
        highNoonPercent = 0.0;
        ammoCount = 6;
    }

    //constructor
    public PlayerAttributes(double health, double highNoonPercent, int ammoCount) {
        this.health = health;
        this.highNoonPercent = highNoonPercent;
        this.ammoCount = ammoCount;
    }
}