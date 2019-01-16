using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClockTime : MonoBehaviour
{
    private GameObject Player;
    private PlayerAttributes stats;
    private RectTransform rectTransform;

    [SerializeField]
    private GameObject clockHand;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        stats = Player.GetComponent<PlayerAttributes>();
        rectTransform = clockHand.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (stats.isHighNoon())
        {
            rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            float angle = -3.6f * stats.highNoonPercent;
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
