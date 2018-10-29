using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
	public MeshRenderer mr;
	public bool glass;
	public bool partOfWall;
	// Use this for initialization
	void Awake ()
	{
		mr = GetComponent<MeshRenderer>();
	}

	public void Check()
	{
		//check for a z wall
		Vector3[] directions = new Vector3[] {Vector3.up, Vector3.down, Vector3.back, Vector3.forward};
		int zCheck = 0;
		foreach (Vector3 v in directions)
		{
			Ray ray = new Ray(transform.position, v);
			if (Physics.Raycast(ray, 1))
			{
				zCheck++;
			}
		}
		//check for a x wall
		directions = new Vector3[] {Vector3.up, Vector3.down, Vector3.left, Vector3.right};
		int xCheck = 0;
		foreach (Vector3 v in directions)
		{
			Ray ray = new Ray(transform.position, v);
			if (Physics.Raycast(ray, 1))
			{
				xCheck++;
			}
		}
		//affect chances for glass
		if (xCheck >= 4 || zCheck >=4)
		{
			partOfWall = true;
			float v = Random.value;
			if (v < 0.5f)
			{
				glass = true;
				//mr.material.color = new Color(0.5f,0.5f,0.5f,0.1f);
				mr.enabled = false;
				float rot = 0;
				if (zCheck >= 4)
				{
					rot = 90;
				}
				Instantiate(Resources.Load("Window"), transform.position,Quaternion.Euler(0,rot,0),transform);
			}
		}
	}
}
