using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class TutorialBoss : BossTrigger
{
    public GameObject endCamCenter = null;
    private string EXPLOSION_PATH = "Environment/Explosion_Particles";

   void Start()
    {
        base.Start();
    }

    protected override void OnBossFightEnd()
    {
        CameraControl cam = Camera.main.GetComponent<CameraControl>();
        cam.Pan(endCamCenter.transform.position, panDuration);
        cam.zoom(0.85f);
        createExplosion().transform.position = endCamCenter.transform.position;
    }

    GameObject createExplosion()
    {
        return (Instantiate(Resources.Load(EXPLOSION_PATH, typeof(GameObject))) as GameObject);
    }

}
