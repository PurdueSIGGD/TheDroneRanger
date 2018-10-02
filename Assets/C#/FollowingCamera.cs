using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {

    GameObject[] targets;
    GameObject target;
    public float offsetX = 0, offsetY = 0;
    bool followYAxis = false;
    private float initY, initZ;

    // Use this for initialization
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
        target = targets[0];
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
