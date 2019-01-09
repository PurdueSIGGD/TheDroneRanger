using System;
using System.Collections.Generic;
using UnityEngine;

public class HighNoon : MonoBehaviour
{
    public GameObject Player;
    public GameObject Gun;
    public float activeTime;

    private float charge = 0;
    private int shotsLeft = 6;
    private bool active = false;
    private float startTime;

    private void startHighNoon()
    {
        Debug.Log("active");
        if (charge >= 100) return;
        charge = 0;
        startTime = Time.realtimeSinceStartup;
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        active = true;
    }

    private void endHighNoon()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        active = false;
    }

    public void Update()
    {
        if(Input.GetAxis("Ability") != 0 || active)
        {
            if (!active)
            {
                startHighNoon();
            }
            else if(Time.realtimeSinceStartup - startTime >= activeTime)
            {
                endHighNoon();
            }
            else
            {
                //TODO add highnoon implimentation
            }
        }
    }
}
