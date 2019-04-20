using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class TutorialBoss : BossTrigger
{
    public GameObject endCamCenter = null;
    public List<GameObject> explosionAreas = new List<GameObject>();
    public float explode_interval = 0.3f;
    public float explode_duration = 6f;
    private string EXPLOSION_PATH = "Environment/ExplosionParticles";
    private string SOUND_PATH = "Sound/TNT_1";
    private bool exploding = false;
    private float first_expl = 0.0f;
    private float last_expl = 0.0f;
    private int last_area_index = 0;
    private CameraControl cam = null;
    private AudioSource music = null;
    private Image canvasImage = null;
    private List<GameObject> particles = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        cam = Camera.main.GetComponent<CameraControl>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        canvasImage = GameObject.FindGameObjectWithTag("UIScreen").GetComponent<Image>();
        
    }

    private void OnExplosionsEnd()
    {
        //Change scene
    }

    protected override void OnBossFightEnd()
    {
        player.enableInput(false);
        music.Stop();
        cam.Pan(endCamCenter.transform.position, panDuration);
        cam.zoom(0.85f);
        exploding = true;
    }

    protected override void Update()
    {
        base.Update();
        if (exploding && cam.getMode() != CameraMode.Pan) //Wait for panning to end before explosions
        {
            canvasImage.color = new Color(0, 0, 0, (Time.time - first_expl) / explode_duration);
            if (first_expl == 0.0f)//First explosion
            {
                createExplosion();
                first_expl = Time.time;
                last_expl = first_expl;
            }else if (Time.time >= first_expl + explode_duration){
                for (int i = 0; i < particles.Count; i++)
                {
                    Destroy(particles[i]);
                }
                particles.Clear();
                OnExplosionsEnd();
                Destroy(this);
            }else if (Time.time >= last_expl + explode_interval)
            {
                createExplosion();
                last_expl = Time.time;
            }
        }
    }

    void createExplosion()
    {
        last_area_index = (last_area_index + 1) % explosionAreas.Count;
        Vector3 position = explosionAreas[last_area_index].transform.position;
        GameObject obj = (Instantiate(Resources.Load(EXPLOSION_PATH, typeof(GameObject))) as GameObject);
        particles.Add(obj);
        obj.transform.position = position;
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        var psmain = ps.main;
        psmain.startSize = 10.0f;
        psmain.startLifetime = 4.0f;
        psmain.duration = 4.0f;
        var emit = ps.emission;
        emit.enabled = true;
        AudioSource audio = obj.AddComponent<AudioSource>();
        audio.clip = Resources.Load(SOUND_PATH, typeof(AudioClip)) as AudioClip;
        audio.Play();

        ps.Play();
    }

}
