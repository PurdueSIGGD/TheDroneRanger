using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoTracker : MonoBehaviour
{
    private const float fill = 1; // the ammo count's image can take any value from 0 to 1

    private float fillAmount = 1;
    private GameObject Player;
    private PlayerAttributes stats;

    [SerializeField]
    private Image ammo;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        stats = Player.GetComponent<PlayerAttributes>();
    }

    void Update()
    {
        updateHealth(stats.getAmmoCount(), 0, stats.maxAmmo);
        ammo.fillAmount = fillAmount;
    }

    public void updateHealth(float newValue, float minValue, float maxValue)
    {
        float amount = (newValue - minValue) / (maxValue - minValue);
        this.fillAmount = amount * fill;
    }
}
