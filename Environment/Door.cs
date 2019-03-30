using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public Sprite openDoorSprite; //openDoor sprite

    public void OpenDoor()
	{
		this.GetComponent<SpriteRenderer>().sprite = openDoorSprite;
		this.GetComponent<BoxCollider2D>().enabled = false;
	}
}
