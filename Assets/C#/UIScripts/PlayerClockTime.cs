using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClockTime : MonoBehaviour
{
    private GameObject Player;
    private HighNoon stats;
    private RectTransform rectTransform;

    [SerializeField]
    private GameObject clockHand;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        stats = Player.GetComponent<HighNoon>();
        rectTransform = clockHand.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (stats.charge >= 100 || stats.charge <= 0)
        {
            rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            float angle = -3.6f * stats.charge;
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
