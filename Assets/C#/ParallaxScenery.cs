using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScenery : MonoBehaviour {

    GameObject[] targets;
    GameObject target;
    // Multipliers that determine how much the scenery moves as the player moves.
    public float xMovementMultiplier = 1, yMovementMultiplier = 0;
    private float initX, initY, initZ;
    private float targetInitX, targetInitY;

    // Use this for initialization
    void Start()
    {
        // Determine the player to have the scenery "follow"
        targets = GameObject.FindGameObjectsWithTag("Player");
        target = targets[0];

        // Get the initial position of the player and the scenery
        targetInitX = target.transform.position.x;
        targetInitY = target.transform.position.y;
        initX = this.transform.position.x;
        initY = this.transform.position.y;
        initZ = this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the scenery based off of the change in the player's position
        if (targets != null)
        {
            transform.position = new Vector3(initX + xMovementMultiplier * (target.transform.position.x - targetInitX), initY + yMovementMultiplier * (target.transform.position.y - targetInitY), initZ);
        }
    }
}
