using System;
using UnityEngine;

public class ProjectileSpawner : CooldownAbility
{
    public GameObject projectile;
    public float thrust = 1;
    public bool hurtPlayer = false;
    public bool mouseAim = false;
    public bool onlyForward = false;

    private Rigidbody2D myRigid = null;

    public override void cooldown_Start(){ }

    public override void cooldown_Update(){ }

    public override void use_CanUse(){ }

    public override void use_UseAbility()
    {
        Vector2 SpawnPosition = transform.position;
        Vector2 Direction;
        
        if (mouseAim)
        {
            Camera cam = Camera.main;
            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            Direction = (mouseWorldPos - SpawnPosition).normalized;

            if (onlyForward && Vector2.Dot((Vector2)(transform.rotation * Vector2.right), Direction) < 0)
            {
                Direction *= -1.0f;
            }

        }
        else
        {
            Direction = transform.rotation * Vector2.right;
        }
        
        GameObject Bullet = GameObject.Instantiate(projectile);

        Projectile projec = Bullet.GetComponent<Projectile>();
        projec.sourceObj = this.gameObject;

        Bullet.transform.position = SpawnPosition;

        Rigidbody2D rigid = Bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = myRigid.velocity;

        if (Direction.x < 0)
        {
            Bullet.GetComponentInChildren<SpriteRenderer>().flipY = true;
        }

        Bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(Direction.x, Direction.y, 0));
        rigid.AddForce(Direction * thrust, ForceMode2D.Impulse);
        
    }

	public virtual void Start()
    {

        myRigid = this.GetComponentInParent<Rigidbody2D>();

    }
}
