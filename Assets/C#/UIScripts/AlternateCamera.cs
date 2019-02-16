using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateCamera : MonoBehaviour
{
    GameObject target;

    // Determines whether or not the camera will follow the player as he traverses the Y-axis
    public bool followYAxis = true;

    private float initY, initZ;
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        // Determine the player to have the camera follow
        target = GameObject.FindGameObjectWithTag("Player");
        cam = this.GetComponent<Camera>();
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
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        // Move the camera based off of the change in the player's position.
        if (target != null)
        {
            if (followYAxis)
            {
                transform.position = new Vector3((mousePos.x+1.2f*target.transform.position.x)/2.2f, (mousePos.y+1.2f*target.transform.position.y)/2.2f, initZ);
            }
            else
            {
                transform.position = new Vector3((mousePos.x + 1.2f * target.transform.position.x)/2.2f, initY, initZ);
            }
        }
    }
}
