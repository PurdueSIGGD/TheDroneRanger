using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class BossTrigger : MonoBehaviour
{
    public Attributes boss = null;
    public GameObject camCenter = null;
    public List<Collider2D> bounds = new List<Collider2D>();
    public float panDuration = 1.0f;
    private BossHealthBar healthBar = null;
    protected PlayerAttributes player = null;
    private bool triggered = false;
    private bool ended = false;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        for (int i = 0; i < bounds.Count; i++)
        {
            bounds[i].isTrigger = true;
        }
    }

    public void setHealthBar(BossHealthBar bar)
    {
        healthBar = bar;
    }

    protected virtual void OnBossFightEnd() { Destroy(this); }

    protected virtual void Update()
    {
        if (triggered && !ended && (player.getHealth() == 0 || !boss || boss.getHealth() == 0))
        {
            healthBar.setBoss(null);
            healthBar.transform.parent.gameObject.SetActive(false);
            for (int i = 0; i < bounds.Count; i++)
            {
                bounds[i].isTrigger = true;
            }
            OnBossFightEnd();
            ended = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !boss || !healthBar)
        {
            return;
        }
        if (other.GetComponent<PlayerAttributes>() == null)
        {
            return;
        }
        if (camCenter)
        {
            CameraControl cam = Camera.main.GetComponent<CameraControl>();
            cam.Pan(camCenter.transform.position, panDuration);
        }
        for (int i = 0; i < bounds.Count; i++)
        {
            bounds[i].isTrigger = false;
        }

        healthBar.setBoss(boss);
        healthBar.transform.parent.gameObject.SetActive(true);
        triggered = true;
    }
}
