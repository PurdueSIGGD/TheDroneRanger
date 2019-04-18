using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    None,
    Target,
    Mouse,
    Position,
    Pan
}

public class CameraControl : MonoBehaviour
{

    public Vector2 offset = new Vector2(0, 3);

    private CameraMode mode = CameraMode.None;
    private GameObject player = null;
    private GameObject target = null;
    private Vector2 position;
    private float panEndTime = 0;
    private Camera cam = null;


    void Start()
    {
        cam = this.GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        Target(player);
    }
    
    void Update()
    {
        if (mode == CameraMode.Target)
        {
            Vector3 targPos = target.transform.position;
            this.transform.position = new Vector3(targPos.x + offset.x, targPos.y + offset.y, this.transform.position.z);
        }
        else if (mode == CameraMode.Mouse)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            transform.position = new Vector3((mousePos.x + 1.2f * player.transform.position.x) / 2.2f, (mousePos.y + 1.2f * player.transform.position.y) / 2.2f, transform.position.z);
        }else if (mode == CameraMode.Position)
        {
            this.transform.position = new Vector3(position.x, position.y, this.transform.position.z);
        }else if (mode == CameraMode.Pan)
        {
            if (Time.time >= panEndTime) //End pan and set position mode
            {
                Position(position);
            }
            else
            {
                float delta = Time.deltaTime;
                float remain = panEndTime - Time.time + delta;
                Vector2 dist = position - (Vector2)this.transform.position;
                dist /= remain;
                Vector3 result = new Vector3(transform.position.x + dist.x * delta, transform.position.y + dist.y * delta, transform.position.z);
                this.transform.position = result;
            }
        }
    }

    public CameraMode getMode()
    {
        return mode;
    }

    public void setMode(CameraMode mode)
    {
        this.mode = mode;
    }
    
    public void Pan(Vector2 dest, float duration)
    {
        setMode(CameraMode.Pan);
        position = dest;
        panEndTime = Time.time + duration;
    }

    public void Target(GameObject target)
    {
        setMode(CameraMode.Target);
        this.target = target;
    }

    public void Position(float x, float y)
    {
        Position(new Vector2(x, y));
    }

    public void Position(Vector2 vec)
    {
        setMode(CameraMode.Position);
        position = vec;
        this.transform.position = new Vector3(vec.x, vec.y, this.transform.position.z);
    }

    public void setSize(float zoom)
    {
        cam.orthographicSize = zoom;
    }

    public float getSize()
    {
        return cam.orthographicSize;
    }

    public void zoom(float scale)
    {
        setSize(getSize() / scale);
    }

}