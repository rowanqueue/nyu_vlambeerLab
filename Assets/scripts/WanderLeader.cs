using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WanderLeader : MonoBehaviour
{
	public string seed;
	private float seedNum;
	public static WanderLeader leader;

	public int numBlocks;

	public List<Wanderer> wanderers;

	public Vector3Int[] direction;
	public Vector3Int[] movementdirection;

	public bool running;

	[HideInInspector]
	public List<Vector3> positions;
	[HideInInspector]
	public List<Vector3> walls;
	private List<Cube> cubes;
	public bool started;
	public bool built;
	public Transform parent;
	private bool checking;
	private int n;//which position
	private int j;//which direction
	public Image image;
	public Slider slider;
	// Use this for initialization
	void Awake ()
	{
		cubes = new List<Cube>();
		n = 0;
		checking = true;
		leader = this;
		direction = new Vector3Int[]
		{
			Vector3Int.down, Vector3Int.left, Vector3Int.up, Vector3Int.right, new Vector3Int(0, 0, 1),
			new Vector3Int(0, 0, -1)
		};
		movementdirection = new Vector3Int[26];
		int i = 0;
		for(int x = -1; x < 2;x++)
		{
			for (int y = -1; y < 2; y++)
			{
				for (int z = -1; z < 2; z++)
				{
					Vector3Int dir = new Vector3Int(x,y,z);
					if (dir != Vector3Int.zero)
					{
						movementdirection[i] = dir;
						//Debug.Log(dir);
						i++;
					}
				}
			}
		}
		wanderers = new List<Wanderer>();
		positions = new List<Vector3>();
		walls = new List<Vector3>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (started == false)
		{
			numBlocks = (int)slider.value;
		}
		foreach (Wanderer wanderer in wanderers)
		{
			wanderer.running = running;
		}

		if (running == false && built == false)
		{
			for (int i = 0; i < 10; i++)
			{
				bool found = false;
				Vector3 position = positions[n];
				position = position + movementdirection[j];
				if (positions.Contains(position) == false && walls.Contains(position) == false)
				{
					walls.Add(position);
					GameObject obj = Instantiate(Resources.Load("Cube"), position, Quaternion.identity, parent) as GameObject;
					cubes.Add(obj.GetComponent<Cube>());
					found = true;
				}
				j++;
				if (j >= movementdirection.Length)
				{
					j = 0;
					n++;
				}
				if (n >= positions.Count)
				{
					built = true;
					n = 0;
				}

				if (found)
				{
					break;
				}
			}

			image.fillAmount = (float) n / positions.Count;
			image.color = Color.Lerp(Color.red, Color.green, image.fillAmount);
		}
		if (positions.Count > numBlocks && running)
		{
			running = false;
			int i = 0;

			positions.Sort((IComparer<Vector3>)new sortY());
		}

		if (running == false && built && checking)//built, now make windows,door
		{
			List<Cube> possibleDoors = new List<Cube>();
			foreach (Cube cube in cubes)
			{
				cube.Check();
				if (cube.transform.position.y == -1 && cube.partOfWall && cube.glass == false)
				{
					possibleDoors.Add(cube);
				}
			}

			Vector3 center = AveragePos();
			Cube door = possibleDoors[0];
			foreach (Cube doorCube in possibleDoors)
			{
				float a = Vector3.Distance(center, door.transform.position);
				float b = Vector3.Distance(center, doorCube.transform.position);
				if (b > a)
				{
					door = doorCube;
				}
				//doorCube.mr.material.color = Color.red;	
			}

			door.mr.material.color = Color.red;
			RaycastHit hit;
			Ray ray = new Ray(door.transform.position, Vector3.up);
			Physics.Raycast(ray, out hit);
			if (hit.collider != null)
			{
				Cube cube2 = hit.collider.GetComponent<Cube>();
				if (cube2 != null)
				{
					cube2.mr.material.color = Color.red;
				}
			}
			//make steps in front of door
			Vector3 step = (door.transform.position - center).normalized;
			Vector3[] directions = new Vector3[]{Vector3.right,Vector3.left,Vector3.forward,Vector3.back};
			Vector3 minDir = directions[0];
			foreach (Vector3 dir in directions)
			{
				if (Vector3.Distance(step, dir) < Vector3.Distance(step, minDir))
				{
					minDir = dir;
				}
			}

			Vector3 endPos = door.transform.position + minDir + Vector3.down;
			GameObject obj = Instantiate(Resources.Load("Cube"), endPos, Quaternion.identity, parent) as GameObject;
			obj.GetComponent<Cube>().mr.material.color = Color.blue;

			checking = false;
		}
	}

	public void AddPosition(Vector3 vector)
	{
		positions.Add(vector);
		//Instantiate(Resources.Load("Cube"), vector, Quaternion.identity, parent);
	}

	public void SpawnWanderer(Vector3 vector)
	{
		GameObject wandererObject = Instantiate(Resources.Load("Wanderer"), transform.position, Quaternion.identity, transform) as GameObject;
		Wanderer wanderer = wandererObject.GetComponent<Wanderer>();
		wanderer.seedNum = seedNum;
		wanderers.Add(wanderer);
	}

	private class sortY : IComparer<Vector3>
	{
		int IComparer<Vector3>.Compare(Vector3 a, Vector3 b)
		{
			return a.y.CompareTo(b.y);
		}
	}

	public void Restart()
	{
		SceneManager.LoadScene(0);
	}

	public void Begin()
	{
		GameObject wandererObject = Instantiate(Resources.Load("Wanderer"), transform.position, Quaternion.identity, transform) as GameObject;
		Wanderer wanderer = wandererObject.GetComponent<Wanderer>();
		wanderers.Add(wanderer);
		started = true;
	}

	public void SetNum(int num)
	{
		numBlocks = num;
	}

	public Vector3 AveragePos()
	{
		Vector3 pos = Vector3.zero;
		int length = WanderLeader.leader.positions.Count;
		foreach (Vector3 posI in WanderLeader.leader.positions)
		{
			pos += posI;

		}
		pos = new Vector3(pos.x / length, pos.y / length, pos.z / length);
		return pos;
	}
}
