using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private const float fill = 1; // the health bar's image can take any value from 0 to 1

    private float fillAmount = 1;

    [SerializeField]
    private Image healthBar;
    
    void Update()
    {
        healthBar.fillAmount = fillAmount;
    }

    public void updateHealth(float newValue, float minValue, float maxValue)
    {
        float amount = (newValue - minValue) / (maxValue - minValue);
        this.fillAmount = amount * fill;
    }
}
