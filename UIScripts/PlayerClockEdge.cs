using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClockEdge : MonoBehaviour
{
    private GameObject Player;
    private HighNoon stats;

    [SerializeField]
    private Image clockEdge;
    [SerializeField]
    private Sprite notReady;
    [SerializeField]
    private Sprite ready;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        stats = Player.GetComponent<HighNoon>();
    }

    void Update()
    {
        if (stats.charge >= 100 || stats.isActive())
        {
            clockEdge.sprite = ready;
        }
        else
        {
            clockEdge.sprite = notReady;
        }
    }
}
