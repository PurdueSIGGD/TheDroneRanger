﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
	public float speed;
	public float pauseTime; //the platform can pause at each node to make it easier to mount and/or dismount
	public bool loop; //if true move in a loop; if false move back and forth
	public List<Coordinates> movePoints; //the points the platform will travel to

	private Vector3 lastPosition;

	private Rigidbody2D rb2d;

	[System.Serializable]
	public class Coordinates : System.Object
	{
		public float x;
		public float y;

		public Coordinates()
		{
			x = 0f;
			y = 0f;
		}

		public Coordinates(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public float getX()
		{
			return x;
		}
		public float getY()
		{
			return y;
		}
	}
	public void SetPosition(Coordinates c, GameObject thing)
	{
		thing.transform.position = new Vector3(movePoints[0].getX(), movePoints[0].getY(), thing.transform.position.z);
	}

	void Start()
    {
		rb2d = GetComponent<Rigidbody2D>();
		SetPosition(movePoints[0], gameObject);
		lastPosition = transform.position;
		if (!loop)
		{
			StartCoroutine(MoveBackAndForth());
		} else
		{
			StartCoroutine(MoveLoop());
		}
	}

	IEnumerator MoveBackAndForth()
	{
		while (true)
		{
			//moving forwards through the points
			for (int i = 0; i < movePoints.Count; i++)
			{
				Vector3 nextNode = new Vector3(movePoints[i].getX(), movePoints[i].getY(), 0f);
				while (transform.position != nextNode)
				{
					transform.position = Vector3.MoveTowards(transform.position, nextNode, speed * Time.deltaTime);
					yield return new WaitForEndOfFrame();
				}
				yield return new WaitForSeconds(pauseTime);
			}

			//moving backwards through the points
			for (int i = movePoints.Count - 1; i >= 0; i--)
			{
				Vector3 nextNode = new Vector3(movePoints[i].getX(), movePoints[i].getY(), 0f);
				while (transform.position != nextNode)
				{
					transform.position = Vector3.MoveTowards(transform.position, nextNode, speed * Time.deltaTime);
					yield return new WaitForEndOfFrame();
				}
				yield return new WaitForSeconds(pauseTime);
			}
		}
	}

	IEnumerator MoveLoop()
	{
		while (true)
		{
			//moving forwards through the points
			for (int i = 0; i < movePoints.Count; i++)
			{
				Vector3 nextNode = new Vector3(movePoints[i].getX(), movePoints[i].getY(), 0f);
				while (transform.position != nextNode)
				{
					transform.position = Vector3.MoveTowards(transform.position, nextNode, speed * Time.deltaTime);
					yield return new WaitForEndOfFrame();
				}
			}
		}
	}
	
}
