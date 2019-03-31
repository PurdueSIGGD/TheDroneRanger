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

    void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<BossHealthBar>();
        for (int i = 0; i < bounds.Count; i++)
        {
            bounds[i].isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!boss || !healthBar)
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
        Destroy(this.gameObject);
    }
}
