using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI_Arc : MonoBehaviour
{

    private int count;
    public int maxCount;
    public float xMove;
    public float yMove;
    bool goDown;
    // Use this for initialization
    void Start()
    {
        count = 0;
        goDown = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!goDown)
        {
            if (count <= maxCount / 2)
            {
                Vector3 newPosition = new Vector3((float)(transform.position.x + xMove), (float)(transform.position.y + yMove), transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, 20 * Time.deltaTime);
                count++;
                print("test");
            }
            else if(count < maxCount)
            {
                Vector3 newPosition = new Vector3((float)(transform.position.x + xMove), (float)(transform.position.y - yMove), transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, 20 * Time.deltaTime);
                count++;
                print("test");
            }
            else
            {
                goDown = true;
            }
        }
        else
        {
            if (count >= maxCount / 2)
            {
                Vector3 newPosition = new Vector3((float)(transform.position.x - xMove), (float)(transform.position.y + yMove), transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, 20 * Time.deltaTime);
                count--;
                print("test");
            }
            else if(count > 0)
            {
                Vector3 newPosition = new Vector3((float)(transform.position.x - xMove), (float)(transform.position.y - yMove), transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, 20 * Time.deltaTime);
                count--;
                print("test");
            }
            else
            {
                goDown = false;
            }
        }
    }
}

    
