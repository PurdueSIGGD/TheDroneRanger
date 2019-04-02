using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
	/*
	 * A second teleporter prefab needs to be in the scene for this teleporter to work
	 */

	public GameObject correspondingTeleporter;
	private Vector2 teleportTo;
	private bool canTeleport; //player can only teleport again if they exit the teleporter

    void Start()
    {
		canTeleport = true;
		if (correspondingTeleporter.GetComponent<Teleporter>().correspondingTeleporter != null && correspondingTeleporter.GetComponent<Teleporter>().correspondingTeleporter != gameObject)
		{
			Debug.LogError(this.name + "'s corresponding teleporter doesn't match");
		}
		else
		{
			teleportTo = correspondingTeleporter.transform.position;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject player = collision.gameObject;
		if (canTeleport && player.name.Equals("Player"))
		{
			if (correspondingTeleporter != null)
			{
				player.transform.position = teleportTo;
				correspondingTeleporter.GetComponent<Teleporter>().setCanTeleport(false);
			} 
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		GameObject player = collision.gameObject;
		if (player.name.Equals("Player"))
		{
			canTeleport = true;
		}
	}

	public void setCanTeleport(bool b)
	{
		canTeleport = b;
	}
}
