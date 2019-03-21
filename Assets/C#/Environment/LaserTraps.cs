using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTraps : MonoBehaviour
{
	[Tooltip("All times are in seconds\nDefault times are 1 second")]
	public float laserLength;
	public float onTime;
	public float offTime;
	public bool random;
	public bool randomMaxTime;
	public bool randomMinTime;
	private GameObject laser;

	void Start()
    {
		laser = transform.GetChild(0).gameObject;
		laser.transform.localScale = new Vector3(1f, laserLength, 0f);
		laser.transform.localPosition = new Vector3(0f, 0.32f * laserLength - 0.05f, 0f);
		if (onTime <= 0)
		{
			onTime = 1;
		}
		if (offTime <= 0)
		{
			offTime = 1;
		}
		StartCoroutine(LaserTrapOn());
	}

    IEnumerator LaserTrapOn()
	{
		while (true)
		{
			laser.SetActive(true);
			yield return new WaitForSeconds(onTime);
			laser.SetActive(false);
			yield return new WaitForSeconds(offTime);
		}
	}
}
