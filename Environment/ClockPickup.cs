using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPickup : MonoBehaviour
{
    public int Charge;
    private const int maxCharge = 100;
    public bool Max;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        HighNoon attr = collision.GetComponent<HighNoon>();
        if (attr == null) return;
        if (attr.charge >= maxCharge || attr.isActive()) return;
        // You actually pick this up
        if (Max) { attr.charge = maxCharge; }
        else if (attr.charge + Charge > maxCharge) { attr.charge = maxCharge; }
        else { attr.charge += Charge; }
        Destroy(this.gameObject);
    }
}
