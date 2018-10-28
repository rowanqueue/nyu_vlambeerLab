using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
	public GameObject startButton;
	public GameObject restartButton;
	// Update is called once per frame
	void Update () {
		if (WanderLeader.leader.started)
		{
			startButton.SetActive(false);
		}
		else
		{
			startButton.SetActive(true);
		}

		if (WanderLeader.leader.built)
		{
			restartButton.SetActive(true);
		}
		else
		{
			restartButton.SetActive(false);
		}
	}
}
