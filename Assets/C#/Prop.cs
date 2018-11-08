using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour {
    public int durability;
    public bool destroyed = false; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //called to change durability of a prop
    void changeDurability(int durability) {
        this.durability = durability;
    }

    // This should be called when the prop gets shot
    void Destroy() {
        destroyed = true;
    }
}
