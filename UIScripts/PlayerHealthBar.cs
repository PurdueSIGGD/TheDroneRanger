using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private const float fill = 1; // the health bar's image can take any value from 0 to 1

    private float fillAmount = 1;
    private GameObject Player;
    private PlayerAttributes stats;

    [SerializeField]
    private Image healthBar;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        stats = Player.GetComponent<PlayerAttributes>();
    }

    void Update()
    {
        updateHealth(stats.health, 0, stats.maxHealth);
        healthBar.fillAmount = fillAmount;
    }

    public void updateHealth(float newValue, float minValue, float maxValue)
    {
        float amount = (newValue - minValue) / (maxValue - minValue);
        this.fillAmount = amount * fill;
    }
}
