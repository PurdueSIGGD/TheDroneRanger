using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponentInParent<PlayerMovement>() == null) { return; }
        col.GetComponentInParent<PlayerMovement>().setNearLadder(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponentInParent<PlayerMovement>() == null) { return; }
        col.GetComponentInParent<PlayerMovement>().setNearLadder(false);
    }
}
