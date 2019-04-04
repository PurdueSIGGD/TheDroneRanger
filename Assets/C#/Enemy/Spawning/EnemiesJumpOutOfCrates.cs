using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesJumpOutOfCrates : MonoBehaviour
{
    [SerializeField]
    private Prop[] crates;
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private Vector3[] enemySpawnPoints;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") == true)
        {
            for(int i=0; i<crates.Length; i++)
            {
                crates[i].decreaseDurability(crates[i].durability);
            }
            for(int i=0; i<enemySpawnPoints.Length && i<enemies.Length; i++)
            {
                GameObject enemy = GameObject.Instantiate(enemies[i]);
                enemy.transform.position = enemySpawnPoints[i];
            }
            Destroy(this.gameObject);
        }
    }
}
