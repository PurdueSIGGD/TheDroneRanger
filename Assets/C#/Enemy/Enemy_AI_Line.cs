using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI_Line : MonoBehaviour {

    private int count;
    public int maxCount;
    public float xMove;
    bool goDown;
	// Use this for initialization
	void Start () {
        count = 0;
        goDown = false;
	}
	
	// Update is called once per frame
	void Update () {
  
        if (!goDown) {
            Vector3 newPosition = new Vector3((float) (transform.position.x+xMove ), transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, 20 * Time.deltaTime);
            count++;
            print("test");
            if(count == maxCount)
            {
                goDown = true;
            }
        }
        else
        {
            Vector3 newPosition = new Vector3((float) (transform.position.x-xMove), transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, 20* Time.deltaTime);
            count--;
            print("test");
            if (count == 0)
            {
                goDown = false;
            }
        }
     
       
	}
}
