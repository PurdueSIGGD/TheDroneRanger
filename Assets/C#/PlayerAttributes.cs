using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : Attributes {

    public KeyCode reloadKey = KeyCode.R;

    private List<WeaponAttributes> weapons = new List<WeaponAttributes>();
    private int activeWepSlot = 0;

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
            weapons.Add(preWeps[i]);
        }

        addWeaponByName("Weapons/Revolver");

    }

    public void Update()
    {

        WeaponAttributes activeWep = weapons[activeWepSlot];

        if ((activeWep.rapidFire && Input.GetButton("Fire1")) ||
            (!activeWep.rapidFire && Input.GetButtonDown("Fire1")))
        {
            activeWep.fire();
        }
        else if (Input.GetKeyDown(reloadKey))
        {
            activeWep.reload();
        }else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {

            int newSlot = activeWepSlot + ((Input.GetAxis("Mouse ScrollWheel") > 0f) ? 1 : weapons.Count - 1);
            newSlot %= weapons.Count;

            activeWep.gameObject.SetActive(false);
            activeWep = weapons[newSlot];
            activeWep.gameObject.SetActive(true);

        }

    }

    public WeaponAttributes addWeaponByName(string path)
    {
        GameObject prefab = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
        if (prefab == null)
        {
            return null;
        }

        return addWeapon(prefab);
    }

    public WeaponAttributes addWeapon(GameObject obj)
    {
        obj.transform.position = weapons[0].transform.position;
        obj.transform.rotation = weapons[0].transform.rotation;
        obj.transform.parent = weapons[0].transform.parent;
        obj.transform.eulerAngles = weapons[0].transform.eulerAngles;
        obj.transform.localScale = weapons[0].transform.localScale;
        obj.SetActive(false);

        WeaponAttributes wep = obj.GetComponent<WeaponAttributes>();
        if (wep == null)
        {
            wep = obj.AddComponent<WeaponAttributes>();
        }

        weapons.Add(wep);
        return wep;
    }

    public WeaponAttributes getActiveWeapon()
    {
        return weapons[activeWepSlot];
    }

    public List<WeaponAttributes> getWeaponList()
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