using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItemOnDeath : MonoBehaviour
{
    // This is used for when an enemy spawns an item when they die.
    public GameObject DropItem;
    public Vector3 offset;
    public float spawnProbability = 1; // 0 means it will never spawn, 1 means it will always spawn.
    
    public void spawnItem()
    {
        if (Random.value <= spawnProbability)
        {
            GameObject item = GameObject.Instantiate(DropItem);
            item.transform.position = this.transform.position + offset;
        }
    }
}
