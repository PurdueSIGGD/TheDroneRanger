using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour {

    public float initX = 10;
    public float endX = 0;
    public float speed = 1;
    [SerializeField]
    private Camera cam;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.x < cam.transform.position.x + endX)
        {
            this.transform.position = new Vector3(initX + this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }
	}
}
