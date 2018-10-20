using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stolen shameless from Andrew Lonsway - edited by Michael Beshear
public abstract class CooldownAbility : MonoBehaviour
{
    /**
     * Abstract class for repeated instances of cooldowns.
     * You do not have to override the 'use' metod as a child of this, all you have to do is implement use_CooledDown
     */
    public enum AnimationType { Trigger, Bool, Float };
    public AnimationType animationTriggerType = AnimationType.Trigger;
    public string CAST_STRING = "Cast";

    public float cooldown = 1; // Cooldown, in seconds
    private float lastUse = -100; // Last time we used it, in seconds;
    [HideInInspector]
    public bool hasNotified;

    public void ability_Start()
    {
        cooldown_Start();
    }

    public void use()
    {
        // Clients should not worry about magic draw
        if (Time.time - lastUse > cooldown)
        {
                lastUse = Time.time;
                hasNotified = false;
                use_UseAbility();

        }
    }

    public void ability_Update()
    {
        if (Time.time - lastUse > cooldown && !hasNotified)
        {
            use_CanUse();
            hasNotified = true;
        }
        cooldown_Update();
    }


    /* these are the other methods you must implement. Can be empty, there for your own benefit */
    public abstract void use_UseAbility(); // Called when the input says to use this ability
    public abstract void cooldown_Start(); // Called when the object is alive
    public abstract void cooldown_Update(); // Called once every frame
    public abstract void use_CanUse(); // Called at the exact time the cooldown timer has reset
}

