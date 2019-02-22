using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shamlessly stolen from Andrew Lonsway - edited by Clayton Detke
public class Projectile : MonoBehaviour {
    [HideInInspector]
    public GameObject sourceObj;

    public float damage = 1;
    public float lifetime = 4;
    public bool dieOnHit = true;
    public bool hurtPlayer = false;
    public float gracePeriod = 0.2f;//Seconds allowed before self-harm
    public int copyAmount = 0; //Amount of copies to create, as used by shotgun
    public float spreadAngle = 45.0f;

    private bool hasHit;
    private float spawnTime = 0.0f;
    private GameObject player;

    // Use this for initialization
    void Start () {

        Invoke("DestroyMe", lifetime);
        player = GameObject.Find("Player");

        if (copyAmount > 0)
        {
            int loops = copyAmount;
            copyAmount = 0;//Set to 0 so that the new spawns don't copy
            Vector2 velo = this.GetComponent<Rigidbody2D>().velocity;

            for (int i = 0; i < loops; i++)
            {
                Rigidbody2D body = GameObject.Instantiate(this.gameObject).GetComponent<Rigidbody2D>();
                body.gameObject.name = this.gameObject.name;
                float angle = Random.Range(-spreadAngle/2.0f, spreadAngle/2.0f) * Mathf.Deg2Rad;
                float sin = Mathf.Sin(angle);
                float cos = Mathf.Cos(angle);
                body.velocity = new Vector2(cos * velo.x - sin * velo.y, sin * velo.x + cos * velo.y);
            }

            copyAmount = loops;
        }

        spawnTime = Time.time;

    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        /* CHECKS FOR HIT VALIDIDTY */
        if (hasHit && dieOnHit) return; // We only want to hit one object... for some reason it collides multiple times before destroying itself
        if (col.isTrigger) return; // Only want our own trigger effects
        if (!sourceObj) return;
        if (col == sourceObj.GetComponent<Collider2D>()) return;
        if (col.GetComponent<Projectile>()) return; // Ignore other projectiles
        Attributes attr;
        Prop p;
        attr = col.GetComponentInParent<Attributes>();
		if (attr is PlayerAttributes)
        {
            if (!hurtPlayer && attr.gameObject == player) return;
            if (hurtPlayer && Time.time - spawnTime < gracePeriod) return;

        }
        else if ((p = col.GetComponentInParent<Prop>()))
        {
            if (!p.isDestroyed())
            {
                p.decreaseDurability(damage);
                destroyThis();
            }
            else { return; }
        }
        /* ACTIONS TO TAKE POST-HIT */
        hasHit = true;
        //TODO: check to see if we can hit the thing that we collided with
        if (attr != null)
        {
            attr.takeDamage(damage);
        }

        destroyThis();
    }

    private void destroyThis()
    {
        if (dieOnHit)
        {
            //TODO: should I blow up?
            Destroy(this.gameObject);
        }
    }
}
