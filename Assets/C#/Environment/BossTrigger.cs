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
    private PlayerAttributes player = null;
    private bool triggered = false;

    protected void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<BossHealthBar>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        for (int i = 0; i < bounds.Count; i++)
        {
            bounds[i].isTrigger = true;
        }
    }

    protected virtual void OnBossFightEnd() { }

    void Update()
    {
        if (player.getHealth() == 0 || boss.getHealth() == 0)
        {
            healthBar.setBoss(null);
            healthBar.transform.parent.gameObject.SetActive(false);
            for (int i = 0; i < bounds.Count; i++)
            {
                bounds[i].isTrigger = true;
            }
            OnBossFightEnd();
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !boss || !healthBar)
        {
            return;
        }
        if (camCenter)
        {
            CameraControl cam = Camera.main.GetComponent<CameraControl>();
            cam.Pan(camCenter.transform.position, panDuration);//Pan to center in 2 seconds
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
