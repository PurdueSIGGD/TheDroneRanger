using System;
using UnityEngine;

public class ProjectileSpawner : CooldownAbility
{
    public GameObject projectile;
    public float thrust = 1;
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

        Bullet.transform.position = SpawnPosition;
        Rigidbody2D rigid = Bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = myRigid.velocity;
        Bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(Direction.x, Direction.y, 0));
        rigid.AddForce(Direction * thrust, ForceMode2D.Impulse);
        
        
    }

	public virtual void Start()
    {

        myRigid = this.GetComponentInParent<Rigidbody2D>();

    }
}
