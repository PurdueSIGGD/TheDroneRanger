using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClockEdge : MonoBehaviour
{
    private GameObject Player;
    private PlayerAttributes stats;

    [SerializeField]
    private Image clockEdge;
    [SerializeField]
    private Sprite notReady;
    [SerializeField]
    private Sprite ready;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        stats = Player.GetComponent<PlayerAttributes>();
    }

    void Update()
    {
        if (stats.isHighNoon())
        {
            clockEdge.sprite = ready;
        }
        else
        {
            clockEdge.sprite = notReady;
        }
    }
}
