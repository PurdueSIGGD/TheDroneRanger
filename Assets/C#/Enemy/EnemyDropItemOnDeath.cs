using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItemOnDeath : MonoBehaviour
{
    // This is used for when an enemy spawns an item when they die.
    public GameObject DropItem;
    public Vector3 offset;
    
    public void spawnItem()
    {
        GameObject item = GameObject.Instantiate(DropItem);
        item.transform.position = this.transform.position + offset;
    }
}
