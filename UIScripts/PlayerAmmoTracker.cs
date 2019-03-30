using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoTracker : MonoBehaviour
{
    private PlayerAttributes Player;
    private RawImage img;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        img = this.GetComponent<RawImage>();
    }

    void Update()
    {
        WeaponAttributes wep = Player.getActiveWeapon();
        if (!wep)
        {
            return;
        }

        Rect uvrec = img.uvRect;
        uvrec.width = wep.getAmmo();
        img.uvRect = uvrec;
        img.transform.localScale = new Vector3(wep.getAmmo(), 1.0f, 1.0f);
        img.texture = wep.ammoUI;

    }

}
