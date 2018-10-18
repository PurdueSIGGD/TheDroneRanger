using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour {

    private float initX, initY, initZ;
    public float offsetX = -10;
    public float speed = 1;

    // Use this for initialization
    void Start () {
        initX = this.transform.position.x;
        initY = this.transform.position.y;
        initZ = this.transform.position.z;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.x < initX + offsetX)
        {
            this.transform.position = new Vector3(initX, initY, initZ);
        }
	}
}
