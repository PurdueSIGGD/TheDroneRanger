using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    private List<AudioSource> music = new List<AudioSource>();
    private Image canvasImage = null;
    private DialogueBox diagBox = null;
    private List<GameObject> particles = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        cam = Camera.main.GetComponent<CameraControl>();
        GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("Music");
        for (int i = 0; i < musicObjs.GetLength(0); i++)
        {
            AudioSource src = null;
            if ((src = musicObjs[i].GetComponent<AudioSource>()) != null)
            {
                music.Add(src);
            }
        }
        canvasImage = GameObject.FindGameObjectWithTag("UIScreen").GetComponent<Image>();
        diagBox = GameObject.FindGameObjectWithTag("UI").GetComponent<DialogueBox>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
    }

    private void OnExplosionsEnd()
    {
        DialogueMessage msg = new DialogueMessage();
        msg.Character = "Scientist";
        msg.Message = "Carefully now, he's a hero.";
        msg.Time = 3.0f;
        diagBox.Message(msg);
        Destroy(player.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    protected override void OnBossFightEnd()
    {
        player.enableInput(false);
        for (int i = 0; i < music.Count; i++)
        {
            music[i].Stop();
        }
        cam.Pan(endCamCenter.transform.position, panDuration);
        cam.zoom(0.85f);
        exploding = true;
    }

    protected override void Update()
    {
        base.Update();
        if (exploding && cam.getMode() != CameraMode.Pan) //Wait for panning to end before explosions
        {
            if (first_expl == 0.0f)//First explosion
            {
                createExplosion();
                first_expl = Time.time;
                last_expl = first_expl;
            }else if (Time.time >= first_expl + explode_duration){
                canvasImage.color = new Color(0, 0, 0, 1.0f);
                if (particles[particles.Count - 1].GetComponent<AudioSource>().isPlaying)
                {
                    return; //Don't end until sounds stop
                }
                for (int i = 0; i < particles.Count; i++)
                {
                    Destroy(particles[i]);
                }
                particles.Clear();
                OnExplosionsEnd();
                Destroy(this);
            }
            else if (Time.time >= last_expl + explode_interval)
            {
                createExplosion();
                last_expl = Time.time;
                canvasImage.color = new Color(0, 0, 0, (Time.time - first_expl) / explode_duration);
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
        psmain.startSize = 5.0f;
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
