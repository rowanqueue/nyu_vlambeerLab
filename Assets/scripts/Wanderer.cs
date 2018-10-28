using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
	public Transform parent;
	public bool running;
	public float seedNum;
	private Vector3 blockSize = Vector3.one;

	private int dir = 0;
	// Use this for initialization
	void Start ()
	{
		running = true;
		dir = 0;
		WanderLeader.leader.AddPosition(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (running)
		{
			Move();
		}
	}

	void Move()
	{
		transform.position += WanderLeader.leader.direction[dir];
		if (WanderLeader.leader.positions.Contains(transform.position) == false)
		{
			WanderLeader.leader.AddPosition(transform.position);
		}
		Vector3Int pos = new Vector3Int((int)transform.position.x,(int)transform.position.y,(int)transform.position.z);
		//rooms
		if (Random.value < 1f)
		{
			Debug.Log("room!");
			GenerateRoom();
		}

		if (Random.value < (0.15f - 0.025f * WanderLeader.leader.wanderers.Count))
		{
			WanderLeader.leader.SpawnWanderer(transform.position);
		}
		//direction
		if (Random.value < 0.3f)//change direction
		{
			if (Random.value < 0.2f)//turn
			{
				if (Random.value < 0.5f)
				{
					dir += 1;
				}
				else
				{
					dir -= 1;
				}
			}
			else//turn around
			{
				if (Random.value < 0.5f)
				{
					dir += 2;
				}
				else
				{
					dir -= 2;
				}
			}

			if (dir >= WanderLeader.leader.direction.Length)
			{
				dir -= WanderLeader.leader.direction.Length;
			}

			if (dir < 0)
			{
				dir = WanderLeader.leader.direction.Length - 1;
			}
		}

		if (transform.position.y < 0f && dir == 0)
		{
			dir = Random.Range(1, 6);
		}
	}

	void GenerateRoom()
	{
		//3x3room
		int xLim = 3;
		int yLim = 3;
		int zLim = 3;
		if (Random.value < 1f)
		{
			xLim = Random.Range(2, 6);
			yLim = Random.Range(2, 6);
			zLim = Random.Range(2, 6);
		}
		for (int x = 0; x < xLim; x++)
		{
			for (int y = 0; y < yLim; y++)
			{
				for (int z = 0; z < zLim; z++)
				{
					Vector3Int pos = new Vector3Int((int)transform.position.x+x,(int)transform.position.y+y,(int)transform.position.z+z);
					if (WanderLeader.leader.positions.Contains(pos) == false)
					{
						WanderLeader.leader.AddPosition(pos);
					}
				}
			}
		}
	}
}
