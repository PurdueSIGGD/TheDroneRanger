using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : Attributes {

    public KeyCode reloadKey = KeyCode.R;

    public int maxWeapons = 3; //Values <= 0 allow infinite amount

    private List<WeaponAttributes> weapons = new List<WeaponAttributes>();
    private int activeWepSlot = 0;
    private WeaponAttributes activeWep = null;

    //high noon at 100, should not be > 100
    public float highNoonPercent = 0;
    public float highNoonPerSecond = 2;

    public float invTime;
    public float hurtTime;
    private bool invincible;
    private HighNoon highNoon = null;

    public void Start()
    {
        highNoon = this.GetComponent<HighNoon>();
        if (highNoon == null)
        {
            highNoon = this.gameObject.AddComponent<HighNoon>();
        }

        WeaponAttributes[] preWeps = this.GetComponentsInChildren<WeaponAttributes>();
        for (int i = 0; i < preWeps.Length; i++)
        {
            weapons.Add(preWeps[i]);
        }

        addWeaponByName("Weapons/Shotgun");
        addWeaponByName("Weapons/Plasma_Cannon");
        addWeaponByName("Weapons/Sniper");

        activeWep = weapons[0];

    }

    private void iterateWeapon(bool forward)
    {

        int newSlot = activeWepSlot +( forward ? 1 : weapons.Count - 1);
        newSlot %= weapons.Count;

        activeWep.gameObject.SetActive(false);
        activeWep = weapons[newSlot];
        activeWepSlot = newSlot;
        activeWep.gameObject.SetActive(true);

    }

    public void Update()
    {

        if ((activeWep.rapidFire && Input.GetButton("Fire1")) ||
            (!activeWep.rapidFire && Input.GetButtonDown("Fire1")))
        {
            activeWep.fire();
        }
        if (!isHighNoon()) //Only allow outside of high noon
        {
            addHighNoonPercent(Time.deltaTime * highNoonPerSecond);
            if (Input.GetKeyDown(reloadKey))
            {
                activeWep.reload();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0f || Input.GetButtonDown("Switch"))
            {
                iterateWeapon(Input.GetAxis("Mouse ScrollWheel") >= 0f);
            }
        }

    }

    /*
     * Returns true if still alive. 
     */
    public override bool takeDamage(float damage)
    {
        if (invincible) return true;
        
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            return false;
        }
        return true;
    }

    public void knockBack(Vector2 direction, float strength)
    {

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<HighNoon>().enabled = false;

        invincible = true;

        StartCoroutine(reEnable());

        Rigidbody2D myRigid = GetComponent<Rigidbody2D>();
        myRigid.velocity = new Vector2(0, 0);
        myRigid.AddForce(new Vector2(myRigid.mass, myRigid.mass) * strength * direction, ForceMode2D.Impulse);
        myRigid.gravityScale = GetComponent<PlayerMovement>().gravity;
    }

    private IEnumerator reEnable()
    {
        yield return new WaitForSeconds(hurtTime);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<HighNoon>().enabled = true;
        StartCoroutine(unInvincible());
    }

    private IEnumerator unInvincible()
    {
        yield return new WaitForSeconds(invTime);
        invincible = false;
    }

    public WeaponAttributes addWeaponByName(string path, bool addToArray = true)
    {
        //Returns null if unable to add.
        if (addToArray && maxWeapons > 0 && weapons.Count >= maxWeapons)
        {
            return null;
        }

        GameObject prefab = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
        if (prefab == null)
        {
            return null;
        }

        return addWeapon(prefab, addToArray);
    }

    public WeaponAttributes addWeapon(GameObject obj, bool addToArray = true)
    {

        //Returns null if unable to add.
        if (addToArray && maxWeapons > 0 && weapons.Count >= maxWeapons)
        {
            return null;
        }

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

        if (addToArray)
        {
            weapons.Add(wep);
        }
        return wep;
    }

    public void setActiveWeapon(WeaponAttributes wep)
    {
        activeWep.gameObject.SetActive(false);
        wep.gameObject.SetActive(true);
        activeWep = wep;
        activeWepSlot = 0;
        for (int i = 0; i < weapons.Count; i++)
        {
            if (wep == weapons[i])
            {
                activeWepSlot = i;
                break;
            }
        }
    }

    public WeaponAttributes getActiveWeapon()
    {
        return activeWep;
    }

    public List<WeaponAttributes> getWeaponList()
    {
        return weapons;
    }

    public float getHighNoonPercent() {
        return highNoon.charge;
    }
    
    //adds percent to the current high noon percent
    public void addHighNoonPercent(float percent) {
        highNoon.charge = Mathf.Min(100.0f, highNoon.charge + percent);
    }
    
    public bool isHighNoon() {
        return highNoon.isActive();
    }
}