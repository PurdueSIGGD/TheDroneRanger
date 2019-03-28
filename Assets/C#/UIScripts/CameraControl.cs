using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraMode
{
    None,
    Default,
    Mouse
}

public class CameraControl : MonoBehaviour
{
    private CameraMode mode = CameraMode.Mouse;
    private GameObject target = null;
    private Camera cam = null;


    void Start()
    {
        cam = this.GetComponent<Camera>();
        target = GameObject.FindGameObjectWithTag("Player");
        //PlayerAttributes player = target.GetComponent<PlayerAttributes>();
        //player.giveWeapon(WEAPONS.SNIPER);
    }
    
    void Update()
    {
        if (mode == CameraMode.Default)
        {
            Vector3 targPos = target.transform.position;
            this.transform.position = new Vector3(targPos.x, this.transform.position.y, this.transform.position.z);
        }
        else if (mode == CameraMode.Mouse)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            transform.position = new Vector3((mousePos.x + 1.2f * target.transform.position.x) / 2.2f, (mousePos.y + 1.2f * target.transform.position.y) / 2.2f, transform.position.z);
        }
    }

    CameraMode getMode()
    {
        return mode;
    }

    void setMode(CameraMode mode)
    {
        this.mode = mode;
    }

    void setTarget(GameObject target)
    {
        this.target = target;
    }
}