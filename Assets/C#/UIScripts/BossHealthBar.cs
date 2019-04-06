using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private const float fill = 1; // the health bar's image can take any value from 0 to 1

    private float fillAmount = 1;
    private Attributes boss = null;

    private Image healthBar = null;

    void Start()
    {
        healthBar = this.GetComponent<Image>();
        setBoss(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>());
        GameObject.FindObjectOfType<BossTrigger>().setHealthBar(this); //Must set because issue with finding inactive objects
        this.transform.parent.gameObject.SetActive(false);
    }

    public void setBoss(Attributes boss)
    {
        this.boss = boss;
    }

    void Update()
    {
        if (!boss)
        {
            this.transform.parent.gameObject.SetActive(false);
            return;
        }
        updateHealth(boss.health, 0, boss.maxHealth);
        healthBar.fillAmount = fillAmount;
    }

    public void updateHealth(float newValue, float minValue, float maxValue)
    {
        float amount = (newValue - minValue) / (maxValue - minValue);
        this.fillAmount = amount * fill;
    }
}
