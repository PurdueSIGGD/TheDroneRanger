using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttributes : Attributes {

    //high noon at 100, should not be > 100
    public float highNoonPercent = 0;

    public float getHighNoonPercent() {
        return highNoonPercent;
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