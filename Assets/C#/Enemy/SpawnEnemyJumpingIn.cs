using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyJumpingIn : MonoBehaviour
{
    public float timeUntilSpawn = 2;
    public float xVel = 0, yVel = 4;
    public GameObject EnemyToSpawn;
    public GameObject target;

    private bool start = false;
    private float startTime = 0;
    private Rigidbody2D rig;

    void Start()
    {
        rig = target.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (start)
        {
            if(startTime == 0)
            {
                startTime = Time.time;
                rig.velocity = new Vector2(xVel, yVel);
            }
            if(Time.time - startTime > timeUntilSpawn)
            {
                GameObject enemy = GameObject.Instantiate(EnemyToSpawn);
                enemy.transform.position = target.transform.position;
                start = false;
                DestroyImmediate(target);
                DestroyImmediate(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            start = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            start = true;
        }
    }
}
