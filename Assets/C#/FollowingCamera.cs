using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {

    GameObject[] targets;
    GameObject target;

    // Determines the position of the player on the screen (with regards to the camera)
    public float offsetX = 0, offsetY = 0;
    // Determines whether or not the camera will follow the player as he traverses the Y-axis
    bool followYAxis = false;

    private float initY, initZ;

    // Use this for initialization
    void Start()
    {

        // Determine the player to have the camera follow
        targets = GameObject.FindGameObjectsWithTag("Player");
        target = targets[0];
        // Get the initial position of the camera
        initY = this.transform.position.y;
        initZ = this.transform.position.z;
    }

    public void toggleFollowYAxis()
    {
        followYAxis = !followYAxis;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the camera based off of the change in the player's position.
        if (targets != null)
        {
            if (followYAxis)
            {
                transform.position = new Vector3(target.transform.position.x + offsetX, target.transform.position.y + offsetY, initZ);
            }
            else
            {
                transform.position = new Vector3(target.transform.position.x + offsetX, initY, initZ);
            }
        }
    }
}
