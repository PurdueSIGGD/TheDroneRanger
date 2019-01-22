using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {
    
    GameObject target;

    // Determines the position of the player on the screen (with regards to the camera)
    public float offsetX = 0, offsetY = 0;
    // Determines whether or not the camera will follow the player as he traverses the Y-axis
    public bool followYAxis = false;

    private float initY, initZ;

    // Use this for initialization
    void Start()
    {

        // Determine the player to have the camera follow
        target = GameObject.FindGameObjectWithTag("Player");
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
        if (target != null)
        {
            float yPos;
            float xPos = target.transform.position.x + offsetX;
            if (followYAxis)
            {
                yPos = target.transform.position.y + offsetY;
            }
            else
            {
                yPos = initY;
            }
            transform.position = new Vector3(xPos, yPos, initZ);
        }
    }
}
