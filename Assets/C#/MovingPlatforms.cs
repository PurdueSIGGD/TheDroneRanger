using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
	public float speed;
	public bool loop; //if true move in a loop; if false move back and forth
	public List<Coordinates> movePoints; //the points the platform will travel to

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
		SetPosition(movePoints[0], gameObject);
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
