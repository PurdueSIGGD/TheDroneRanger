using System;
using UnityEngine;

public class ProjectileSpawner : CooldownAbility
{
    public GameObject projectile;
    public float thrust = 30;
    public bool hurtPlayer = false;

    private Rigidbody2D myRigid = null;

    public override void cooldown_Start(){ }

    public override void cooldown_Update(){ }

    public override void use_CanUse(){ }

    public override void use_UseAbility()
    {
        Vector2 SpawnPosition = transform.position;
        Vector2 Direction = transform.rotation * Vector2.right;

        //TODO add targeting and spawn the bullet
        GameObject Bullet = GameObject.Instantiate(projectile);
        Projectile projec = Bullet.GetComponent<Projectile>();
        projec.sourceObj = this.gameObject;
        projec.hurtPlayer = hurtPlayer;
        projec.damage = 1;

        Bullet.transform.position = SpawnPosition;
        Rigidbody2D rigid = Bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = myRigid.velocity;
        rigid.AddForce(Direction * thrust);
    }

    private void Start()
    {

        myRigid = this.GetComponentInParent<Rigidbody2D>();

    }
}
