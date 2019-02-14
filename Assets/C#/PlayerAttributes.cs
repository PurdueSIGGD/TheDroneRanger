using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : Attributes {

    public KeyCode reloadKey = KeyCode.R;

    public int maxWeapons = 3; //Values <= 0 allow infinite amount

    private List<WeaponAttributes> weapons = new List<WeaponAttributes>();
    private int activeWepSlot = 0;

    //high noon at 100, should not be > 100
    public float highNoonPercent = 0;

    public float invTime;
    public float hurtTime;
    public float kForce;
    private bool invincible;
    private Camera cam;

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

        cam = Camera.main;
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
            activeWepSlot = newSlot;
            activeWep.gameObject.SetActive(true);

        }

    }

    public override bool takeDamage(float damage)
    {
        if (invincible) return true;

        knockBack();
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            return false;
        }
        return true;
    }

    public void knockBack()
    {
        Vector2 direction = Vector2.left;
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        if (mouseWorldPos.x < this.transform.position.x)
        {
            direction = Vector2.right;
        }
        direction += Vector2.up;

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<HighNoon>().enabled = false;

        invincible = true;

        StartCoroutine(reEnable());

        Rigidbody2D myRigid = GetComponent<Rigidbody2D>();
        myRigid.velocity = new Vector2(0, 0);
        myRigid.AddForce(new Vector2(myRigid.mass, myRigid.mass) * kForce * direction, ForceMode2D.Impulse);
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

    public WeaponAttributes addWeaponByName(string path)
    {
        //Returns null if unable to add.
        if (maxWeapons > 0 && weapons.Count >= maxWeapons)
        {
            return null;
        }

        GameObject prefab = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
        if (prefab == null)
        {
            return null;
        }

        return addWeapon(prefab);
    }

    public WeaponAttributes addWeapon(GameObject obj)
    {

        //Returns null if unable to add.
        if (maxWeapons > 0 && weapons.Count >= maxWeapons)
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