using System;
using UnityEngine;

public class ProjectileSpawner : CooldownAbility
{
    public GameObject projectile;
    public float thrust = 10;

    private Camera cam;

    public override void cooldown_Start(){ }

    public override void cooldown_Update(){ }

    public override void use_CanUse(){ }

    public override void use_UseAbility()
    {
        Vector2 SpawnPosition = transform.position;
        Vector2 MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        MousePos = (Vector2)cam.ScreenToWorldPoint(MousePos);
        Vector2 Direction = MousePos - SpawnPosition;
        Direction.Normalize();
        //TODO add targeting and spawn the bullet
        GameObject Bullet = GameObject.Instantiate(projectile);
        Bullet.GetComponent<Projectile>().sourcePlayer = this.gameObject;
        Bullet.GetComponent<Transform>().position = SpawnPosition;
        Bullet.GetComponent<Rigidbody2D>().AddForce(Direction * thrust);
    }

    private void Start()
    {
        cam = Camera.main;
    }

    public virtual void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            use();
        }
    }
}
