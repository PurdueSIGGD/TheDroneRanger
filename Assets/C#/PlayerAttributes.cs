using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttributes : Attributes {

    public KeyCode reloadKey = KeyCode.R;
    public GameObject weaponPosition = null;

    public int maxWeapons = 3; //Values <= 0 allow infinite amount

    private List<WeaponAttributes> weapons = new List<WeaponAttributes>();
    private int activeWepSlot = 0;
    private WeaponAttributes activeWep = null;
    private PlayerMovement move = null;
    private Animator anim = null;
    
    public float highNoonPerSecond = 2;

    public float invTime;
    public float hurtTime;
    private bool invincible;
    private bool allowInput = true;
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

        activeWep = weapons[0];

        anim = this.GetComponent<Animator>();
        move = this.GetComponent<PlayerMovement>();
    }

    public void enableInput(bool enable)
    {
        allowInput = enable;
        if (!enable && isHighNoon())
        {
            highNoon.endHighNoon();
        }
    }

    public bool canInput()
    {
        return allowInput;
    }

    private void iterateWeapon(bool forward)
    {
        if(weapons.Count == 0)
        {
            return;
        }

        int newSlot = activeWepSlot +( forward ? 1 : weapons.Count - 1);
        newSlot %= weapons.Count;

        activeWep.gameObject.SetActive(false);
        activeWep = weapons[newSlot];
        activeWepSlot = newSlot;
        activeWep.gameObject.SetActive(true);

    }

    public void dropActiveWeapon()
    {
        if (weapons.Count == 0)
        {
            return;
        }
        activeWep.drop();
        weapons.RemoveAt(activeWepSlot);
        if (weapons.Count > 0)
        {
            int newSlot = activeWepSlot % weapons.Count;
            activeWep = weapons[newSlot];
            activeWepSlot = newSlot;
            activeWep.gameObject.SetActive(true);
        }
        else
        {
            activeWep = null;
        }
    }

    public void Update()
    {
        if (!activeWep || !allowInput)
        {
            return;
        }
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
            else if (Input.GetKeyDown(KeyCode.G))
            {
                dropActiveWeapon();
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

        Transform trans = this.transform;
        if (weaponPosition)
        {
            trans = weaponPosition.transform;
        }

        obj.transform.position = trans.position;
        obj.transform.rotation = trans.rotation;
        obj.transform.parent = trans.parent;
        obj.transform.eulerAngles = trans.eulerAngles;
        obj.transform.localScale = trans.localScale;
        obj.setOwner(this);

        if (weapons.Count > 0)
        {
            obj.gameObject.SetActive(false);
        }
        else
        {
            activeWep = obj;
            activeWepSlot = 0;
        }

        if (addToArray)
        {
            weapons.Add(obj);
        }
        return true;
    }

    public void setActiveWeapon(WeaponAttributes wep)
    {
        if (activeWep)
        {
            activeWep.gameObject.SetActive(false);
        }
        activeWep = wep;
        if (!wep)
        {
            return;
        }
        wep.gameObject.SetActive(true);
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