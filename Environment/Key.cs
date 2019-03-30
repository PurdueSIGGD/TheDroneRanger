using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
	public GameObject correspondingDoor;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name.Equals("Player"))
		{
			correspondingDoor.GetComponent<Door>().OpenDoor();
			Destroy(this.gameObject);
		}
	}
}
