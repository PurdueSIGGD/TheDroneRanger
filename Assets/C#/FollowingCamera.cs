using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {
    
    GameObject[] targets;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");

        if (targets != null)
        {
            GameObject target = targets[0];
            transform.position = new Vector3(target.transform.position.x, this.transform.position.y, this.transform.position.z);
        }
    }
}
