using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	private Camera cam;
	public float distance;
	public float moveSpeed;
	// Use this for initialization
	void Start ()
	{
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = Positioning();
		if (WanderLeader.leader.running == false)
		{
			//transform.position += transform.right*Time.deltaTime;
			if (Input.GetMouseButton(0))
			{
				float x = Input.GetAxis("Mouse X");
				transform.RotateAround(pos,Vector3.up,x*moveSpeed*Time.deltaTime);
				float y = Input.GetAxis("Mouse Y");
				transform.RotateAround(pos,Vector3.right,y*moveSpeed*Time.deltaTime);
			}
			/*float actualDistance = Vector3.Distance(transform.position, pos);
			float diff = Mathf.Abs(actualDistance - distance);
			if (diff > 5f)
			{
				transform.position = (transform.position - pos).normalized*distance + pos;
			}*/
		}
	}

	Vector3 Positioning()
	{
		Vector3 pos = Vector3.zero;
		int length = WanderLeader.leader.walls.Count;//.positions.Count;
		bool goBack = false;
		foreach (Vector3 posI in WanderLeader.leader.walls)
		{
			pos += posI;
			Vector3 seen = cam.WorldToViewportPoint(posI);
			if (seen.x > 0 && seen.x < 1 && seen.y > 0 && seen.y < 1 && seen.z > 0)
			{
				//chill
			}
			else
			{
				goBack = true;
			}
		}
		pos = new Vector3(pos.x / length, pos.y / length, pos.z / length);
		transform.LookAt(pos);
		if (goBack)
		{
			transform.position += transform.forward * -2f;
			distance = Vector3.Distance(transform.position, pos);
		}

		return pos;
	}
}
