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

        //TEST weapon below
        GameObject wep2 = Instantiate(Resources.Load("Weapons/Revolver_Future", typeof(GameObject))) as GameObject;
        addWeapon(wep2);
        wep2.transform.localScale *= 1.5f;

    }

    public void Update()
    {
        
        if (Input.GetMouseButtonDown(2))
        {
            weapons[0].SetActive(!weapons[0].activeSelf);
            weapons[1].SetActive(!weapons[1].activeSelf);
        }

    }

    public void addWeapon(GameObject wep)
    {
        wep.transform.position = weapons[0].transform.position;
        wep.transform.rotation = weapons[0].transform.rotation;
        wep.transform.parent = weapons[0].transform.parent;
        wep.transform.eulerAngles = weapons[0].transform.eulerAngles;
        wep.transform.localScale = weapons[0].transform.localScale;
        wep.SetActive(false);
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