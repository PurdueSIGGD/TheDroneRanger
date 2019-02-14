using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoTracker : MonoBehaviour
{
    private const float fill = 1; // the ammo count's image can take any value from 0 to 1

    private float fillAmount = 1;
    private PlayerAttributes Player;

    [SerializeField]
    private Image ammo;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
    }

    void Update()
    {
        WeaponAttributes wep = Player.getActiveWeapon();
        if (wep == null)
        {
            return;
        }
        updateHealth(wep.getAmmo(), 0, wep.getClipSize());
        ammo.fillAmount = fillAmount;
    }

    public void updateHealth(float newValue, float minValue, float maxValue)
    {
        float amount = (newValue - minValue) / (maxValue - minValue);
        this.fillAmount = amount * fill;
    }
}
