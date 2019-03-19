using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleportItemOnDeath : MonoBehaviour
{
    // This is used for when an enemy moves an item that already exists the scene when they die.
    public GameObject teleportItem;
    public Vector3 offset;

    public void moveItem()
    {
        teleportItem.transform.position = this.transform.position + offset;
    }
}
