using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : Attributes {

    private List<GameObject> weapons = new List<GameObject>();

    //high noon at 100, should not be > 100
    public float highNoonPercent = 0;

    public void Start()
    {

        WeaponAttributes[] preWeps = this.GetComponentsInChildren<WeaponAttributes>();
        for (int i = 0; i < preWeps.Length; i++)
        {
            if (preWeps[i].gameObject == null)
            {
                continue;
            }
            weapons.Add(preWeps[i].gameObject);
        }

    }

    public void addWeapon(GameObject wep)
    {
        weapons.Add(wep);
    }

    public List<GameObject> getWeaponList()
    {
        return weapons;
    }

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