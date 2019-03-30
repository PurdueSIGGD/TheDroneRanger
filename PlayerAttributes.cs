using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttributes : Attributes {

    public KeyCode reloadKey = KeyCode.R;

    public int maxWeapons = 3; //Values <= 0 allow infinite amount

    private List<WeaponAttributes> weapons = new List<WeaponAttributes>();
    private int activeWepSlot = 0;
    private WeaponAttributes activeWep = null;
    private PlayerMovement move = null;
    private Animator anim = null;

    //high noon at 100, should not be > 100
    public float highNoonPercent = 0;
    public float highNoonPerSecond = 2;

    public float invTime;
    public float hurtTime;
    private bool invincible;
    private HighNoon highNoon = null;
    public static GameObject control;
    [SerializeField]
    private GameObject arm;

    public void Awake()
    {
        highNoon = this.GetComponent<HighNoon>();
        if (highNoon == null)
        {
            highNoon = this.gameObject.AddComponent<HighNoon>();
        }
        if(control == null)
        {
            DontDestroyOnLoad(this.gameObject);
            control = this.gameObject;
        }
        else if(control != this.gameObject)
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        WeaponAttributes[] preWeps = this.GetComponentsInChildren<WeaponAttributes>();
        for (int i = 0; i < preWeps.Length; i++)
        {
            weapons.Add(preWeps[i]);
        }
        
        //giveWeapon(WEAPONS.BLADE);
        //giveWeapon(WEAPONS.SHOTGUN);
        //giveWeapon(WEAPONS.PLASMA);
        //giveWeapon(WEAPONS.SNIPER);

        activeWep = weapons[0];

        anim = this.GetComponent<Animator>();
        move = this.GetComponent<PlayerMovement>();
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
            if (Input.GetKey(reloadKey))
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
            StartCoroutine("Die");
            return false;
        }

        knockBack();

        return true;
    }

    private void die()
    {
        StartCoroutine("Die");
    }

    private IEnumerator Die()
    {
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        anim.SetBool("IsDead", true);
        anim.SetTrigger("Died");
        if (!anim.GetBool("LookingRight")) { sprite.flipX = false; }
        arm.SetActive(false);
        move.isDead = true;
        yield return new WaitForSeconds(1.3f);
        anim.SetBool("IsDead", false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single); //Load a new scene
        //Restore starting conditions
        health = maxHealth;
        weapons[activeWepSlot].setAmmo(weapons[activeWepSlot].clipSize);
        this.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        arm.SetActive(true);
        move.isDead = false;
        move.respawn();
    }

    public void knockBack()
    {
        knockBack(Vector2.up, 5.0f);
    }

    public void knockBack(Vector2 direction, float strength)
    {
        if (invincible) return;

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

    public WeaponAttributes giveWeapon(WEAPONS wep, bool addToArray = true)
    {
        WeaponAttributes obj = WeaponAttributes.create(wep);
        if (!obj || !giveWeapon(obj, addToArray))
        {
            Destroy(obj.gameObject);
            return null;
        }
        return obj;
    }

    public bool giveWeapon(WeaponAttributes obj, bool addToArray = true)
    {
        if (addToArray && maxWeapons > 0 && weapons.Count >= maxWeapons)
        {
            return false;
        }

        obj.transform.position = weapons[0].transform.position;
        obj.transform.rotation = weapons[0].transform.rotation;
        obj.transform.parent = weapons[0].transform.parent;
        obj.transform.eulerAngles = weapons[0].transform.eulerAngles;
        obj.transform.localScale = weapons[0].transform.localScale;
        obj.gameObject.SetActive(false);

        if (addToArray)
        {
            weapons.Add(obj);
        }
        return true;
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